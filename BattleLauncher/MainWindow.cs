using System;
using System.ComponentModel;
using System.Net;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Generic;
using BattleLauncher.Battlelog;
using System.Diagnostics;
using Microsoft.Win32;

namespace BattleLauncher
{
    public partial class MainWindow : Form
    {
        private List<ServerInfo> serverList;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            Globals.BF3GameDir = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\EA Games\Battlefield 3", "Install Dir", null);
            Globals.BF4GameDir = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\EA Games\Battlefield 4", "Install Dir", null);
            Globals.SessionToken = "31af75355585b20b7374ae576ea9b323";
            serverList = new List<ServerInfo>();
            string json = Utils.DoHttpXHRRequest("http://battlelog.battlefield.com/bf3/servers/getAutoBrowseServers/?offset=30&count=30&post-check-sum=31af753555&filtered=1&expand=1&gameexpansions=0&gameexpansions=512&gameexpansions=2048&gameexpansions=4096&gameexpansions=8192&gameexpansions=16384&q=&premium=-1&ranked=-1&mapRotation=-1&modeRotation=-1&password=-1&settings=&regions=&country=");
            
            ServerListResponse resp = Utils.DeserializeResponse<ServerListResponse>(json);
            foreach (var i in resp.data)
            {
                serverList.Add(i);
                dataGridView1.Rows.Add(i.map, i.name, i.guid, '-');
            }
        }

        private void ServerJoinWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string serverGuid = e.Argument as string;

            ServerJoinWorker.ReportProgress(0, "Getting server info...");
            ServerInfo serverInfo = Utils.DeserializeResponse<ServerInfoResponse>(Utils.DoHttpRequest(String.Format(URL.BF3ServerInfoShort, serverGuid))).data;
            string gameId = serverInfo.gameId;

            ServerJoinWorker.ReportProgress(0, "Getting user info...");
            PersonaData personaData = Utils.DeserializeResponse<PlayablePersonaResponse>(Utils.DoHttpRequest(URL.BF3PlayablePersona)).data;
            string personaId = personaData.personaId;

            ServerJoinWorker.ReportProgress(0, "Reservating server slot...");
            ReservationInfo reservation = Utils.DeserializeResponse<SlotReservationResponse>(Utils.DoHttpRequest(String.Format(URL.BF3ReserveSlot, personaId, gameId, serverGuid))).data;
            bool queued = false;
            if (reservation.joinState == "IN_QUEUE")
                queued = true;

            ServerJoinWorker.ReportProgress(0, "Retrieving auth data...");
            string authJsonRaw = Utils.DoHttpRequest(String.Format(URL.BF3AuthData, personaId));
            authJsonRaw = authJsonRaw.Substring(1);
            authJsonRaw = authJsonRaw.Substring(0, authJsonRaw.Length - 2);
            AuthData authData = Utils.DeserializeResponse<AuthDataResponse>(authJsonRaw).data;
            string loginToken = authData.encryptedToken;
            string authCode = authData.authCode;
            if (queued)
            {
                while (true)
                {
                    if (ServerJoinWorker.CancellationPending)
                    {
                        ServerJoinWorker.ReportProgress(0, "Cancellation...");
                        BattlelogResponse leave = Utils.DeserializeResponse<BattlelogResponse>(Utils.DoHttpRequest(String.Format(URL.BF3LeaveServer, personaId, gameId)));
                        e.Cancel = true;
                        return;
                    }
                    QueueStatusInfo queueStatusInfo = Utils.DeserializeResponse<QueueStatusResponse>(Utils.DoHttpRequest(String.Format(URL.BF3QueueStatus, personaId, gameId))).data;
                    if (queueStatusInfo.queuePosition == -1)
                        break;
                    ServerJoinWorker.ReportProgress(0, String.Format("Queued: {0}", queueStatusInfo.queuePosition));
                    Thread.Sleep(1000);
                }
            }
            if (ServerJoinWorker.CancellationPending)
            {
                ServerJoinWorker.ReportProgress(0, "Cancellation...");
                BattlelogResponse leave = Utils.DeserializeResponse<BattlelogResponse>(Utils.DoHttpRequest(String.Format(URL.BF3LeaveServer, personaId, gameId)));
                return;
            }
            ServerJoinWorker.ReportProgress(100, "Joined to server");
            e.Result = new Tuple<String, String, String, String>(loginToken, authCode, gameId, personaId);
        }

        private void ServerJoinWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            toolStripStatusLabel1.Text = e.UserState as string;
        }

        private void ToolStripDropDownButton1_Click(object sender, EventArgs e)
        {
            if (GameLaunchWorker.IsBusy)
                GameLaunchWorker.CancelAsync();
            else
                ServerJoinWorker.CancelAsync();
            toolStripDropDownButton1.Enabled = false;
        }

        private void DataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (!ServerJoinWorker.IsBusy)
            {
                ServerJoinWorker.RunWorkerAsync(serverList[e.RowIndex].guid);
                toolStripDropDownButton1.Enabled = true;
            }
        }

        private void ServerJoinWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                GameLaunchWorker.RunWorkerAsync(e.Result);
            }
        }

        private void GameLaunchWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Tuple <String, String, String, String> authData = e.Argument as Tuple<String, String, String, String>;
            string loginToken = authData.Item1;
            string authCode = authData.Item2;
            string gameId = authData.Item3;
            string personaId = authData.Item4;

            GameLaunchWorker.ReportProgress(0, "Starting game...");

            ProcessStartInfo process = new ProcessStartInfo();
            process.FileName = Globals.BF3ExePath;
            process.Arguments = String.Format(Globals.GameArgs, authCode, gameId, personaId, loginToken);
            process.WorkingDirectory = Globals.BF3GameDir;
            Process.Start(process);
            bool webHelperStarted = false;
            bool cancelled = false;
            while (true)
            {
                if (GameLaunchWorker.CancellationPending)
                {
                    if (!cancelled)
                    {
                        GameLaunchWorker.ReportProgress(0, "Cancellation...");
                        BattlelogResponse leave = Utils.DeserializeResponse<BattlelogResponse>(Utils.DoHttpRequest(String.Format(URL.BF3LeaveServer, personaId, gameId)));
                        cancelled = true;
                    }
                    GameLaunchWorker.ReportProgress(0, "Waiting for game to exit...");
                    try
                    {
                        Utils.DoLocalHttpRequest(URL.KillGame);
                    }
                    catch (WebException)
                    {
                        if (webHelperStarted)
                        {
                            e.Cancel = true;
                            return;
                        }
                        else
                        {
                            Thread.Sleep(500);
                            continue;
                        }      
                    }
                }
                string state;
                try
                {
                    state = Utils.DoLocalHttpRequest(URL.WebHelper);
                }
                catch (WebException)
                {
                    Thread.Sleep(500);
                    continue;
                }
                if (state != "null")
                {
                    webHelperStarted = true;
                    GameLaunchWorker.ReportProgress(0, state.Replace("\t", " ").Trim());
                    if (state.Contains("GAMEISGONE"))
                    {
                        e.Cancel = true;
                        return;
                    }
                }   
            }
        }

        private void GameLaunchWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            toolStripStatusLabel1.Text = e.UserState as string;
        }

        private void GameLaunchWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            toolStripDropDownButton1.Enabled = false;
            toolStripStatusLabel1.Text = "Game closed";
        }
    }
}
