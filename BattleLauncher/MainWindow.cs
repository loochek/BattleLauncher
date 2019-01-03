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
using System.Threading.Tasks;
using System.Linq;

namespace BattleLauncher
{
    public partial class MainWindow : Form
    {
        private List<ServerInfo> serverList;

        public MainWindow()
        {
            InitializeComponent();
        }

        private bool Cancel = false;
        private bool Busy = false;

        private async void RunGame(string ServerGuid)
        {
            Busy = true;
            toolStripDropDownButton1.Enabled = true;
            toolStripStatusLabel1.Text = "Getting server info...";
            ServerInfo serverInfo = (await Utils.DeserializeResponseAsync<ServerInfoResponse>(await Utils.DoHttpRequestAsync(String.Format(URL.BF3ServerInfoShort, ServerGuid)))).data;
            Globals.CurrentGameId = serverInfo.gameId;

            toolStripStatusLabel1.Text = "Getting user info...";
            PersonaData personaData = (await Utils.DeserializeResponseAsync<PlayablePersonaResponse>(await Utils.DoHttpRequestAsync(URL.BF3PlayablePersona))).data;
            Globals.PersonaId = personaData.personaId;

            toolStripStatusLabel1.Text = "Reservating server slot...";
            ReservationInfo reservation = (await Utils.DeserializeResponseAsync<SlotReservationResponse>(await Utils.DoHttpRequestAsync(String.Format(URL.BF3ReserveSlot, Globals.PersonaId, Globals.CurrentGameId, ServerGuid)))).data;
            bool queued = false;
            if (reservation.joinState == "IN_QUEUE")
                queued = true;

            if (queued)
            {
                while (true)
                {
                    if (Cancel)
                    {
                        toolStripStatusLabel1.Text = "Leaving server...";
                        BattlelogResponse leave = await Utils.DeserializeResponseAsync<BattlelogResponse>(await Utils.DoHttpRequestAsync(String.Format(URL.BF3LeaveServer, Globals.PersonaId, Globals.CurrentGameId)));
                        toolStripStatusLabel1.Text = "Idle";
                        Cancel = false;
                        toolStripDropDownButton1.Enabled = false;
                        Busy = false;
                        return;
                    }
                    try
                    {
                        QueueStatusInfo queueStatusInfo = (await Utils.DeserializeResponseAsync<QueueStatusResponse>(await Utils.DoHttpRequestAsync(String.Format(URL.BF3QueueStatus, Globals.PersonaId, Globals.CurrentGameId)))).data;
                        if (queueStatusInfo.queuePosition == -1)
                            break;
                        toolStripStatusLabel1.Text = String.Format("Queued: {0}", queueStatusInfo.queuePosition);
                    }
                    catch (WebException)
                    {
                        toolStripStatusLabel1.Text = "Queue status unknown";
                    }
                    await Task.Delay(1000);
                }
            }

            toolStripStatusLabel1.Text = "Retrieving auth data...";
            string authJsonRaw;
            authJsonRaw = await Utils.DoHttpRequestAsync(String.Format(URL.BF3AuthData, Globals.PersonaId));
            authJsonRaw = authJsonRaw.Substring(1);
            authJsonRaw = authJsonRaw.Substring(0, authJsonRaw.Length - 2);
            AuthData authData = (await Utils.DeserializeResponseAsync<AuthDataResponse>(authJsonRaw)).data;
            string loginToken = authData.encryptedToken;
            string authCode = authData.authCode;

            if (Cancel)
            {
                toolStripStatusLabel1.Text = "Leaving server...";
                BattlelogResponse leave = await Utils.DeserializeResponseAsync<BattlelogResponse>(await Utils.DoHttpRequestAsync(String.Format(URL.BF3LeaveServer, Globals.PersonaId, Globals.CurrentGameId)));
                toolStripStatusLabel1.Text = "Idle";
                Cancel = false;
                toolStripDropDownButton1.Enabled = false;
                Busy = false;
                return;
            }

            toolStripStatusLabel1.Text = "Starting game...";

            ProcessStartInfo process = new ProcessStartInfo();
            process.FileName = Globals.BF3ExePath;
            process.Arguments = String.Format(Globals.GameArgs, authCode, Globals.CurrentGameId, Globals.PersonaId, loginToken);
            process.WorkingDirectory = Globals.BF3GameDir;
            Process.Start(process);

            bool webHelperStarted = false;
            bool leavedServer = false;
            while (true)
            {
                if (Cancel)
                {
                    if (!leavedServer)
                    {
                        toolStripStatusLabel1.Text = "Leaving server...";
                        BattlelogResponse leave = await Utils.DeserializeResponseAsync<BattlelogResponse>(await Utils.DoHttpRequestAsync(String.Format(URL.BF3LeaveServer, Globals.PersonaId, Globals.CurrentGameId)));
                        leavedServer = true;
                    }
                    toolStripStatusLabel1.Text = "Waiting for game to exit...";
                    try
                    {
                        await Utils.DoLocalHttpRequestAsync(URL.KillGame);
                        toolStripStatusLabel1.Text = "Idle";
                        toolStripDropDownButton1.Enabled = false;
                        Cancel = false;
                        Busy = false;
                        return;
                    }
                    catch (Exception)
                    {
                        await Task.Delay(500);
                        continue;
                    }
                }
                string state;
                try
                {
                    state = await Utils.DoLocalHttpRequestAsync(URL.WebHelper);
                }
                catch (Exception)
                {
                    if (webHelperStarted)
                    {
                        toolStripStatusLabel1.Text = "Idle";
                        toolStripDropDownButton1.Enabled = false;
                        Busy = false;
                        return;
                    }
                    await Task.Delay(500);
                    continue;
                }
                if (state != "null")
                {
                    webHelperStarted = true;
                    toolStripStatusLabel1.Text = state.Replace("\t", " ").Trim();
                    if (state.Contains("GAMEISGONE"))
                    {
                        toolStripStatusLabel1.Text = "Idle";
                        toolStripDropDownButton1.Enabled = false;
                        Busy = false;
                        return;
                    }
                }
            }
        }
        private int ServerIndex2Row(int serverIndex)
        {
            DataGridViewRow row = dataGridView1.Rows.Cast<DataGridViewRow>().Where(r => r.Cells["Index"].Value.Equals(serverIndex)).First();
            return row.Index;
        }

