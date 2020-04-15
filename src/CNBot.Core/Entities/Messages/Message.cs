using CNBot.Core.Entities.Chats;
using System;
using System.Collections.Generic;

namespace CNBot.Core.Entities.Messages
{
    public class Message : BaseEntity
    {
        private ICollection<MessageEntity> _messageEntities;
        public string Text { get; set; }
        public long TGMessageId { get; set; }
        public long TGChatId { get; set; }
        public long FromTGUserId { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public ICollection<MessageEntity> MessageEntities
        {
            get => _messageEntities ?? (_messageEntities = new List<MessageEntity>());
            private set => _messageEntities = value;
        }
    }
}
