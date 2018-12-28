using System;
using System.ComponentModel;
using System.Net;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Collections.Generic;
using BattleLauncher.Battlelog;
using System.Diagnostics;

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
            serverList = new List<ServerInfo>();
            string json = Utils.doHttpXmlRequest("http://battlelog.battlefield.com/bf3/servers/getAutoBrowseServers/?count=30", null);
            
            ServerListResponse resp = Utils.deserializeResponse<ServerListResponse>(json);
            foreach (var i in resp.data)
            {
                serverList.Add(i);
                dataGridView1.Rows.Add(i.map, i.name, i.guid, '-');
            }
        }

        private void serverJoinWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Tuple<string, string> args = e.Argument as Tuple<string, string>;
            string serverGuid = args.Item1;
            string userToken = args.Item2;

            CookieContainer cookies = new CookieContainer();
            cookies.Add(new Uri("http://battlelog.battlefield.com"), new Cookie("beaker.session.id", userToken));

            serverJoinWorker.ReportProgress(0, "Getting server info...");
            ServerInfo serverInfo = Utils.deserializeResponse<ServerInfoResponse>(Utils.doHttpRequest(String.Format("http://battlelog.battlefield.com/bf3/servers/show/pc/{0}/?json=1&join=true", serverGuid), cookies)).data;
            string gameId = serverInfo.gameId;

            serverJoinWorker.ReportProgress(0, "Getting user info...");
            PersonaData personaData = Utils.deserializeResponse<PlayablePersonaResponse>(Utils.doHttpRequest("https://battlelog.battlefield.com/bf3/launcher/playablepersona/", cookies)).data;
            string personaId = personaData.personaId;

            serverJoinWorker.ReportProgress(0, "Reservating server slot...");
            ReservationInfo reservation = Utils.deserializeResponse<SlotReservationResponse>(Utils.doHttpRequest(String.Format("http://battlelog.battlefield.com/bf3/launcher/reserveslotbygameid/1/{0}/{1}/1/{2}/0", personaId, gameId, serverGuid), cookies)).data;
            bool queued = false;
            if (reservation.joinState == "IN_QUEUE")
                queued = true;

            serverJoinWorker.ReportProgress(0, "Retrieving auth data...");
            string authJsonRaw = Utils.doHttpRequest(String.Format("https://battlelog.battlefield.com/bf3/launcher/token/1/{0}", personaId), cookies);
            authJsonRaw = authJsonRaw.Substring(1);
            authJsonRaw = authJsonRaw.Substring(0, authJsonRaw.Length - 2);
            AuthData authData = Utils.deserializeResponse<AuthDataResponse>(authJsonRaw).data;
            string loginToken = authData.encryptedToken;
            string authCode = authData.authCode;
            if (queued)
            {
                while (true)
                {
                    if (serverJoinWorker.CancellationPending)
                    {
                        serverJoinWorker.ReportProgress(0, "Cancellation...");
                        BattlelogXHRResponse leave = Utils.deserializeResponse<BattlelogXHRResponse>(Utils.doHttpRequest(String.Format("http://battlelog.battlefield.com/bf3/launcher/mpleavegameserver/1/{0}/{1}", personaId, gameId), cookies));
                        if (leave.message == "CANCELED")
                            serverJoinWorker.ReportProgress(0, "Successful cancellation");
                        else
                            serverJoinWorker.ReportProgress(0, "Cancellation failed");
                        return;
                    }
                    QueueStatusInfo queueStatusInfo = Utils.deserializeResponse<QueueStatusResponse>(Utils.doHttpRequest(String.Format("http://battlelog.battlefield.com/bf3/launcher/fetchQueueStatus/1/{0}/{1}", personaId, gameId), cookies)).data;
                    if (queueStatusInfo.queuePosition == -1)
                        break;
                    serverJoinWorker.ReportProgress(0, String.Format("Queued: {0}", queueStatusInfo.queuePosition));
                    Thread.Sleep(1000);
                }
            }
            if (serverJoinWorker.CancellationPending)
            {
                serverJoinWorker.ReportProgress(0, "Cancellation...");
                BattlelogXHRResponse leave = Utils.deserializeResponse<BattlelogXHRResponse>(Utils.doHttpRequest(String.Format("http://battlelog.battlefield.com/bf3/launcher/mpleavegameserver/1/{0}/{1}", personaId, gameId), cookies));
                if (leave.message == "CANCELED")
                    serverJoinWorker.ReportProgress(0, "Successful cancellation");
                else
                    serverJoinWorker.ReportProgress(0, "Cancellation failed");
                return;
            }
            serverJoinWorker.ReportProgress(100, "Joined to server");
            e.Result = new Tuple<String, String, String, String>(loginToken, authCode, gameId, personaId);
        }

        private void serverJoinWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            toolStripStatusLabel1.Text = e.UserState as string;
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {
            if (gameLaunchWorker.IsBusy)
                gameLaunchWorker.CancelAsync();
            else
                serverJoinWorker.CancelAsync();
            toolStripDropDownButton1.Enabled = false;
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Tuple<string, string> args = new Tuple<string, string>(serverList[e.RowIndex].guid, "31af75355585b20b7374ae576ea9b323");
            if (!serverJoinWorker.IsBusy)
            {
                serverJoinWorker.RunWorkerAsync(args);
                toolStripDropDownButton1.Enabled = true;
            }
        }

        private void serverJoinWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                gameLaunchWorker.RunWorkerAsync(e.Result);
            }
        }

        private void gameLaunchWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Tuple <String, String, String, String> authData = e.Argument as Tuple<String, String, String, String>;
            string loginToken = authData.Item1;
            string authCode = authData.Item2;
            string gameId = authData.Item3;
            string personaId = authData.Item4;
            string launchArgs = @"-webMode MP -Origin_NoAppFocus --activate-webhelper -AuthCode {0} -requestState State_ClaimReservation -requestStateParams ""<data putinsquad =\""true\"" gameid =\""{1}\"" role =\""soldier\"" personaref =\""{2}\"" levelmode =\""mp\"" logintoken =\""{3}\""></data>"" -Online.BlazeLogLevel 2 -Online.DirtysockLogLevel 2";

            ProcessStartInfo process = new ProcessStartInfo();
            process.FileName = "D:/origin/Battlefield 3/BF3WebHelper.exe";
            process.Arguments = String.Format(launchArgs, authCode, gameId, personaId, loginToken);
            process.WorkingDirectory = "D:/origin/Battlefield 3";
            Process.Start(process);
            while (true)
            {
                if (gameLaunchWorker.CancellationPending)
                {
                    try
                    {
                        Utils.doLocalHttpRequest("http://127.0.0.1:4219/killgame");
                    }
                    catch (WebException)
                    {
                        return;
                    }
                }
                string state;
                try
                {
                    state = Utils.doLocalHttpRequest("http://127.0.0.1:4219");
                }
                catch (WebException)
                {
                    Thread.Sleep(500);
                    continue;
                }
                if (state != "null")
                {
                    gameLaunchWorker.ReportProgress(0, state.Replace("\t", " ").Trim());
                    if (state.Contains("GAMEISGONE"))
                        return;
                }   
            }
        }

        private void gameLaunchWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            toolStripStatusLabel1.Text = e.UserState as string;
        }

        private void gameLaunchWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            toolStripDropDownButton1.Enabled = false;
            toolStripStatusLabel1.Text = "Game closed";
        }
    }
}
