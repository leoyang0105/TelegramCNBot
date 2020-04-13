namespace CNBot.Core.Configurations
{
    public class TelegramUrlsConfig
    {
        private const string API_ENDPOINT = "https://api.telegram.org";
        public string ApiToken { get; set; }
        public static class Chat
        {
            public static string Get(string token, string chatId) => $"{API_ENDPOINT}/bot{token}/getChat?chat_id={chatId}";
            public static string GetAdministrators(string token, string chatId) => $"{API_ENDPOINT}/bot{token}/getChatAdministrators?chat_id={chatId}";
            public static string GetMembersCount(string token, string chatId) => $"{API_ENDPOINT}/bot{token}/getChatMembersCount?chat_id={chatId}";
            public static string GetMember(string token, string chatId, string userId) => $"{API_ENDPOINT}/bot{token}/getChatMember?chat_id={chatId}&user_id={userId}";
        }
        public static class Message
        {
            public static string Send(string token) => $"{API_ENDPOINT}/bot{token}/sendMessage";
            public static string SendPhoto(string token) => $"{API_ENDPOINT}/bot{token}/sendPhoto";
            public static string SendAudio(string token) => $"{API_ENDPOINT}/bot{token}/sendAudio";
            public static string SendVideo(string token) => $"{API_ENDPOINT}/bot{token}/sendVideo";
            public static string SendDocument(string token) => $"{API_ENDPOINT}/bot{token}/sendDocument";
            public static string SendAnimation(string token) => $"{API_ENDPOINT}/bot{token}/sendAnimation";
            public static string SendVoice(string token) => $"{API_ENDPOINT}/bot{token}/sendVoice";
            public static string SendMediaGroup(string token) => $"{API_ENDPOINT}/bot{token}/sendMediaGroup";
        }
    }
}
