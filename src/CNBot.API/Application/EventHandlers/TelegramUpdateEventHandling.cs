using CNBot.API.Application.Events;
using CNBot.API.Extensions;
using CNBot.Core;
using CNBot.Core.Dtos;
using CNBot.Core.Entities.Chats;
using CNBot.Core.Entities.Messages;
using CNBot.Core.Entities.Users;
using CNBot.Core.EventBus.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CNBot.API.Application.EventHandlers
{
    public class TelegramUpdateEventHandling : IIntegrationEventHandler<TelegramUpdateEvent>
    {
        private readonly ILogger _logger;
        private readonly IEventBus _eventBus;
        private readonly IRepository<Chat> _chatRepository;
        private readonly IRepository<User> _userRepository;
        public TelegramUpdateEventHandling(
            ILogger<TelegramUpdateEventHandling> logger,
            IEventBus eventBus,
            IRepository<Chat> chatRepository,
            IRepository<User> userRepository)
        {
            _logger = logger;
            _eventBus = eventBus;
            _chatRepository = chatRepository;
            _userRepository = userRepository;
        }
        public async Task Handle(TelegramUpdateEvent @event)
        {
            try
            {
                var dto = JsonConvert.DeserializeObject<TGUpdateDTO>(@event.Body).Message;

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
                var utcNow = DateTime.UtcNow;
                var chat = await _chatRepository.TableNoTracking.FirstOrDefaultAsync(c => c.TGChatId == dto.Chat.Id);
                if (chat == null)
                {
                    chat = new Chat()
                    {
                        Created = utcNow,
                        Description = dto.Chat.Description,
                        TGChatId = dto.Chat.Id,
                        Title = dto.Chat.Title,
                        UserName = dto.Chat.Username,
                        Updated = utcNow,
                        ChatType = dto.Chat.GetChatType(),
                    };
                    await _chatRepository.AddAsync(chat);
                    _eventBus.Publish(new TelegramChatRefreshEvent(chat.Id));
                }
                var user = await _userRepository.TableNoTracking.FirstOrDefaultAsync(u => u.TGUserId == dto.From.Id);
                if (user == null)
                {
                    user = new User
                    {
                        Created = utcNow,
                        FirstName = dto.From.FirstName,
                        IsBot = dto.From.IsBot,
                        LanguageCode = dto.From.LanguageCode,
                        LastName = dto.From.LastName,
                        TGUserId = dto.From.Id,
                        Updated = utcNow,
                        UserName = dto.From.Username
                    };
                    await _userRepository.AddAsync(user);
                }
                _eventBus.Publish(new TelegramMessageEvent(message, user.Id, chat.TGChatId, dto.Chat.GetChatType()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理Telegram消息失败");
            }
            await Task.FromResult(0);
        }
    }
}
