using System.Text.RegularExpressions;

namespace Chatly.Shared.Constants;

public static partial class Routes
{
    [GeneratedRegex("{.*?}")]
    private static partial Regex StringFormatArgsRegex();

    public static string Format(this string template, params object[] args)
    {
        var index = 0;
        var formattedTemplate = StringFormatArgsRegex().Replace(template, _ => $"{{{index++}}}");
        
        return string.Format(formattedTemplate, args);
    }

    public static class Api
    {
        public static class Messages
        {
            public const string GetAll = "api/messages";
            public const string GetById = "api/messages/{id:Guid}";
            public const string Create = "api/messages";
            public const string Update = "api/messages/{id:Guid}";
            public const string Delete = "api/messages/{id:Guid}";
        }
    }

    public static class Pages
    {
        public const string Home = "/";

        public static class Messages
        {
            public const string Index = "/messages";
            public const string Create = "/messages/create";
            public const string Update = "/messages/update/{id:guid}";
        }      
    }
}