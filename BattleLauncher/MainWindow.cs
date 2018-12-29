using System;
using System.ComponentModel;
using System.Net;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Generic;
using BattleLauncher.Battlelog;
using System.Diagnostics;
using Microsoft.Win32;
using System.Net.NetworkInformation;

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
            ServerListRetriever.RunWorkerAsync(serverList.Count);
        }

        private void GameLauncher_DoWork(object sender, DoWorkEventArgs e)
        {
            string serverGuid = e.Argument as string;

            GameLauncher.ReportProgress(0, "Getting server info...");
            ServerInfo serverInfo = Utils.DeserializeResponse<ServerInfoResponse>(Utils.DoHttpRequest(String.Format(URL.BF3ServerInfoShort, serverGuid))).data;
            Globals.CurrentGameId = serverInfo.gameId;

            GameLauncher.ReportProgress(0, "Getting user info...");
            PersonaData personaData = Utils.DeserializeResponse<PlayablePersonaResponse>(Utils.DoHttpRequest(URL.BF3PlayablePersona)).data;
            Globals.PersonaId = personaData.personaId;

            GameLauncher.ReportProgress(0, "Reservating server slot...");
            ReservationInfo reservation = Utils.DeserializeResponse<SlotReservationResponse>(Utils.DoHttpRequest(String.Format(URL.BF3ReserveSlot, Globals.PersonaId, Globals.CurrentGameId, serverGuid))).data;
            bool queued = false;
            if (reservation.joinState == "IN_QUEUE")
                queued = true;

            if (queued)
            {
                while (true)
                {
                    if (GameLauncher.CancellationPending)
                    {
                        GameLauncher.ReportProgress(0, "Leaving server...");
                        BattlelogResponse leave = Utils.DeserializeResponse<BattlelogResponse>(Utils.DoHttpRequest(String.Format(URL.BF3LeaveServer, Globals.PersonaId, Globals.CurrentGameId)));
                        e.Cancel = true;
                        return;
                    }
                    try
                    {
                        QueueStatusInfo queueStatusInfo = Utils.DeserializeResponse<QueueStatusResponse>(Utils.DoHttpRequest(String.Format(URL.BF3QueueStatus, Globals.PersonaId, Globals.CurrentGameId))).data;
                        if (queueStatusInfo.queuePosition == -1)
                            break;
                        GameLauncher.ReportProgress(0, String.Format("Queued: {0}", queueStatusInfo.queuePosition));
                    }      
                    catch (WebException)
                    {
                        GameLauncher.ReportProgress(0, "Queue status unknown");
                    }
                    Thread.Sleep(1000);
                }
            }

            GameLauncher.ReportProgress(0, "Retrieving auth data...");
            string authJsonRaw = Utils.DoHttpRequest(String.Format(URL.BF3AuthData, Globals.PersonaId));
            authJsonRaw = authJsonRaw.Substring(1);
            authJsonRaw = authJsonRaw.Substring(0, authJsonRaw.Length - 2);
            AuthData authData = Utils.DeserializeResponse<AuthDataResponse>(authJsonRaw).data;
            string loginToken = authData.encryptedToken;
            string authCode = authData.authCode;

            if (GameLauncher.CancellationPending)
            {
                GameLauncher.ReportProgress(0, "Leaving server...");
                BattlelogResponse leave = Utils.DeserializeResponse<BattlelogResponse>(Utils.DoHttpRequest(String.Format(URL.BF3LeaveServer, Globals.PersonaId, Globals.CurrentGameId)));
                e.Cancel = true;
                return;
            }

            GameLauncher.ReportProgress(0, "Starting game...");

            ProcessStartInfo process = new ProcessStartInfo();
            process.FileName = Globals.BF3ExePath;
            process.Arguments = String.Format(Globals.GameArgs, authCode, Globals.CurrentGameId, Globals.PersonaId, loginToken);
            process.WorkingDirectory = Globals.BF3GameDir;
            Process.Start(process);
        }

        private void GameLauncher_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            toolStripStatusLabel1.Text = e.UserState as string;
        }

        private void GameLauncher_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
                GameListener.RunWorkerAsync(e.Result);
            else
            {
                Globals.CurrentGameId = "null";
                toolStripStatusLabel1.Text = "Idle";
            }
        }

        private void GameListener_DoWork(object sender, DoWorkEventArgs e)
        {
            bool webHelperStarted = false;
            bool cancelled = false;
            while (true)
            {
                if (GameListener.CancellationPending)
                {
                    if (!cancelled)
                    {
                        GameListener.ReportProgress(0, "Leaving server...");
                        BattlelogResponse leave = Utils.DeserializeResponse<BattlelogResponse>(Utils.DoHttpRequest(String.Format(URL.BF3LeaveServer, Globals.PersonaId, Globals.CurrentGameId)));
                        cancelled = true;
                    }
                    GameListener.ReportProgress(0, "Waiting for game to exit...");
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
                    GameListener.ReportProgress(0, state.Replace("\t", " ").Trim());
                    if (state.Contains("GAMEISGONE"))
                    {
                        e.Cancel = true;
                        return;
                    }
                }   
            }
        }

        private void GameListener_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            toolStripStatusLabel1.Text = e.UserState as string;
        }

        private void GameListener_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            toolStripDropDownButton1.Enabled = false;
            Globals.CurrentGameId = "null";
            toolStripStatusLabel1.Text = "Idle";
        }

        private void ToolStripDropDownButton1_Click(object sender, EventArgs e)
        {
            if (GameListener.IsBusy)
                GameListener.CancelAsync();
            else
                GameLauncher.CancelAsync();
            toolStripDropDownButton1.Enabled = false;
        }

        private void DataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == -1)
                return;
            if (!(GameLauncher.IsBusy || GameListener.IsBusy))
            {
                GameLauncher.RunWorkerAsync(serverList[e.RowIndex].guid);
                toolStripDropDownButton1.Enabled = true;
            }
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == -1)
                return;
            textBox1.Text = serverList[e.RowIndex].guid;
            NumPlayersResponse updatedInfo = Utils.DeserializeResponse2<NumPlayersResponse>(Utils.DoHttpXHRRequest(String.Format(URL.BF3NumPlayersOnServer, serverList[e.RowIndex].guid)));
            string playerCountString = String.Format("{0}/{1}", updatedInfo.slots.normal.current, updatedInfo.slots.normal.max);
            if (updatedInfo.slots.queued.current != 0)
                playerCountString = String.Format("{0} [{1}]", playerCountString, updatedInfo.slots.queued.current);
            dataGridView1.Rows[e.RowIndex].Cells[0].Value = updatedInfo.map;
            dataGridView1.Rows[e.RowIndex].Cells[3].Value = playerCountString;
        }

        private void dataGridView1_Scroll(object sender, ScrollEventArgs e)
        {
            if (dataGridView1.DisplayedRowCount(false) + dataGridView1.FirstDisplayedScrollingRowIndex >= dataGridView1.RowCount)
            {
                ServerListRetriever.RunWorkerAsync(serverList.Count);
            }
        }

        private void ServerListRetriever_DoWork(object sender, DoWorkEventArgs e)
        {
            string json = Utils.DoHttpXHRRequest(String.Format("http://battlelog.battlefield.com/bf3/servers/getAutoBrowseServers/?offset={0}&count=30&post-check-sum=31af753555&filtered=1&expand=1&gameexpansions=0&gameexpansions=512&gameexpansions=2048&gameexpansions=4096&gameexpansions=8192&gameexpansions=16384&q=&premium=-1&ranked=-1&mapRotation=-1&modeRotation=-1&password=-1&settings=&regions=&country=", e.Argument));
            e.Result = Utils.DeserializeResponse<ServerListResponse>(json);
        }

        private void ServerListRetriever_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ServerListResponse resp = e.Result as ServerListResponse;
            foreach (var i in resp.data)
            {
                serverList.Add(i);
                string playerCountString = String.Format("{0}/{1}", i.slots.normal.current, i.slots.normal.max);
                if (i.slots.queued.current != 0)
                    playerCountString = String.Format("{0} [{1}]", playerCountString, i.slots.queued.current);
                dataGridView1.Rows.Add(i.map, i.name, i.guid, playerCountString, '-');
            }
        }

        private void Pinger_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
        }

        private void Pinger_DoWork(object sender, DoWorkEventArgs e)
        {
            List<string> servers = e.Argument as List<string>;
            Ping ping = new Ping();
            foreach (string i in servers)
            {
                PingReply pr = ping.Send(i);
            }
        }

        private void Pinger_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            
        }
    }
}
