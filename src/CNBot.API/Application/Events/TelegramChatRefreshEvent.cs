using CNBot.Core.Events;

namespace CNBot.API.Application.Events
{
    public class TelegramChatRefreshEvent : IntegrationEvent
    {
        public long ChatId { get; set; }
        public long TGUserId { get; set; }
        public TelegramChatRefreshEvent(long chatId, long tgUserId = 0)
        {
            ChatId = chatId; 
            TGUserId = tgUserId;
        }
    }
}
