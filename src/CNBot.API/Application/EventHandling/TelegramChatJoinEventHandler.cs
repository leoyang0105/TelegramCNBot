using CNBot.API.Application.Events;
using CNBot.API.Services;
using CNBot.Core.Clients;
using CNBot.Core.Dtos;
using CNBot.Core.Entities.Chats;
using CNBot.Core.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CNBot.API.Application.EventHandling
{
    public class TelegramChatJoinEventHandler : IIntegrationEventHandler<TelegramChatJoinEvent>
    {
        private readonly ILogger _logger;
        private readonly IEventBus _eventBus;
        private readonly IChatService _chatService;
        private readonly ITelegramHttpClient _telegramHttpClient;
        public TelegramChatJoinEventHandler(
            ILogger<TelegramChatJoinEventHandler> logger,
            IEventBus eventBus,
            IChatService chatService,
            ITelegramHttpClient telegramHttpClient)
        {
            _logger = logger;
            _eventBus = eventBus;
            _chatService = chatService;
            _telegramHttpClient = telegramHttpClient;
        }
        public async Task Handle(TelegramChatJoinEvent @event)
        {
            try
            {
                var username = @event.ChatUserName.Trim();
                username = username.StartsWith("https://t.me/") ? username.Replace("https://t.me/", string.Empty) : username;
                username = username.Contains("@") ? username : $"@{username}";
                var chatResponse = await _telegramHttpClient.GetChat(username);
                var message = new TGSendMessageDTO
                {
                    ChatId = @event.TgChatId
                };
                if (chatResponse.IsOK)
                {
                    var type = chatResponse.Result.GetChatType();
                    if (type == ChatType.Private || type == ChatType.None)
                    {
                        message.Text = "用户名仅支持群组或频道";
                    }
                    else
                    {
                        var chat = await _chatService.GetByTGChatId(chatResponse.Result.Id);
                        if (chat == null)
                        {
                            chat = await _chatService.GetOrCreateChat(chatResponse.Result, @event.TgUserId);
                            message.Text = $"【{chatResponse.Result.Title}】 收录成功！ 你可以继续输入其他群组用户名";
                            _eventBus.Publish(new TelegramChatRefreshEvent(chat.Id));
                        }
                        else
                        {
                            message.Text = "此群组已经收录过了，请勿重复操作！";
                        }
                    }
                }
                else
                {
                    message.Text = "未能找到群组或者频道";
                }
                await _telegramHttpClient.SendMessage(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "添加群组失败");
            }
        }
    }
}
