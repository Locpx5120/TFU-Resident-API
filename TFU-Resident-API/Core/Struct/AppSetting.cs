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
            public const string SmtpUsername = "AppSettings:SmtpUsername";
            public const string SmtpPassword = "AppSettings:SmtpPassword";
            public const string SmtpServer = "AppSettings:SmtpServer";
            public const string SmtpPort = "AppSettings:SmtpPort";
        }
    }
}
