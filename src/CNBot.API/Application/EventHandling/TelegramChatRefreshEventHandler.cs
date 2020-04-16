using CNBot.API.Application.Events;
using CNBot.API.Services;
using CNBot.Core;
using CNBot.Core.Clients;
using CNBot.Core.Entities.Chats;
using CNBot.Core.EventBus.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CNBot.API.Application.EventHandling
{
    public class TelegramChatRefreshEventHandler : IIntegrationEventHandler<TelegramChatRefreshEvent>
    {
        private readonly ILogger _logger;
        private readonly IRepository<Chat> _chatRepository;
        private readonly ITelegramHttpClient _telegramHttpClient;
        private readonly IChatService _chatService;
        public TelegramChatRefreshEventHandler(
            ILogger<TelegramChatRefreshEventHandler> logger,
            IRepository<Chat> chatRepository,
            ITelegramHttpClient telegramHttpClient,
            IChatService chatService)
        {
            _logger = logger;
            _chatRepository = chatRepository;
            _telegramHttpClient = telegramHttpClient;
            _chatService = chatService;
        }
        public async Task Handle(TelegramChatRefreshEvent @event)
        {
            try
            {
                var chat = await _chatRepository.TableNoTracking
                    .SingleOrDefaultAsync(c => c.Id == @event.ChatId);
                if (chat == null)
                    throw new NullReferenceException("The chat was not found");
                var chatResponse = await _telegramHttpClient.GetChat(chat.TGChatId.ToString());
                if (chat.ChatType != ChatType.Private)
                {
                    var membersCountResponse = await _telegramHttpClient.GetChatMembersCount(chat.TGChatId.ToString());
                    if (membersCountResponse.IsOK)
                    {
                        chat.MembersCount = membersCountResponse.Result;
                    }
                }
                if (chatResponse.IsOK)
                {
                    chat.UserName = chatResponse.Result.Username;
                    chat.Description = chatResponse.Result.Description;
                    chat.ChatType = chatResponse.Result.GetChatType();
                    chat.Title = chatResponse.Result.Title;
                    chat.Updated = DateTime.Now;
                }
                await _chatRepository.UpdateAsync(chat);
                if (chat.ChatType == ChatType.Group || chat.ChatType == ChatType.SuperGroup)
                {
                    var administratorsResponse = await _telegramHttpClient.GetChatAdministrators(chat.TGChatId.ToString());
                    if (administratorsResponse.IsOK)
                    {
                        foreach (var admin in administratorsResponse.Result)
                        {
                            var member = new ChatMember
                            {
                                ChatId = chat.Id,
                                TGUserId = admin.User.Id,
                                Status = admin.GetStatus(),
                                Permissions = admin.GetPermission(),
                                Created = DateTime.UtcNow
                            };
                            await _chatService.CreateOrUpdateMember(member);

                            if (member.Status == ChatMemberStatusType.Creator && 
                                member.TGUserId == @event.TGUserId && 
                                member.TGUserId != chat.CreatorId)  //更新归属到群主
                            {
                                chat.CreatorId = member.TGUserId;
                                await _chatRepository.UpdateAsync(chat);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "刷新填充聊天失败");
            }
        }
    }
}
