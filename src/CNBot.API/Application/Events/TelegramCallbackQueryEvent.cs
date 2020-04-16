using CNBot.Core.Dtos;
using CNBot.Core.Events;

namespace CNBot.API.Application.Events
{
    public class TelegramCallbackQueryEvent : IntegrationEvent
    {
        public TGCallbackQueryDTO CallbackQuery { get; set; }
        public TelegramCallbackQueryEvent(TGCallbackQueryDTO callbackQuery)
        {
            CallbackQuery = callbackQuery;
        }
    }
}
