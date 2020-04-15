using CNBot.API.Application.Events;
using CNBot.Core.EventBus.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CNBot.API.Application.EventHandlers
{
    public class TelegramChatRemoveEventHandling : IIntegrationEventHandler<TelegramChatRemoveEvent>
    {
        public async Task Handle(TelegramChatRemoveEvent @event)
        {
            await Task.FromResult(0);
        }
    }
}
