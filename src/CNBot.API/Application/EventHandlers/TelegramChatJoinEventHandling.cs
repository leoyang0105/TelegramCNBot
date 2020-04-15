using CNBot.API.Application.Events;
using CNBot.Core.Clients;
using CNBot.Core.Dtos;
using CNBot.Core.Entities.Chats;
using CNBot.Core.EventBus.Abstractions;
using CNBot.Core.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CNBot.API.Application.EventHandlers
{
    public class TelegramChatJoinEventHandling : IIntegrationEventHandler<TelegramChatJoinEvent>
    {
        private readonly ILogger _logger;
        private readonly IEventBus _eventBus;
        private readonly IChatService _chatService;
        private readonly ITelegramHttpClient _telegramHttpClient;
        public TelegramChatJoinEventHandling(
            ILogger<TelegramChatJoinEventHandling> logger,
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
                        message.Text = $"【{chatResponse.Result.Title}】 收录成功！ 你可以继续输入其他群组用户名";
                        var utcNow = DateTime.UtcNow;
                        chat = new Chat()
                        {
                            Created = utcNow,
                            Description = chatResponse.Result.Description,
                            TGChatId = chatResponse.Result.Id,
                            Title = chatResponse.Result.Title,
                            UserName = chatResponse.Result.Username,
                            Updated = utcNow,
                            ChatType = type,
                        };
                        await _chatService.CreateChat(chat);
                        _eventBus.Publish(new TelegramChatRefreshEvent(chat.Id));
                    }
                    else
                    {
                        message.Text = "此群组已经收录过了，请勿重复操作！ \n 如果本次操作是群主，将自动更改归属权至群主！";
                    }
                }
            }
            else
            {
                message.Text = "未能找到群组或者频道";
            }
            await _telegramHttpClient.SendMessage(message);
        }
    }
}
