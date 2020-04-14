using CNBot.API.Application.Events;
using CNBot.Core;
using CNBot.Core.Clients;
using CNBot.Core.Dtos;
using CNBot.Core.Entities.Messages;
using CNBot.Core.Entities.Users;
using CNBot.Core.EventBus.Abstractions;
using CNBot.Core.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CNBot.API.Application.EventHandlers
{
    public class TelegramMessageEventHandling : IIntegrationEventHandler<TelegramMessageEvent>
    {
        private readonly ILogger _logger;
        private readonly IRepository<Message> _messageRepository;
        private readonly IUserService _userService;
        private readonly ITelegramHttpClient _telegramHttpClient;
        public TelegramMessageEventHandling(
            ILogger<TelegramMessageEventHandling> logger,
            IRepository<Message> messageRepository,
            IUserService userService,
            ITelegramHttpClient telegramHttpClient)
        {
            _logger = logger;
            _messageRepository = messageRepository;
            _userService = userService;
            _telegramHttpClient = telegramHttpClient;
        }
        public async Task Handle(TelegramMessageEvent @event)
        {
            try
            {
                await _messageRepository.AddAsync(@event.Message);
                await this.HandleMessage(@event);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理用户消息失败");
            }
        }
        private async Task HandleMessage(TelegramMessageEvent @event)
        {
            if (@event.ChatType != Core.Entities.Chats.ChatType.Private)
            {
                // Only use command in private chat
                var message = new TGSendMessageDTO
                {
                    ChatId = @event.TGChatId,
                    Text = "群组中无法使用命令，请在私聊中使用",
                    ReplyToMessageId = @event.Message.TGMessageId
                };
                await _telegramHttpClient.SendMessage(message);
                return;
            }
            UserCommand command = null;
            var utcNow = DateTime.UtcNow;

            if (@event.Message.MessageEntities.Any(s => s.Type.Equals(MessageEntityType.bot_command)))
            {
                command = new UserCommand
                {
                    Created = utcNow,
                    Completed = false,
                    Type = ConvertCommandType(@event.Message.Text),
                    UserId = @event.UserId
                };
                if (command.Type == UserCommandType.None)
                {
                    // Don't know how to handle this command. Just complete this command
                    command.Completed = true;
                }
                await _userService.AddCommand(command);
            }
            else
            {
                command = await _userService.FindLastCommand(@event.UserId);
            }
            if (command == null)
            {
                // No command found. So we show the default message E.g :what can i help you?
                //TODO Send message : default message
            }
            else
            {
                await this.HandleCommand(command, @event);
            }
        }
        private async Task HandleCommand(UserCommand command, TelegramMessageEvent @event)
        {
            var message = new TGSendMessageDTO
            {
                ChatId = @event.TGChatId
            };
            switch (command.Type)
            {
                case UserCommandType.Remove:
                    message.Text = "请点击下方按钮选择要删除的群组";
                    message.ReplyMarkup = new TGReplyKeyboardMarkup
                    {
                        OneTimeKeyboard = true,
                        Keyboard = new[]
                        {
                            new List<TGReplyKeyboardMarkup.KeyboardButton>()
                            {
                                new TGReplyKeyboardMarkup.KeyboardButton
                                {
                                   Text = "@cn_tg_bot",
                                   RequestContact=true
                                },
                                 new TGReplyKeyboardMarkup.KeyboardButton
                                {
                                   Text = "@ProgrammerClub",
                                   RequestLocation=true
                                }
                            }
                        }
                    };
                    break;
                case UserCommandType.Help:
                    message.Text =
                        "/list 列出已收录群组分类 \n" +
                        "/mylist 列出你是创建人的群组或频道 \n" +
                        "/join 收录群组或频道 \n" +
                        "/update 更改群组或频道分类 \n" +
                        "/remove 移除群组或频道 \n" +
                        "/reset 重置命令状态";
                    break;
                case UserCommandType.Update:
                    message.Text = "请点击下方按钮选择要更改的群组或频道";
                    break;
                case UserCommandType.Join:
                    message.Text = "请输入群组或者频道用户名";
                    break;
                case UserCommandType.List:
                    message.Text = @"<a href=""https://t.me/cn_tg_bot"">CN_Bot</a>";
                    message.ParseMode = nameof(MessageParseModelType.Html);
                    break;
                case UserCommandType.MyList:
                    message.Text = @"<a href=""https://t.me/cn_tg_bot"">CN_Bot</a>";
                    message.ParseMode = nameof(MessageParseModelType.Html);
                    break;
                case UserCommandType.Reset:
                    message.Text = "命令状态已重置";
                    message.ReplyMarkup = new { remove_keyboard = true };
                    break;
            }
            command.Completed = true;
            await _userService.UpdateCommand(command);
            await _telegramHttpClient.SendMessage(message);
        }
        private UserCommandType ConvertCommandType(string text)
        {
            var command = UserCommandType.None;
            switch (text)
            {
                case "/help":
                    command = UserCommandType.Help;
                    break;
                case "/list":
                    command = UserCommandType.List;
                    break;
                case "/mylist":
                    command = UserCommandType.MyList;
                    break;
                case "/join":
                    command = UserCommandType.Join;
                    break;
                case "/Reset":
                    command = UserCommandType.Reset;
                    break;
                case "/remove":
                    command = UserCommandType.Remove;
                    break;
                case "/update":
                    command = UserCommandType.Update;
                    break;
            }
            return command;
        }
    }
}
