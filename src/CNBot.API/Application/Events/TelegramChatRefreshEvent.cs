using CNBot.Core.Events;

namespace CNBot.API.Application.Events
{
    public class TelegramChatRefreshEvent : IntegrationEvent
    {
        public long ChatId { get; set; }
        public TelegramChatRefreshEvent(long chatId)
        {
            ChatId = chatId;
        }
    }
}
