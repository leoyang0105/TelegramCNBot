using System;
using System.Collections.Generic;
using System.Text;

namespace CNBot.Core
{
   public static class ApplicationDefaults
    {
        public static string DefaultContentType => "application/json";
        public static string TelegramApiEndpoint => "https://api.telegram.org";
        public static int CacheTime => 60 * 60;
        public static string DataProtectionKey => "CNBot.DataProtectionKeys";
    }
}
