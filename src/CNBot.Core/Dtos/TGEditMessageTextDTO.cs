using Newtonsoft.Json;

namespace CNBot.Core.Dtos
{
    public class TGEditMessageTextDTO
    {
        [JsonProperty("chat_id")]
        public long ChatId { get; set; }
        [JsonProperty("message_id")]
        public long MessageId { get; set; }
        [JsonProperty("inline_message_id")]
        public string InlineMessageId { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("parse_mode")]
        public string ParseMode { get; set; }
        [JsonProperty("reply_markup")]
        public object ReplyMarkup { get; set; }
        [JsonProperty("disable_web_page_preview")]
        public bool DisableWebPagePreview { get; set; }
    }
}
