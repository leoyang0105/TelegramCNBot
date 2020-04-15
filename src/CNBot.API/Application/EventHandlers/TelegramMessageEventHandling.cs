using CNBot.API.Application.Events;
using CNBot.Core;
using CNBot.Core.Clients;
using CNBot.Core.Dtos;
using CNBot.Core.Entities.Chats;
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
        private readonly IEventBus _eventBus;
        private readonly IRepository<Message> _messageRepository;
        private readonly IUserService _userService;
        private readonly IChatService _chatService;
        private readonly ITelegramHttpClient _telegramHttpClient;
        public TelegramMessageEventHandling(
            ILogger<TelegramMessageEventHandling> logger,
            IEventBus eventBus,
            IRepository<Message> messageRepository,
            IUserService userService,
            IChatService chatService,
            ITelegramHttpClient telegramHttpClient)
        {
            _logger = logger;
            _eventBus = eventBus;
            _messageRepository = messageRepository;
            _userService = userService;
            _chatService = chatService;
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
                if (@event.Message.Text.ToUpper().Contains(ApplicationDefaults.CNBotUserName))
                {
                    // Only use command in private chat
                    var message = new TGSendMessageDTO
                    {
                        ChatId = @event.TGChatId,
                        Text = "群组中无法使用命令，请在私聊中使用",
                        ReplyToMessageId = @event.Message.TGMessageId,
                        ParseMode = nameof(MessageParseModelType.Html)
                    };
                    await _telegramHttpClient.SendMessage(message);
                }
                return;
            }
            UserCommand command = null;
            var utcNow = DateTime.UtcNow;

            if (@event.Message.MessageEntities.Any(s => s.Type.Equals(MessageEntityType.bot_command)))
            {
                if (@event.Message.Text.Contains("@"))
                {
                    if (!@event.Message.Text.EndsWith(ApplicationDefaults.CNBotUserName))
                    {

                        return;
                    }
                }
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
            if (command == null || command.Completed)
            {
                // No command found. So we show the default message E.g :what can i help you?
                var message = new TGSendMessageDTO
                {
                    ChatId = @event.TGChatId,
                    Text = "无法识别该命令，要查看帮助请输入 /help",
                    ReplyToMessageId = @event.Message.TGMessageId
                };
                await _telegramHttpClient.SendMessage(message);
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
                    {
                        if (ApplicationDefaults.Commands.Contains(@event.Message.Text))
                        {
                            message.Text = "【移除收录】请输入群组或频道用户名(可直接分享或粘贴群组链接)";
                            message.ReplyMarkup = new { remove_keyboard = true };
                            await _telegramHttpClient.SendMessage(message);
                        }
                        else
                        {
                            _eventBus.Publish(new TelegramChatRemoveEvent(@event.Message.FromTGUserId, @event.Message.Text));
                        }
                        break;
                    }
                //message.Text = "请点击下方按钮选择要删除的群组";
                //message.ReplyMarkup = new TGReplyKeyboardMarkup
                //{
                //    OneTimeKeyboard = true,
                //    Keyboard = new[]
                //    {
                //        new List<TGReplyKeyboardMarkup.KeyboardButton>()
                //        {
                //            new TGReplyKeyboardMarkup.KeyboardButton
                //            {
                //               Text = "@cn_tg_bot",
                //               RequestContact=true
                //            },
                //             new TGReplyKeyboardMarkup.KeyboardButton
                //            {
                //               Text = "@ProgrammerClub",
                //               RequestLocation=true
                //            }
                //        }
                //    }
                //};
                case UserCommandType.Help:
                    {
                        message.Text =
                            "/list 列出已收录群组分类 \n" +
                            "/mylist 列出你是创建人的群组或频道 \n" +
                            "/join 收录群组或频道 \n" +
                            "/update 更改群组或频道分类 \n" +
                            "/remove 移除群组或频道 \n" +
                            "/reset 重置命令状态";
                        command.Completed = true;
                        message.ReplyMarkup = new { remove_keyboard = true };
                        await _userService.UpdateCommand(command);
                        await _telegramHttpClient.SendMessage(message);
                        break;
                    }
                case UserCommandType.Update:
                    {
                        if (ApplicationDefaults.Commands.Contains(@event.Message.Text))
                        {
                            message.Text = "【更新群组】请输入群组或频道用户名(可直接分享或粘贴群组链接)";
                            message.ReplyMarkup = new { remove_keyboard = true };
                            await _telegramHttpClient.SendMessage(message);
                        }
                        else
                        {
                            _eventBus.Publish(new TelegramChatUpdateEvent(@event.Message.FromTGUserId, @event.Message.Text));
                        }
                        break;
                    }
                case UserCommandType.Join:
                    if (ApplicationDefaults.Commands.Contains(@event.Message.Text))
                    {
                        message.Text = "【收录群组】请输入群组或频道用户名(可直接分享或粘贴群组链接)";
                        message.ReplyMarkup = new { remove_keyboard = true };
                        await _telegramHttpClient.SendMessage(message);
                    }
                    else
                    {
                        _eventBus.Publish(new TelegramChatJoinEvent(@event.TGChatId, @event.Message.FromTGUserId, @event.Message.Text));
                    }
                    break;
                case UserCommandType.List:
                    await HandleCommandList(message, 0);
                    command.Completed = true;
                    await _userService.UpdateCommand(command);
                    break;
                case UserCommandType.MyList:
                    await HandleCommandList(message, @event.Message.FromTGUserId);
                    command.Completed = true;
                    await _userService.UpdateCommand(command);
                    break;
                case UserCommandType.Reset:
                    {

                        message.Text = "命令状态已重置";
                        message.ReplyMarkup = new { remove_keyboard = true };
                        command.Completed = true;
                        await _userService.UpdateCommand(command);
                        await _telegramHttpClient.SendMessage(message);
                        break;
                    }
            }
        }
        private UserCommandType ConvertCommandType(string text)
        {
            text = text.Replace(ApplicationDefaults.CNBotUserName, string.Empty);
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
                case "/update":
                    command = UserCommandType.Update;
                    break;
                case "/remove":
                    command = UserCommandType.Remove;
                    break;
                case "/reset":
                    command = UserCommandType.Reset;
                    break;
            }
            return command;
        }

        private async Task HandleCommandList(TGSendMessageDTO message, long tgUserId = 0)
        {
            var paged = _chatService.GetChatsPaged(1, 20, tgUserId, null);

            message.ParseMode = nameof(MessageParseModelType.Html);
            message.DisableWebPagePreview = true;
            foreach (var item in paged.Data)
            {
                var chatTypeIcon = string.Empty;
                if (item.ChatType == ChatType.Channel)
                {
                    chatTypeIcon = "📢|";
                }
                else if (item.ChatType == ChatType.Group || item.ChatType == ChatType.SuperGroup)
                {
                    chatTypeIcon = "👥|";
                }
                message.Text += $"{chatTypeIcon}👤{item.MembersCount}| <a href=\"https://t.me/{item.UserName}\">{item.Title}</a>\n";
            }
            // message.ReplyMarkup = this.BuildListPager(1, paged.TotalPages);

            await _telegramHttpClient.SendMessage(message);
        }
        private TGInlineKeyboardMarkup BuildListPager(int pageIndex, int totalPages)
        {
            var pager = new TGInlineKeyboardMarkup
            {
                InlineKeyboard = new[]
                {
                    new List<TGInlineKeyboardMarkup.InlineKeyboardButton>()
                    {
                        new TGInlineKeyboardMarkup.InlineKeyboardButton
                        {

                        }
                    }
                }
            };
            return pager;
        }
    }
}
