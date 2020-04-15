using System;

namespace CNBot.Core.Entities.Chats
{
    public class ChatMember : BaseEntity
    {
        public Chat Chat { get; set; }
        public long ChatId { get; set; }
        public long TGUserId { get; set; }
        public DateTime Created { get; set; }
        public ChatMemberStatusType Status { get; set; }
        public ChatMemberPermissionType Permissions { get; set; }
    }
}
