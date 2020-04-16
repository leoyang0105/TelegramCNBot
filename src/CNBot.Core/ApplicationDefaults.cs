using System;
using System.Collections.Generic;
using System.Text;

namespace CNBot.Core
{
    public static class ApplicationDefaults
    {
        public static int ConcurrentTaskCount => Environment.ProcessorCount * 2;
        public static string[] Commands => new[] { "/help", "/list", "/join", "/search", "/mylist", "/update", "/remove", "/reset" };
        public static string CNBotUserName => "@CN_TG_BOT";
        public static string DefaultContentType => "application/json";
        public static string TelegramApiEndpoint => "https://api.telegram.org";
        public static int CacheTime => 60 * 60;
        public static string DataProtectionKey => "CNBot.DataProtectionKeys";
    }
}
