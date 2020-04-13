using System;
using System.Collections.Generic;
using System.Text;

namespace CNBot.Mvc
{
    internal static class ApplicationDefaults
    {
        public static string DbConnectionStringKey => "DbConnection";
        public static string DbProviderTypeKey => "DbProviderType";

        public static string RedisConnectionStringKey => "RedisConnection";
        public static string CacheProviderTypeKey => "CacheProviderType";
        public static string EventBusProviderTypeKey => "EventBusProviderType";
    }
}
