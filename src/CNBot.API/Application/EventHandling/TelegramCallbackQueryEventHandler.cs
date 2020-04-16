using CNBot.API.Application.Events;
using CNBot.API.Services;
using CNBot.Core.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CNBot.API.Application.EventHandling
{
    public class TelegramCallbackQueryEventHandler : IIntegrationEventHandler<TelegramCallbackQueryEvent>
    {
        private readonly ILogger _logger;
        private readonly IMessageService _messageService;
        public TelegramCallbackQueryEventHandler(
            ILogger<TelegramCallbackQueryEventHandler> logger,
            IMessageService messageService)
        {
            _logger = logger;
            _messageService = messageService;
        }
        public async Task Handle(TelegramCallbackQueryEvent @event)
        {
            try
            {
                await _messageService.FeedCallbackQuery(@event.CallbackQuery);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理消息回调失败");
            }
        }
    }
}
