using System;
using System.Collections.Generic;

namespace CNBot.Core.Entities.Chats
{
    public class Chat : BaseEntity
    {
        private ICollection<ChatCategory> _chatCategories;
        public long TGChatId { get; set; }
        public string Title { get; set; }
        public string UserName { get; set; }
        public string Description { get; set; }
        public ChatType ChatType { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public int MembersCount { get; set; }
        /// <summary>
        /// Telegram user Id
        /// </summary>
        public long CreatorId { get; set; }
        public string InviteLink { get; set; }
        public ICollection<ChatCategory> ChatCategories
        {
            get => _chatCategories ?? (_chatCategories = new List<ChatCategory>());
            private set => _chatCategories = value;
        }
    }
}
