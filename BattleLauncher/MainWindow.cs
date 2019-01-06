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
using System.Reflection;
using static BattleLauncher.Utils;

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

        private SearchFilter currentFilter;

        private Task LeaveServer()
        {
            return Task.Run(async () =>
            {
                toolStripStatusLabel1.Text = "Leaving server...";
                BattlelogResponse leave = await DeserializeResponseAsync<BattlelogResponse>(await DoHttpRequestAsync(String.Format(URL.BF3LeaveServer, Globals.PersonaId, Globals.CurrentGameId)));
            });
            
        }

        private async void RunGame(string ServerGuid)
        {
            Busy = true;
            toolStripDropDownButton1.Enabled = true;
            toolStripStatusLabel1.Text = "Getting server info...";
            ServerInfo serverInfo = (await DeserializeResponseAsync<ServerInfoResponse>(await DoHttpRequestAsync(String.Format(URL.BF3ServerInfoShort, ServerGuid)))).data;
            Globals.CurrentGameId = serverInfo.gameId;

            toolStripStatusLabel1.Text = "Getting user info...";
            PersonaData personaData = (await DeserializeResponseAsync<PlayablePersonaResponse>(await DoHttpRequestAsync(URL.BF3PlayablePersona))).data;
            Globals.PersonaId = personaData.personaId;

            toolStripStatusLabel1.Text = "Reservating server slot...";
            ReservationInfo reservation = (await DeserializeResponseAsync<SlotReservationResponse>(await DoHttpRequestAsync(String.Format(URL.BF3ReserveSlot, Globals.PersonaId, Globals.CurrentGameId, ServerGuid)))).data;
            bool queued = false;
            if (reservation.joinState == "IN_QUEUE")
                queued = true;

            if (queued)
            {
                while (true)
                {
                    if (Cancel)
                    {
                        await LeaveServer();
                        return;
                    }
                    try
                    {
                        QueueStatusInfo queueStatusInfo = (await DeserializeResponseAsync<QueueStatusResponse>(await DoHttpRequestAsync(String.Format(URL.BF3QueueStatus, Globals.PersonaId, Globals.CurrentGameId)))).data;
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
            authJsonRaw = await DoHttpRequestAsync(String.Format(URL.BF3AuthData, Globals.PersonaId));
            authJsonRaw = authJsonRaw.Substring(1);
            authJsonRaw = authJsonRaw.Substring(0, authJsonRaw.Length - 2);
            AuthData authData = (await DeserializeResponseAsync<AuthDataResponse>(authJsonRaw)).data;
            string loginToken = authData.encryptedToken;
            string authCode = authData.authCode;

            if (Cancel)
            {
                await LeaveServer();
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
                        await LeaveServer();
                        leavedServer = true;
                    }
                    toolStripStatusLabel1.Text = "Waiting for game to exit...";
                    try
                    {
                        await DoLocalHttpRequestAsync(URL.KillGame);
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
                    state = await DoLocalHttpRequestAsync(URL.WebHelper);
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
            DataGridViewRow row = serverBrowserView.Rows.Cast<DataGridViewRow>().Where(r => r.Cells["Index"].Value.Equals(serverIndex)).First();
            return row.Index;
        }

        private async void GetServerList()
        {
            currentFilter.offset = serverList.Count;
            string json = await DoHttpRequestAsync(URL.BF3SearchServers + SerializeFilter(currentFilter));
            ServerListResponse resp = await DeserializeResponse2Async<ServerListResponse>(json);
            foreach (var i in resp.data)
            {
                serverList.Add(i);
                string playerCountString = String.Format("{0}/{1}", i.slots.normal.current, i.slots.normal.max);
                if (i.slots.queued.current != 0)
                    playerCountString = String.Format("{0} [{1}]", playerCountString, i.slots.queued.current);
                serverBrowserView.Rows.Add(serverList.Count - 1, GetMapName(i.map), i.name, GetGameModeName(i.mapMode), playerCountString, '-');
                UpdatePing(serverList.Count - 1);
            }
        }

        private async void UpdateServerInfo(int serverIndex)
        {
            textBox1.Text = serverList[serverIndex].guid;
            NumPlayersResponse updatedInfo = await DeserializeResponse2Async<NumPlayersResponse>(await DoHttpRequestAsync(String.Format(URL.BF3NumPlayersOnServer, serverList[serverIndex].guid)));
            string playerCountString = String.Format("{0}/{1}", updatedInfo.slots.normal.current, updatedInfo.slots.normal.max);
            if (updatedInfo.slots.queued.current != 0)
                playerCountString = String.Format("{0} [{1}]", playerCountString, updatedInfo.slots.queued.current);
            serverList[serverIndex].map = updatedInfo.map;
            serverList[serverIndex].slots = updatedInfo.slots;
            int rowIndex = ServerIndex2Row(serverIndex);
            serverBrowserView.Rows[rowIndex].Cells["Map"].Value = GetMapName(updatedInfo.map);
            serverBrowserView.Rows[rowIndex].Cells["Players"].Value = playerCountString;
            serverBrowserView.Rows[rowIndex].Cells["Gamemode"].Value = GetGameModeName(updatedInfo.mapMode);
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
            serverBrowserView.Rows[ServerIndex2Row(serverIndex)].Cells["Ping"].Value = ping;
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            Globals.BF3GameDir = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\EA Games\Battlefield 3", "Install Dir", null);
            Globals.BF4GameDir = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\EA Games\Battlefield 4", "Install Dir", null);
            Globals.SessionToken = "44ec883bb2dd72d84db54efbb2fdd74b";
            this.FilterGM1.Text = Properties.Resources.GM1;
            this.FilterGM2.Text = Properties.Resources.GM2;
            this.FilterGM4.Text = Properties.Resources.GM4;
            this.FilterGM8.Text = Properties.Resources.GM8;
            this.FilterGM32.Text = Properties.Resources.GM32;
            this.FilterGM64.Text = Properties.Resources.GM64;
            this.FilterGM128.Text = Properties.Resources.GM128;
            this.FilterGM256.Text = Properties.Resources.GM256;
            this.FilterGM512.Text = Properties.Resources.GM512;
            this.FilterGM1024.Text = Properties.Resources.GM1024;
            this.FilterGM2048.Text = Properties.Resources.GM2048;
            this.FilterGM131072.Text = Properties.Resources.GM131072;
            this.FilterGM524288.Text = Properties.Resources.GM524288;
            this.FilterGM4194304.Text = Properties.Resources.GM4194304;
            this.FilterGM8388608.Text = Properties.Resources.GM8388608;
            currentFilter = new SearchFilter();
            serverBrowserView.DoubleBuffered(true);
            serverList = new List<ServerInfo>();
            GetServerList();
        }

        private void ToolStripDropDownButton1_Click(object sender, EventArgs e)
        {
            Cancel = true;
        }

        private void serverBrowserView_Scroll(object sender, ScrollEventArgs e)
        {
            if (serverBrowserView.DisplayedRowCount(false) + serverBrowserView.FirstDisplayedScrollingRowIndex >= serverBrowserView.RowCount)
                GetServerList();
        }

        private void serverBrowserView_SelectionChanged(object sender, EventArgs e)
        {
            if (serverBrowserView.SelectedRows.Count == 0)
                return;
            if (serverBrowserView.SelectedRows[0].Index == -1)
                return;
            int serverIndex = (int)serverBrowserView.SelectedRows[0].Cells["Index"].Value;
            UpdateServerInfo(serverIndex);
            UpdatePing(serverIndex);
        }

        private void serverBrowserView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == -1)
                return;
            if (!Busy)
                RunGame(serverList[(int)serverBrowserView.Rows[e.RowIndex].Cells["Index"].Value].guid);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            currentFilter = new SearchFilter();
            if (FilterGM1.Checked)
                currentFilter.gamemodes.Add(1);
            if (FilterGM2.Checked)
                currentFilter.gamemodes.Add(2);
            if (FilterGM4.Checked)
                currentFilter.gamemodes.Add(4);
            if (FilterGM8.Checked)
                currentFilter.gamemodes.Add(8);
            if (FilterGM32.Checked)
                currentFilter.gamemodes.Add(32);
            if (FilterGM64.Checked)
                currentFilter.gamemodes.Add(64);
            if (FilterGM128.Checked)
                currentFilter.gamemodes.Add(128);
            if (FilterGM256.Checked)
                currentFilter.gamemodes.Add(256);
            if (FilterGM512.Checked)
                currentFilter.gamemodes.Add(512);
            if (FilterGM1024.Checked)
                currentFilter.gamemodes.Add(1024);
            if (FilterGM2048.Checked)
                currentFilter.gamemodes.Add(2048);
            if (FilterGM131072.Checked)
                currentFilter.gamemodes.Add(131072);
            if (FilterGM524288.Checked)
                currentFilter.gamemodes.Add(524288);
            if (FilterGM4194304.Checked)
                currentFilter.gamemodes.Add(4194304);
            if (FilterGM8388608.Checked)
                currentFilter.gamemodes.Add(8388608);
            if (FilterGameBF3.Checked)
                currentFilter.gameexpansions.Add(0);
            if (FilterGameB2K.Checked)
                currentFilter.gameexpansions.Add(512);
            if (FilterGameCQ.Checked)
                currentFilter.gameexpansions.Add(2048);
            if (FilterGameAK.Checked)
                currentFilter.gameexpansions.Add(4096);
            if (FilterGameAM.Checked)
                currentFilter.gameexpansions.Add(8192);
            if (FilterGameEG.Checked)
                currentFilter.gameexpansions.Add(16384);
            serverList.Clear();
            serverBrowserView.Rows.Clear();
            GetServerList();
        }
    }

    public static class ExtensionMethods
    {
        public static void DoubleBuffered(this DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }
    }
}