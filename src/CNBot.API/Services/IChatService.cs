using CNBot.Core.Dtos;
using CNBot.Core.Entities.Chats;
using CNBot.Core.Paging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CNBot.API.Services
{
    public interface IChatService
    {
        Task CreateOrUpdateMember(ChatMember chatMember);
        Task<Chat> GetById(long id);
        Task<Chat> GetByTGChatId(long tgChatId);
        IPagedResult<Chat> GetChatsPaged(int pagedIndex = 1, int pageSize = 20, string category = null, long tgUserId = 0, string keywords = null);
        Task<Chat> GetOrCreateChat(TGChatDTO dto, long tgUserId = 0);
        Task<List<Category>> GetChatCategories();
    }
}