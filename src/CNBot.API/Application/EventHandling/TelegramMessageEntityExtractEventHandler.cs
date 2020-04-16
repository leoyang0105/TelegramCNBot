using CNBot.API.Application.Events;
using CNBot.API.Services;
using CNBot.Core;
using CNBot.Core.Clients;
using CNBot.Core.Dtos;
using CNBot.Core.Entities.Messages;
using CNBot.Core.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CNBot.API.Application.EventHandling
{
    public class TelegramMessageEntityExtractEventHandler : IIntegrationEventHandler<TelegramMessageEntityExtractEvent>
    {
        private readonly ILogger _logger;
        private readonly ITelegramHttpClient _telegramHttpClient;
        private readonly IChatService _chatService;
        public TelegramMessageEntityExtractEventHandler(
            ILogger<TelegramMessageEntityExtractEventHandler> logger,
            ITelegramHttpClient telegramHttpClient,
            IChatService chatService)
        {
            _logger = logger;
            _telegramHttpClient = telegramHttpClient;
            _chatService = chatService;
        }

        public async Task Handle(TelegramMessageEntityExtractEvent @event)
        {
            try
            {
                var list = @event.Entities.Where(e => e.GetEntityType() == MessageEntityType.text_link).ToList();
                while (list.Any())
                {
                    var entities = list.Take(ApplicationDefaults.ConcurrentTaskCount).ToList();
                    var tasks = entities.Select(entity => this.HandleUrlEntity(entity));
                    entities.ForEach(entity => list.Remove(entity));
                    await Task.WhenAll(tasks);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理MessageEntity失败");
            }
        }
        private async Task HandleUrlEntity(TGMessageEntityDTO entity)
        {
            try
            {
                var chatResponse = await _telegramHttpClient.GetChatByNameOrLink(entity.Url);
                if (chatResponse.IsOK)
                {
                    await _chatService.GetOrCreateChat(chatResponse.Result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "处理MessageEntity失败");
            }
        }
    }
}
