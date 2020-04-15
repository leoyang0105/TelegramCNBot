using CNBot.Core.Entities.Messages;
using Newtonsoft.Json;
using System;

namespace CNBot.Core.Dtos
{
    public class TGMessageEntityDTO
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("offset")]
        public int Offset { get; set; }
        [JsonProperty("length")]
        public int Length { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("user")]
        public TGUserDTO User { get; set; }
        [JsonProperty("language")]
        public string Language { get; set; }
        public MessageEntityType GetEntityType()
        {
            MessageEntityType entityType;
            Enum.TryParse(this.Type, out entityType);
            return entityType;
        }
    }
}
