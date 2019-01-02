using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BattleLauncher
{
    public static class Utils
    {
        static public Task<string> DoHttpRequestAsync(string url)
        {
            return Task.Run(async () =>
            {
                WebClient wc = new WebClient();
                wc.Headers.Add(HttpRequestHeader.Cookie, "beaker.session.id=" + Globals.SessionToken);
                wc.Headers.Add("X-Requested-With", "XMLHttpRequest");
                return await wc.DownloadStringTaskAsync(url);
            });
        }

        static public async Task<string> DoLocalHttpRequestAsync(string url)
        {
            return await Task.Run(async () =>
            {

                WebClient wc = new WebClient();
                wc.Headers.Add(HttpRequestHeader.Accept, "*/*");
                wc.Headers.Add("Origin", URL.Battlelog);
                return await wc.DownloadStringTaskAsync(url);
            });
        }

        static public async Task<T> DeserializeResponseAsync<T>(string response) where T: Battlelog.BattlelogResponse
        {
            return await Task.Run(() =>
            {
                T resp;
                resp = JsonConvert.DeserializeObject<T>(response);
                if (resp.type == "error")
                    throw new Exception("Battlelog faulty response");
                return resp;
            });
        }

        static public async Task<T> DeserializeResponse2Async<T>(string response)
        {
            return await Task.Run(() =>
            {
                T resp;
                resp = JsonConvert.DeserializeObject<T>(response);
                return resp;
            }); 
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
        }

        public class ReservationInfo
        {
            public string joinState;
        }

        public class PersonaData
        {
            public string personaId;
            public string personaName;
            public string allowRunGame;
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