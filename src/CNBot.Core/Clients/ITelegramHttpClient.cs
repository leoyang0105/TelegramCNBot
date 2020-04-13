using CNBot.Core.Dtos;
using System.Threading.Tasks;

namespace CNBot.Core.Clients
{
    public interface ITelegramHttpClient
    {
        Task<TGResponseDTO> GetChat(string chatId);
        Task<TGResponseDTO> GetChatAdministrators(string chatId);
        Task<TGResponseDTO> GetChatMembersCount(string chatId);
        Task<TGResponseDTO> SendMessage(object dto);
    }
}