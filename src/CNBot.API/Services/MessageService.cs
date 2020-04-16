using CNBot.API.Application.Events;
using CNBot.API.Extensions;
using CNBot.Core;
using CNBot.Core.Clients;
using CNBot.Core.Dtos;
using CNBot.Core.Entities.Chats;
using CNBot.Core.Entities.Messages;
using CNBot.Core.Entities.Users;
using CNBot.Core.EventBus.Abstractions;
using CNBot.Core.Paging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CNBot.API.Services
{
    public class MessageService : IMessageService
    {
        private readonly IEventBus _eventBus;
        private readonly IRepository<Message> _repository;
        private readonly ITelegramHttpClient _telegramHttpClient;
        private readonly IChatService _chatService;
        private readonly IUserService _userService;
        public MessageService(
            IEventBus eventBus,
            IRepository<Message> repository,
            ITelegramHttpClient telegramHttpClient,
            IChatService chatService,
            IUserService userService)
        {
            _eventBus = eventBus;
            _repository = repository;
            _chatService = chatService;
            _userService = userService;
            _telegramHttpClient = telegramHttpClient;
        }
        public async Task<Message> SaveMessage(TGMessageDTO dto)
        {
            var message = new Message
            {
                Created = dto.Date.ToDateTime().Value,
                Updated = dto.EditDate.ToDateTime(),
                FromTGUserId = dto.From.Id,
                Text = dto.Text,
                TGChatId = dto.Chat.Id,
                TGMessageId = dto.MessageId
            };
            if (dto.Entities != null && dto.Entities.Any())
            {
                foreach (var entityDTO in dto.Entities)
                {
                    message.MessageEntities.Add(new MessageEntity
                    {
                        Language = entityDTO.Language,
                        Length = entityDTO.Length,
                        Offset = entityDTO.Offset,
                        Type = entityDTO.GetEntityType(),
                        Url = entityDTO.Url
                    });
                }
            }
            await _repository.AddAsync(message);
            return message;
        }
        public async Task FeedMessage(TGMessageDTO dto)
        {
            await _chatService.GetOrCreateChat(dto.Chat, dto.From.Id);
            var user = await _userService.GetOrCreateUser(dto.From);
            await this.SaveMessage(dto);
            await this.HandleMessage(dto, user);
        }
        public async Task FeedCallbackQuery(TGCallbackQueryDTO dto)
        {
            await this.HandleCallbackQuery(dto);
            await _telegramHttpClient.AnswerCallbackQuery(dto.Id);
        }
        public async Task HandleMessage(TGMessageDTO dto, User user)
        {
            var chatType = dto.Chat.GetChatType();
            if (chatType != ChatType.Private)
            {
                if (dto.Text.ToUpper().Contains(ApplicationDefaults.CNBotUserName))
                {
                    // Only use command in private chat 
                    await _telegramHttpClient.SendMessage(TGSendMessageDTO.BuildOnlyUseInPrivateMessage(dto.Chat.Id, dto.MessageId));
                }
                //If not my command. Nothing to do
                return;
            }
            var utcNow = DateTime.UtcNow;
            UserCommandType commandType = dto.GetCommandType();
            if (commandType == UserCommandType.None)
            {
                var lastCommand = await _userService.FindLastCommand(user.Id);
                if (lastCommand == null || lastCommand.Completed)
                {
                    // No command found. So we just show the default message  
                    await _telegramHttpClient.SendMessage(TGSendMessageDTO.BuildUnrecognizedMessage(dto.Chat.Id, dto.MessageId));
                    return;
                }
                else
                {
                    commandType = lastCommand.Type;
                }
            }
            var command = new UserCommand
            {
                Created = utcNow,
                Completed = false,
                Type = commandType,
                UserId = user.Id,
                Text = dto.Text,
                TGMessageId = dto.MessageId
            };
            await _userService.AddCommand(command);
            await this.HandleCommand(dto, command);
        }
        private async Task HandleCommand(TGMessageDTO dto, UserCommand command)
        {
            switch (command.Type)
            {
                case UserCommandType.Help:
                    {
                        command.Completed = true;
                        await _userService.CompleteCommand(command);
                        await _telegramHttpClient.SendMessage(TGSendMessageDTO.BuildHelpMessage(dto.Chat.Id));
                        break;
                    }
                case UserCommandType.Reset:
                    {
                        command.Completed = true;
                        await _userService.CompleteCommand(command);
                        await _telegramHttpClient.SendMessage(TGSendMessageDTO.BuildResetMessage(dto.Chat.Id));
                        break;
                    }
                case UserCommandType.List:
                    {
                        if (ApplicationDefaults.Commands.Contains(dto.Text))
                        {
                            var categories = await _chatService.GetChatCategories();
                            var message = TGSendMessageDTO.BuildChatCategoriesMessage(dto.Chat.Id, categories.OrderBy(c => c.DisplayOrder).Select(c => c.Name).ToList());
                            await _telegramHttpClient.SendMessage(message);
                        }
                        else
                        {
                            var category = dto.Text;
                            if (!string.IsNullOrWhiteSpace(category) && category.StartsWith("全部群组", StringComparison.OrdinalIgnoreCase))
                            {
                                category = null;
                            }
                            var paged = _chatService.GetChatsPaged(pagedIndex: 1, pageSize: 20, category: category);
                            var message = TGSendMessageDTO.BuildChatListMessage(paged, dto.Chat.Id, dto.MessageId);
                            await _telegramHttpClient.SendMessage(message);
                        }
                        break;
                    }
                case UserCommandType.MyList:
                    {
                        //TODO
                        break;
                    }
                case UserCommandType.Join:
                    {
                        if (ApplicationDefaults.Commands.Contains(dto.Text))
                        {
                            await _telegramHttpClient.SendMessage(TGSendMessageDTO.BuildChatJoinMessage(dto.Chat.Id));
                        }
                        else
                        {
                            _eventBus.Publish(new TelegramChatJoinEvent(dto.Chat.Id, dto.From.Id, dto.Text));
                        }
                        break;
                    }
                case UserCommandType.Remove:
                    {
                        await _telegramHttpClient.SendMessage(TGSendMessageDTO.BuildChatRemoveMessage(dto.Chat.Id));
                        break;
                    }
                case UserCommandType.Update:
                    {
                        await _telegramHttpClient.SendMessage(TGSendMessageDTO.BuildChatUpdateMessage(dto.Chat.Id));
                        break;
                    }
                case UserCommandType.Search:
                    {
                        if (ApplicationDefaults.Commands.Contains(dto.Text))
                        {
                            await _telegramHttpClient.SendMessage(TGSendMessageDTO.BuildChatSearchMessage(dto.Chat.Id));
                        }
                        else
                        {
                            var paged = _chatService.GetChatsPaged(pagedIndex: 1, pageSize: 20, keywords: dto.Text);
                            var message = TGSendMessageDTO.BuildChatListMessage(paged, dto.Chat.Id, dto.MessageId);
                            await _telegramHttpClient.SendMessage(message);
                        }
                        break;
                    }
            }
        }
        public async Task HandleCallbackQuery(TGCallbackQueryDTO dto)
        {
            var queryData = JsonConvert.DeserializeObject<TGCallbackQueryDTO.TGCallbackQueryDataDTO>(dto.Data);
            if (queryData.PageIndex == 0)
            {
                return;
            }
            var command = await _userService.FindLastCommand(dto.From.Id, queryData.MessageId);
            if (command == null)
                return;
            if (command.Type == UserCommandType.List)
            {
                var category = command.Text;
                if (!string.IsNullOrWhiteSpace(category) && category.StartsWith("全部群组", StringComparison.OrdinalIgnoreCase))
                {
                    category = null;
                }
                
                var paged = _chatService.GetChatsPaged(pagedIndex: queryData.PageIndex, pageSize: 20, category: category);
                var message = TGSendMessageDTO.BuildChatListMessage(paged, dto.Message.Chat.Id, queryData.MessageId);
                var editedMessage = TGSendMessageDTO.BuildChatListEditMessage(message, queryData.MessageId);
                await _telegramHttpClient.EditMessage(editedMessage);
            }
        }
    }
}
