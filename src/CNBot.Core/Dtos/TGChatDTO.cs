using CNBot.Core.Entities.Chats;
using Newtonsoft.Json;

namespace CNBot.Core.Dtos
{
    public class TGChatDTO
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("photo")]
        public object Photo { get; set; }
        [JsonProperty("invite_link")]
        public string InviteLink { get; set; }
        [JsonProperty("permissions")]
        public object Permissions { get; set; }
        public ChatType GetChatType()
        {
            ChatType chatType = ChatType.None;
            switch (this.Type)
            {
                case "private":
                    chatType = ChatType.Private;
                    break;
                case "channel":
                    chatType = ChatType.Channel;
                    break;
                case "group":
                    chatType = ChatType.Group;
                    break;
                case "supergroup":
                    chatType = ChatType.SuperGroup;
                    break;
            }
            return chatType;
        }
    }
}
