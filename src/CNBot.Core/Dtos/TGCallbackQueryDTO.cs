using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CNBot.Core.Dtos
{
    public class TGCallbackQueryDTO
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("from")]
        public TGUserDTO From { get; set; }

        [JsonProperty("message")]
        public TGMessageDTO Message { get; set; }
        [JsonProperty("inline_message_id")]
        public string InlineMessageId { get; set; }
        [JsonProperty("chat_instance")]
        public string ChatInstance { get; set; }
        [JsonProperty("data")]
        public string Data { get; set; }
    }
}
