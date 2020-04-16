using Newtonsoft.Json;

namespace CNBot.Core.Dtos
{
    public class TGUpdateDTO
    {
        [JsonProperty("update_id")]
        public long UpdateId { get; set; }
        [JsonProperty("message")]
        public TGMessageDTO Message { get; set; }
        [JsonProperty("callback_query")]
        public TGCallbackQueryDTO CallbackQuery { get; set; }
    }
}
