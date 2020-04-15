using CNBot.API.Application.Events;
using CNBot.Core.EventBus.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CNBot.API.Application.EventHandling
{
    public class TelegramChatUpdateEventHandler : IIntegrationEventHandler<TelegramChatUpdateEvent>
    {
        public async Task Handle(TelegramChatUpdateEvent @event)
        {
            await Task.FromResult(0);
        }
    }
}
