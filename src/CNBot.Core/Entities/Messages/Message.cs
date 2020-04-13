using CNBot.Core.Entities.Chats;
using System;
using System.Collections.Generic;

namespace CNBot.Core.Entities.Messages
{
    public class Message : BaseEntity
    {
        private ICollection<MessageEntity> _messageEntities;
        public string Text { get; set; }
        public ChatType ChatType { get; set; }
        public int TGMessageId { get; set; }
        public int TGChatId { get; set; }
        public int FromTGUserId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public int ForwardTGChatId { get; set; }
        public int ForwardTGMessageId { get; set; }
        public DateTime? ForwardCreated { get; set; }
        public ICollection<MessageEntity> MessageEntities
        {
            get => _messageEntities ?? (_messageEntities = new List<MessageEntity>());
            private set => _messageEntities = value;
        }
    }
}
