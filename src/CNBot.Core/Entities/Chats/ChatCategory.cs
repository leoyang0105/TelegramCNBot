using System;
using System.Collections.Generic;
using System.Text;

namespace CNBot.Core.Entities.Chats
{
    public class ChatCategory : BaseEntity
    {
        public Chat Chat { get; set; }
        public long ChatId { get; set; }
        public Category Category { get; set; }
        public long CategoryId { get; set; }
    }
}
