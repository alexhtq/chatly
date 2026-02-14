namespace Chatly.Shared.Constants;

public static class Routes
{
    public static class Api
    {
        public static class Messages
        {
            public const string GetAll = "api/messages";
            public const string GetById = "api/messages/{id:Guid}";
            public const string Create = "api/messages";
        }
    }

    public static class Pages
    {
        public const string Home = "/";

        public static class Messages
        {
            public const string Index = "/messages";
        }      
    }
}