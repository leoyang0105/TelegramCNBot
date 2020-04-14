using Newtonsoft.Json;
using System.Collections.Generic;

namespace CNBot.Core.Dtos
{
    public class TGSendMessageDTO
    {
        [JsonProperty("chat_id")]
        public int ChatId { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("parse_mode")]
        public string ParseMode { get; set; }
        [JsonProperty("reply_to_message_id")]
        public int ReplyToMessageId { get; set; }
        [JsonProperty("reply_markup")]
        public object ReplyMarkup { get; set; }
    }
    public class TGReplyKeyboardMarkup
    {

        [JsonProperty("keyboard")]
        public List<KeyboardButton>[] Keyboard { get; set; }
        [JsonProperty("resize_keyboard")]
        public bool ResizeKeyboard { get; set; }
        [JsonProperty("one_time_keyboard")]
        public bool OneTimeKeyboard { get; set; }
        [JsonProperty("selective")]
        public bool Selective { get; set; }
        public class KeyboardButton
        {
            [JsonProperty("text")]
            public string Text { get; set; }
            [JsonProperty("request_contact")]
            public bool RequestContact { get; set; }
            [JsonProperty("request_location")]
            public bool RequestLocation { get; set; }
            [JsonProperty("request_poll")]
            public object RequestPoll { get; set; }
        }
    }
    public enum MessageParseModelType
    {
        Markdown,
        Html
    }
}
