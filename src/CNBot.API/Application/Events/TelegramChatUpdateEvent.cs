using CNBot.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CNBot.API.Application.Events
{
    public class TelegramChatUpdateEvent : IntegrationEvent
    {
        public long TgUserId { get; set; }
        public string ChatUserName { get; set; }
        public TelegramChatUpdateEvent(long tgUserId, string chatUserName)
        {
            TgUserId = tgUserId;
            ChatUserName = chatUserName;
        }
    }
}
