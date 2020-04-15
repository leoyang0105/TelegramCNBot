using Newtonsoft.Json;

namespace CNBot.Core.Dtos
{
    public class TGUserDTO
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("is_bot")]
        public bool IsBot { get; set; }
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("language_code")]
        public string LanguageCode { get; set; }
        [JsonProperty("can_join_groups")]
        public bool CanJoinGroups { get; set; }
        [JsonProperty("can_read_all_group_messages")]
        public bool Photo { get; set; }
        [JsonProperty("supports_inline_queries")]
        public bool SupportsInlineQueries { get; set; }
    }
}
