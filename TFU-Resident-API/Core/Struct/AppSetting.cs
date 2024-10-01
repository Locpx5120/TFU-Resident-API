namespace Core.Struct
{
    public struct AppSetting
    {
        public struct ConnectionStrings
        {
            public const string DbConnection = "ConnectionStrings:DefaultConnection";
            public const string BaseSchema = "ConnectionStrings:BaseSchema";
        }

        public struct AppSettings
        {
            public const string SecretKey = "AppSettings:SecretKey";
        }
    }
}
