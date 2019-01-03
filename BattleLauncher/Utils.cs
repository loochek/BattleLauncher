using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace BattleLauncher
{
    public static class Utils
    {
        static public Task<string> DoHttpRequestAsync(string url)
        {
            return Task.Run(() =>
            {
                WebClient wc = new WebClient();
                wc.Headers.Add(HttpRequestHeader.Cookie, "beaker.session.id=" + Globals.SessionToken);
                wc.Headers.Add("X-Requested-With", "XMLHttpRequest");
                return wc.DownloadString(url);
            });
        }

        static public Task<string> DoLocalHttpRequestAsync(string url)
        {
            return Task.Run(() =>
            {
                WebClient wc = new WebClient();
                wc.Headers.Add(HttpRequestHeader.Accept, "*/*");
                wc.Headers.Add("Origin", URL.Battlelog);
                return wc.DownloadString(url);
            });
        }

        static public Task<T> DeserializeResponseAsync<T>(string response) where T: Battlelog.BattlelogResponse
        {
            return Task.Run(() =>
            {
                T resp;
                resp = JsonConvert.DeserializeObject<T>(response);
                if (resp.type == "error")
                    throw new Exception("Battlelog faulty response");
                return resp;
            });
        }

        static public Task<T> DeserializeResponse2Async<T>(string response)
        {
            return Task.Run(() =>
            {
                T resp;
                resp = JsonConvert.DeserializeObject<T>(response);
                return resp;
            }); 
        }

        public static string GetMapName(string id)
        {
            switch (id)
            {
                case "MP_001":
                    return Properties.Resources.MP_001;
                case "MP_003":
                    return Properties.Resources.MP_003;
                case "MP_007":
                    return Properties.Resources.MP_007;
                case "MP_011":
                    return Properties.Resources.MP_011;
                case "MP_012":
                    return Properties.Resources.MP_012;
                case "MP_013":
                    return Properties.Resources.MP_013;
                case "MP_017":
                    return Properties.Resources.MP_017;
                case "MP_018":
                    return Properties.Resources.MP_018;
                case "MP_Subway":
                    return Properties.Resources.MP_Subway;
                case "XP1_001":
                    return Properties.Resources.XP1_001;
                case "XP1_002":
                    return Properties.Resources.XP1_002;
                case "XP1_003":
                    return Properties.Resources.XP1_003;
                case "XP1_004":
                    return Properties.Resources.XP1_004;
                case "XP2_Factory":
                    return Properties.Resources.XP2_Factory;
                case "XP2_Office":
                    return Properties.Resources.XP2_Office;
                case "XP2_Palace":
                    return Properties.Resources.XP2_Palace;
                case "XP2_Skybar":
                    return Properties.Resources.XP2_Skybar;
                case "XP3_Alborz":
                    return Properties.Resources.XP3_Alborz;
                case "XP3_Desert":
                    return Properties.Resources.XP3_Desert;
                case "XP3_Shield":
                    return Properties.Resources.XP3_Shield;
                case "XP3_Valley":
                    return Properties.Resources.XP3_Valley;
                case "XP4_FD":
                    return Properties.Resources.XP4_FD;
                case "XP4_Parl":
                    return Properties.Resources.XP4_Parl;
                case "XP4_Quake":
                    return Properties.Resources.XP4_Quake;
                case "XP4_Rubble":
                    return Properties.Resources.XP4_Rubble;
                case "XP5_001":
                    return Properties.Resources.XP5_001;
                case "XP5_002":
                    return Properties.Resources.XP5_002;
                case "XP5_003":
                    return Properties.Resources.XP5_003;
                case "XP5_004":
                    return Properties.Resources.XP5_004;
                default:
                    return "Unknown map ID";
            }
        }

        public static string GetGameModeName(int id)
        {
            switch (id)
            {
                case 1:
                    return Properties.Resources.GM1;
                case 2:
                    return Properties.Resources.GM2;
                case 4:
                    return Properties.Resources.GM4;
                case 8:
                    return Properties.Resources.GM8;
                case 32:
                    return Properties.Resources.GM32;
                case 64:
                    return Properties.Resources.GM64;
                case 128:
                    return Properties.Resources.GM128;
                case 256:
                    return Properties.Resources.GM256;
                case 512:
                    return Properties.Resources.GM512;
                case 1024:
                    return Properties.Resources.GM1024;
                case 2048:
                    return Properties.Resources.GM2048;
                case 131072:
                    return Properties.Resources.GM131072;
                case 524288:
                    return Properties.Resources.GM524288;
                case 4194304:
                    return Properties.Resources.GM4194304;
                case 8388608:
                    return Properties.Resources.GM8388608;
                default:
                    return "Unknown game mode ID";
            }
        }
    }

    namespace Battlelog
    {

        public class BattlelogResponse
        {
            public string type;
            public string message;
        }

        public class ServerInfoResponse : BattlelogResponse
        {
            public ServerInfo data;
        }

        public class ServerListResponse : BattlelogResponse
        {
            public List<ServerInfo> data;
        }

        public class SlotReservationResponse : BattlelogResponse
        {
            public ReservationInfo data;
        }

        public class QueueStatusResponse : BattlelogResponse
        {
            public QueueStatusInfo data;
        }

        public class AuthDataResponse : BattlelogResponse
        {
            public AuthData data;
        }

        public class QueueStatusInfo
        {
            public int queuePosition;
        }

        public class ServerInfo
        {
            public string map;
            public string gameId;
            public string ip;
            public bool ranked;
            public string guid;
            public int port;
            public int region;
            public bool fairfight;
            public int tickRate;
            public bool hasPassword;
            public string country;
            public string name;
            public SlotsInfo slots;
            public int ping;
            public int mapMode;
        }

        public class ReservationInfo
        {
            public string joinState;
        }

        public class PersonaData
        {
            public string personaId;
            public string personaName;
            public bool allowRunGame;
            public bool banned;
        }

        public class AuthData
        {
            public string encryptedToken;
            public string authCode;
        }

        public class PlayablePersonaResponse : BattlelogResponse
        {
            public PersonaData data;
        }

        public class SlotsInfo
        {
            [JsonProperty("1")]
            public PlayersCountInfo queued;
            [JsonProperty("2")]
            public PlayersCountInfo normal;

            public class PlayersCountInfo
            {
                public int current;
                public int max;
            }
        }

        public class NumPlayersResponse
        {
            public int mapMode;
            public SlotsInfo slots;
            public string map;
        }
    }
}