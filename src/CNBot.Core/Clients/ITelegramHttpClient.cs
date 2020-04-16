using CNBot.Core.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CNBot.Core.Clients
{
    public interface ITelegramHttpClient
    {
        Task<TGResponseDTO<TGChatDTO>> GetChat(string chatId);
        Task<TGResponseDTO<TGChatDTO>> GetChatByNameOrLink(string nameOrLink);
        Task<TGResponseDTO<List<TGChatMemberDTO>>> GetChatAdministrators(string chatId);
        Task<TGResponseDTO<int>> GetChatMembersCount(string chatId);
        Task<TGResponseDTO<TGMessageDTO>> SendMessage(TGSendMessageDTO dto);
        Task<TGResponseDTO<TGMessageDTO>> EditMessage(TGEditMessageTextDTO dto);
        Task<TGResponseDTO<object>> AnswerCallbackQuery(string callbackQueryId);
    }
}