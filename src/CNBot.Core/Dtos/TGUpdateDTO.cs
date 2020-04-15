using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CNBot.Core.Dtos
{
   public class TGUpdateDTO
    {
        [JsonProperty("update_id")]
        public long UpdateId { get; set; }
        [JsonProperty("message")]
        public TGMessageDTO Message { get; set; }
    }
}
