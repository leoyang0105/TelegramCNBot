using CNBot.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CNBot.API.Application.Events
{
    public class TelegramChatRemoveEvent: IntegrationEvent
    {
        public long TgUserId { get; set; }
        public string ChatUserName { get; set; }
        public TelegramChatRemoveEvent(long tgUserId, string chatUserName)
        {
            TgUserId = tgUserId;
            ChatUserName = chatUserName;
        }
    }
}
