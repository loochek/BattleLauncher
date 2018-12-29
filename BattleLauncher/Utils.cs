using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace BattleLauncher
{
    public static class Utils
    {
        static public string DoHttpRequest(string url)
        {
            HttpWebRequest req = WebRequest.CreateHttp(url);
            req.CookieContainer = new CookieContainer();
            req.CookieContainer.Add(new Uri(URL.Battlelog), new Cookie("beaker.session.id", Globals.SessionToken));
            WebResponse resp;
            resp = req.GetResponse();
            return new StreamReader(resp.GetResponseStream()).ReadToEnd();
        }

        static public string DoLocalHttpRequest(string url)
        {
            HttpWebRequest req = WebRequest.CreateHttp(url);
            req.Accept = "*/*";
            req.Headers["Origin"] = URL.Battlelog;
            WebResponse resp;
            resp = req.GetResponse();
            return new StreamReader(resp.GetResponseStream()).ReadToEnd();
        }

        static public string DoHttpXHRRequest(string url)
        {
            HttpWebRequest req = WebRequest.CreateHttp(url);
            req.CookieContainer = new CookieContainer();
            req.CookieContainer.Add(new Uri(URL.Battlelog), new Cookie("beaker.session.id", Globals.SessionToken));
            req.Headers["X-Requested-With"] = "XMLHttpRequest";
            WebResponse resp;
            try
            {
                resp = req.GetResponse();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return new StreamReader(resp.GetResponseStream()).ReadToEnd();
        }

        static public T1 DeserializeResponse<T1>(string response) where T1: Battlelog.BattlelogResponse
        {
            T1 resp;
            resp = JsonConvert.DeserializeObject<T1>(response);
            if (resp.type == "error")
                throw new Exception("Battlelog faulty response");
            return resp;
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
    }
}