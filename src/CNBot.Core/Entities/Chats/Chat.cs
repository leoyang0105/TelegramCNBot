using System;

namespace CNBot.Core.Entities.Chats
{
    public class Chat : BaseEntity
    {
        public int TGChatId { get; set; }
        public string Title { get; set; }
        public string UserName { get; set; }
        public string Description { get; set; }
        public ChatType ChatType { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public ChatPhoto ChatPhoto { get; set; }
    }
}
