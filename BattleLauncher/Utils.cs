using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace BattleLauncher
{
    public static class Utils
    {
        static public string doHttpRequest(string url, CookieContainer cookies)
        {
            HttpWebRequest req = WebRequest.CreateHttp(url);
            req.CookieContainer = cookies;
            WebResponse resp;
            resp = req.GetResponse();
            return new StreamReader(resp.GetResponseStream()).ReadToEnd();
        }

        static public string doLocalHttpRequest(string url)
        {
            HttpWebRequest req = WebRequest.CreateHttp(url);
            req.Accept = "*/*";
            req.Headers["Origin"] = "http://battlelog.battlefield.com";
            WebResponse resp;
            resp = req.GetResponse();
            return new StreamReader(resp.GetResponseStream()).ReadToEnd();
        }

        static public string doHttpXmlRequest(string url, CookieContainer cookies)
        {
            HttpWebRequest req = WebRequest.CreateHttp(url);
            req.CookieContainer = cookies;
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

        static public T1 deserializeResponse<T1>(string response) where T1: Battlelog.BattlelogXHRResponse
        {
            T1 resp;
            try
            {  
                resp = JsonConvert.DeserializeObject<T1>(response);
            }
            catch (JsonException e)
            {
                throw new Exception(e.Message);
            }
            if (resp.type == "error")
                throw new Exception("Battlelog faulty response");
            return resp;
        }
    }

    namespace Battlelog
    {
        public class BattlelogXHRResponse
        {
            public string type;
            public string message;
        }

        public class ServerInfoResponse : BattlelogXHRResponse
        {
            public ServerInfo data;
        }

        public class ServerListResponse : BattlelogXHRResponse
        {
            public List<ServerInfo> data;
        }

        public class SlotReservationResponse : BattlelogXHRResponse
        {
            public ReservationInfo data;
        }

        public class QueueStatusResponse : BattlelogXHRResponse
        {
            public QueueStatusInfo data;
        }

        public class AuthDataResponse : BattlelogXHRResponse
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

        public class PlayablePersonaResponse : BattlelogXHRResponse
        {
            public PersonaData data;
        }
    }
}