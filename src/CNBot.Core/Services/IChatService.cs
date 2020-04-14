using CNBot.Core.Entities.Chats;
using System.Threading.Tasks;

namespace CNBot.Core.Services
{
    public interface IChatService
    {
        Task CreateOrUpdateMember(ChatMember chatMember);
    }
}