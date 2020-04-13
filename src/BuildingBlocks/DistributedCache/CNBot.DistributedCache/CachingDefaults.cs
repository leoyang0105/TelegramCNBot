namespace CNBot.DistributedCache
{
    public static class CachingDefaults
    {
        public static int CacheTime => 60 * 60;
        public static string DataProtectionKey => "CNBot.DataProtectionKeys";
    }
}