        private async void GetServerList()
        {
            string json = await Utils.DoHttpRequestAsync(String.Format("http://battlelog.battlefield.com/bf3/servers/getAutoBrowseServers/?offset={0}&count=30&post-check-sum=31af753555&filtered=1&expand=1&gameexpansions=0&gameexpansions=512&gameexpansions=2048&gameexpansions=4096&gameexpansions=8192&gameexpansions=16384&q=&premium=-1&ranked=-1&mapRotation=-1&modeRotation=-1&password=-1&settings=&regions=&country=", serverList.Count));
            ServerListResponse resp = await Utils.DeserializeResponse2Async<ServerListResponse>(json);
            foreach (var i in resp.data)
            {
                serverList.Add(i);
                string playerCountString = String.Format("{0}/{1}", i.slots.normal.current, i.slots.normal.max);
                if (i.slots.queued.current != 0)
                    playerCountString = String.Format("{0} [{1}]", playerCountString, i.slots.queued.current);
                dataGridView1.Rows.Add(serverList.Count - 1, i.map, i.name, i.guid, playerCountString, '-');
                UpdatePing(serverList.Count - 1);
            }
        }

        private async void UpdateServerInfo(int serverIndex)
        {
            textBox1.Text = serverList[serverIndex].guid;
            NumPlayersResponse updatedInfo = await Utils.DeserializeResponse2Async<NumPlayersResponse>(await Utils.DoHttpRequestAsync(String.Format(URL.BF3NumPlayersOnServer, serverList[serverIndex].guid)));
            string playerCountString = String.Format("{0}/{1}", updatedInfo.slots.normal.current, updatedInfo.slots.normal.max);
            if (updatedInfo.slots.queued.current != 0)
                playerCountString = String.Format("{0} [{1}]", playerCountString, updatedInfo.slots.queued.current);
            serverList[serverIndex].map = updatedInfo.map;
            serverList[serverIndex].slots = updatedInfo.slots;
            int rowIndex = ServerIndex2Row(serverIndex);
            dataGridView1.Rows[rowIndex].Cells["Map"].Value = updatedInfo.map;
            dataGridView1.Rows[rowIndex].Cells["Players"].Value = playerCountString;
        }

        private async void UpdatePing(int serverIndex)
        {
            int ping;
            if (serverList[serverIndex].ip == "")
                ping = -1;
            else
            {
                Ping p = new Ping();
                ping = (int)(await p.SendPingAsync(serverList[serverIndex].ip)).RoundtripTime;
            }
            dataGridView1.Rows[ServerIndex2Row(serverIndex)].Cells["Ping"].Value = ping;
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            Globals.BF3GameDir = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\EA Games\Battlefield 3", "Install Dir", null);
            Globals.BF4GameDir = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\EA Games\Battlefield 4", "Install Dir", null);
            Globals.SessionToken = "03e73a9a56d830856333ebe9b6a24a72";
            serverList = new List<ServerInfo>();
            GetServerList();
        }

        private void ToolStripDropDownButton1_Click(object sender, EventArgs e)
        {
            Cancel = true;
        }

        private void DataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == -1)
                return;
            if (!Busy)
                RunGame(serverList[(int)dataGridView1.Rows[e.RowIndex].Cells["Index"].Value].guid);
        }

        private void dataGridView1_Scroll(object sender, ScrollEventArgs e)
        {
            if (dataGridView1.DisplayedRowCount(false) + dataGridView1.FirstDisplayedScrollingRowIndex >= dataGridView1.RowCount)
            {
                GetServerList();
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows[0].Index == -1)
                return;
            int serverIndex = (int)dataGridView1.SelectedRows[0].Cells["Index"].Value;
            UpdateServerInfo(serverIndex);
            UpdatePing(serverIndex);
        }
    }
}