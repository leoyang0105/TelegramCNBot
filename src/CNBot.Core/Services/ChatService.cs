using CNBot.Core.Entities.Chats;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CNBot.Core.Services
{
    public class ChatService : IChatService
    {
        private readonly IRepository<ChatMember> _chatMemberRepository;
        public ChatService(IRepository<ChatMember> chatMemberRepository)
        {
            _chatMemberRepository = chatMemberRepository;
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
    }
}
