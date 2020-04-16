using CNBot.API.Application.Events;
using CNBot.Core;
using CNBot.Core.Caching;
using CNBot.Core.Clients;
using CNBot.Core.Dtos;
using CNBot.Core.Entities.Chats;
using CNBot.Core.EventBus.Abstractions;
using CNBot.Core.Paging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CNBot.API.Services
{
    public class ChatService : IChatService
    {
        private readonly IEventBus _eventBus;
        private readonly ICacheManager _cacheManager;
        private readonly ITelegramHttpClient _telegramHttpClient;
        private readonly IRepository<Chat> _chatRepository;
        private readonly IRepository<ChatMember> _chatMemberRepository;
        private readonly IRepository<Category> _categoryRepository;
        public ChatService(
            IEventBus eventBus,
            ICacheManager cacheManager,
            ITelegramHttpClient telegramHttpClient,
            IRepository<Chat> chatRepository,
            IRepository<ChatMember> chatMemberRepository,
            IRepository<Category> categoryRepository)
        {
            _eventBus = eventBus;
            _cacheManager = cacheManager;
            _telegramHttpClient = telegramHttpClient;
            _chatRepository = chatRepository;
            _chatMemberRepository = chatMemberRepository;
            _categoryRepository = categoryRepository;
        }
        public async Task<Chat> GetOrCreateChat(TGChatDTO dto, long tgUserId = 0)
        {
            var chat = await this.GetByTGChatId(dto.Id);
            if (chat == null)
            {
                var utcNow = DateTime.UtcNow;
                chat = new Chat
                {
                    ChatType = dto.GetChatType(),
                    Created = utcNow,
                    Description = dto.Description,
                    TGChatId = dto.Id,
                    Title = dto.Title,
                    Updated = utcNow,
                    UserName = dto.Username,
                    InviteLink = dto.InviteLink
                };
                await _chatRepository.AddAsync(chat);
                _eventBus.Publish(new TelegramChatRefreshEvent(chat.Id, tgUserId));
            }
            return chat;
        }

        public async Task CreateOrUpdateMember(ChatMember chatMember)
        {
            var entity = await _chatMemberRepository.TableNoTracking.FirstOrDefaultAsync(s => s.ChatId == chatMember.ChatId && s.TGUserId == chatMember.TGUserId);
            if (entity == null)
            {
                await _chatMemberRepository.AddAsync(chatMember);
            }
            else
            {
                if (chatMember.Status != entity.Status || chatMember.Permissions != entity.Permissions)
                {
                    entity.Status = chatMember.Status;
                    entity.Permissions = chatMember.Permissions;
                    await _chatMemberRepository.UpdateAsync(entity);
                }
            }
        }

        public async Task<Chat> GetById(long id)
        {
            return await _chatRepository.TableNoTracking.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Chat> GetByTGChatId(long tgChatId)
        {
            return await _chatRepository.TableNoTracking.FirstOrDefaultAsync(s => s.TGChatId == tgChatId);
        }

        public IPagedResult<Chat> GetChatsPaged(int pagedIndex = 1, int pageSize = 20, string category = null, long tgUserId = 0, string keywords = null)
        {
            var query = _chatRepository.TableNoTracking.Where(q => q.ChatType != ChatType.Private);
            if (tgUserId != 0)
            {
                query = query.Where(q => q.CreatorId == tgUserId);
            }
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(q => q.ChatCategories.Any(c => c.Category.Name == category));
            }
            if (!string.IsNullOrEmpty(keywords))
            {
                query = query.Where(q => q.Description.Contains(keywords) || q.Title.Contains(keywords) || q.UserName.Contains(keywords));
            }
            return new PagedResult<Chat>(query.OrderByDescending(q => q.MembersCount).ThenBy(q => q.Id), pagedIndex, pageSize);
        }
        public async Task<List<Category>> GetChatCategories()
        {
            return await _cacheManager.GetOrSetAsync(Category.AllCacheKey, () => _categoryRepository.TableNoTracking.Where(s => s.Published).ToListAsync());
        }
    }
}
