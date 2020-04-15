using Newtonsoft.Json;

namespace CNBot.Core.Dtos
{
    public class TGMessageDTO
    {
        [JsonProperty("message_id")]
        public long MessageId { get; set; }
        /// <summary>
        /// From user
        /// </summary>
        [JsonProperty("from")]
        public TGUserDTO From { get; set; }

        [JsonProperty("chat")]
        public TGChatDTO Chat { get; set; }

        [JsonProperty("date")]
        public int Date { get; set; }

        #region Forward info

        [JsonProperty("forward_from_message_id")]
        public long ForwardFromMessageId { get; set; }
        [JsonProperty("forward_signature")]
        public string ForwardSignature { get; set; }
        [JsonProperty("forward_sender_name")]
        public string ForwardSenderName { get; set; }
        [JsonProperty("forward_date")]
        public int ForwardDate { get; set; }
        /// <summary>
        /// Forward from user
        /// </summary>
        [JsonProperty("forward_from")]
        public TGUserDTO ForwardFrom { get; set; }
        [JsonProperty("forward_from_chat")]
        public TGChatDTO ForwardFromChat { get; set; }
        #endregion

        [JsonProperty("reply_to_message")]
        public object ReplyToMessage { get; set; }

        [JsonProperty("edit_date")]
        public int EditDate { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("entities")]
        public TGMessageEntityDTO[] Entities { get; set; }

        [JsonProperty("reply_markup")]
        public object ReplyMarkup { get; set; }
    }
}
