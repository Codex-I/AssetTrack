namespace AssetTrackApi.Configuration
{
    public class Settings
    {
        public JWTOptions JWT { get; init; } = new();
        public ConnectionStringOptions ConnStrings { get; init; } = new();

        public class ConnectionStringOptions
        {
            public string SqlDb { get; init; } = string.Empty;
        }

        public class JWTOptions
        {
            public string Key { get; init; } = string.Empty;
            public string Issuer { get; init; } = string.Empty;
            public int ExpiresIn { get; init; } = 3600;
        }

        public static class Headers
        {
            public const string PAGE_SIZE = "X-Page-Size";
            public const string PAGE_NUMBER = "X-Page-Number";
            public const string TOTAL_PAGES = "X-Total-Pages";
            public const string TOTAL_ITEMS = "X-Total-Count";
        }
    }
}
