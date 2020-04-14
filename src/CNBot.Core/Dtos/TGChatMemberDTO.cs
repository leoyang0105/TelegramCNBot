using CNBot.Core.Entities.Chats;
using Newtonsoft.Json;

namespace CNBot.Core.Dtos
{
    public class TGChatMemberDTO
    {
        [JsonProperty("user")]
        public TGUserDTO User { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("custom_title")]
        public string CustomTitle { get; set; }
        [JsonProperty("until_date")]
        public string UntilDate { get; set; }

        public ChatMemberStatusType GetStatus()
        {
            ChatMemberStatusType type = ChatMemberStatusType.None;
            switch (Status)
            {
                case "creator":
                    type = ChatMemberStatusType.Creator;
                    break;
                case "administrator":
                    type = ChatMemberStatusType.Administrator;
                    break;
                case "member":
                    type = ChatMemberStatusType.Member;
                    break;
                case "restricted":
                    type = ChatMemberStatusType.Restricted;
                    break;
                case "left":
                    type = ChatMemberStatusType.Left;
                    break;
                case "kicked":
                    type = ChatMemberStatusType.Kicked;
                    break;
            }
            return type;
        }
        public ChatMemberPermissionType GetPermission()
        {
            var permissions = ChatMemberPermissionType.none;
            //TODO 稍后实现
            return permissions;
        }
    }
}
