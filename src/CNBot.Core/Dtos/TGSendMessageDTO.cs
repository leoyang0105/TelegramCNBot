using Newtonsoft.Json;
using System.Collections.Generic;

namespace CNBot.Core.Dtos
{
    public class TGSendMessageDTO
    {
        [JsonProperty("chat_id")]
        public long ChatId { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("parse_mode")]
        public string ParseMode { get; set; }
        [JsonProperty("reply_to_message_id")]
        public long ReplyToMessageId { get; set; }
        [JsonProperty("reply_markup")]
        public object ReplyMarkup { get; set; }

        [JsonProperty("disable_web_page_preview")]
        public bool DisableWebPagePreview { get; set; }
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
    public class TGInlineKeyboardMarkup
    {
        [JsonProperty("inline_keyboard")]
        public List<InlineKeyboardButton>[] InlineKeyboard { get; set; }
        public class InlineKeyboardButton
        {
            [JsonProperty("text")]
            public string Text { get; set; }
            [JsonProperty("url")]
            public string Url { get; set; }
            [JsonProperty("login_url")]
            public string LoginUrl { get; set; }
            [JsonProperty("callback_data")]
            public string CallbackData { get; set; }
            [JsonProperty("switch_inline_query")]
            public string WwitchInlineQuery { get; set; }
            [JsonProperty("switch_inline_query_current_chat")]
            public string WwitchInlineQueryCurrentChat { get; set; }
            [JsonProperty("pay")]
            public bool Pay { get; set; } 
        }
    }
    public enum MessageParseModelType
    {
        Markdown,
        Html
    }
}
