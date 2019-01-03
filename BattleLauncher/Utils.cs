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
                    return "Grand Bazaar";
                case "MP_003":
                    return "Tehran Highway";
                case "MP_007":
                    return "Caspian Border";
                case "MP_011":
                    return "Seine Crossing";
                case "MP_012":
                    return "Operation Firestorm";
                case "MP_013":
                    return "Damavand Peak";
                case "MP_017":
                    return "Noshahr Canals";
                case "MP_018":
                    return "Kharg Island";
                case "MP_Subway":
                    return "Operation Metro";
                case "XP1_001":
                    return "Strike at Karkand";
                case "XP1_002":
                    return "Gulf of Oman";
                case "XP1_003":
                    return "Sharqi Peninsula";
                case "XP1_004":
                    return "Wake Island";
                case "XP2_Factory":
                    return "Scrapmetal";
                case "XP2_Office":
                    return "Operation 925";
                case "XP2_Palace":
                    return "Donya Fortress";
                case "XP2_Skybar":
                    return "Ziba Tower";
                case "XP3_Alborz":
                    return "Alborz Mountains";
                case "XP3_Desert":
                    return "Bandar Desert";
                case "XP3_Shield":
                    return "Armored Shield";
                case "XP3_Valley":
                    return "Death Valley";
                case "XP4_FD":
                    return "Markaz Monolith";
                case "XP4_Parl":
                    return "Azadi Palace";
                case "XP4_Quake":
                    return "Epicenter";
                case "XP4_Rubble":
                    return "Talah Market";
                case "XP5_001":
                    return "Operation Riverside";
                case "XP5_002":
                    return "Nebandan Flats";
                case "XP5_003":
                    return "Kiasar Railroad";
                case "XP5_004":
                    return "Sabalan Pipeline";
                default:
                    return "Unknown map ID";
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
            public bool HasPassword;
            public string country;
            public string name;
            public SlotsInfo slots;
            public int ping;
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