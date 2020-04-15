using CNBot.Core.Entities.Chats;
using CNBot.Core.Entities.Messages;
using CNBot.Core.Events;

namespace CNBot.API.Application.Events
{
    public class TelegramMessageEvent : IntegrationEvent
    {
        public long UserId { get; set; }
        public long TGChatId { get; set; }
        public ChatType ChatType { get; set; }
        public Message Message { get; set; }
        public TelegramMessageEvent(Message message, long userId, long tgChatId, ChatType chatType)
        {
            Message = message;
            UserId = userId;
            TGChatId = tgChatId;
            ChatType = chatType;
        }
    }
}
