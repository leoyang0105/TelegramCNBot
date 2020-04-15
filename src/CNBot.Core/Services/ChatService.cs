using CNBot.Core.Entities.Chats;
using CNBot.Core.Paging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNBot.Core.Services
{
    public class ChatService : IChatService
    {
        private readonly IRepository<Chat> _chatRepository;
        private readonly IRepository<ChatMember> _chatMemberRepository;
        public ChatService(
            IRepository<Chat> chatRepository,
            IRepository<ChatMember> chatMemberRepository)
        {
            _chatRepository = chatRepository;
            _chatMemberRepository = chatMemberRepository;
        }

        public async Task CreateChat(Chat chat)
        {
            await _chatRepository.AddAsync(chat);
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

        public IPagedResult<Chat> GetChatsPaged(int pagedIndex = 1, int pageSize = 20, long tgUserId = 0, string keywords = null)
        {
            var query = _chatRepository.TableNoTracking.Where(q => q.ChatType != ChatType.Private);
            if (tgUserId != 0)
            {
                query = query.Where(q => q.CreatorId == tgUserId);
            }
            if (!string.IsNullOrEmpty(keywords))
            {
                query = query.Where(q => q.Description.Contains(keywords) || q.Title.Contains(keywords) || q.UserName.Contains(keywords));
            }
            return new PagedResult<Chat>(query.OrderByDescending(q => q.MembersCount).ThenBy(q => q.Id), pagedIndex, pageSize);
        }
    }
}
