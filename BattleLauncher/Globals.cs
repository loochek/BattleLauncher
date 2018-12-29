namespace BattleLauncher
{
    public static class Globals
    {
        public static string SessionToken { get; set; }

        private static string bf3GameDir;

        public static string BF3GameDir
        {
            get
            {
                return bf3GameDir;
            }

            set
            {
                bf3GameDir = value;
                BF3ExePath = bf3GameDir + "BF3WebHelper.exe";
            }
        }

        public static string BF3ExePath { get; set; }

        private static string bf4GameDir;

        public static string BF4GameDir
        {
            get
            {
                return bf4GameDir;
            }

            set
            {
                bf4GameDir = value;
                BF4ExePath = bf4GameDir + "BF4WebHelper.exe";
            }
        }

        public static string BF4ExePath { get; set; }

        public const string GameArgs = @"-webMode MP -Origin_NoAppFocus --activate-webhelper -AuthCode {0} -requestState State_ClaimReservation -requestStateParams ""<data putinsquad =\""true\"" gameid =\""{1}\"" role =\""soldier\"" personaref =\""{2}\"" levelmode =\""mp\"" logintoken =\""{3}\""></data>"" -Online.BlazeLogLevel 2 -Online.DirtysockLogLevel 2";
    }

    public static class URL
    {
        public const string Battlelog = "http://battlelog.battlefield.com";
        public const string BF3ServerInfoShort = "http://battlelog.battlefield.com/bf3/servers/show/pc/{0}/?json=1&join=true";
        public const string BF3PlayablePersona = "https://battlelog.battlefield.com/bf3/launcher/playablepersona/";
        public const string BF3ReserveSlot = "http://battlelog.battlefield.com/bf3/launcher/reserveslotbygameid/1/{0}/{1}/1/{2}/0";
        public const string BF3AuthData = "https://battlelog.battlefield.com/bf3/launcher/token/1/{0}";
        public const string BF3LeaveServer = "http://battlelog.battlefield.com/bf3/launcher/mpleavegameserver/1/{0}/{1}";
        public const string BF3QueueStatus = "http://battlelog.battlefield.com/bf3/launcher/fetchQueueStatus/1/{0}/{1}";
        public const string WebHelper = "http://127.0.0.1:4219";
        public const string KillGame = "http://127.0.0.1:4219/killgame";
    }

}
