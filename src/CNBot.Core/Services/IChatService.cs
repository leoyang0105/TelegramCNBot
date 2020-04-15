using CNBot.Core.Entities.Chats;
using CNBot.Core.Paging;
using System.Threading.Tasks;

namespace CNBot.Core.Services
{
    public interface IChatService
    {
        Task CreateChat(Chat chat);
        Task<Chat> GetByTGChatId(long tgChatId);
        Task<Chat> GetById(long id);
        IPagedResult<Chat> GetChatsPaged(int pagedIndex = 1, int pageSize = 20, long tgUserId = 0, string keywords = null);
        Task CreateOrUpdateMember(ChatMember chatMember);
    }
}