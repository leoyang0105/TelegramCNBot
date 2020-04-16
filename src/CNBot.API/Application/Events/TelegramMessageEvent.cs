using CNBot.Core.Dtos;
using CNBot.Core.Events;

namespace CNBot.API.Application.Events
{
    public class TelegramMessageEvent : IntegrationEvent
    {
        public TGMessageDTO Message { get; set; }
        public TelegramMessageEvent(TGMessageDTO message)
        {
            Message = message;
        }
    }
}
