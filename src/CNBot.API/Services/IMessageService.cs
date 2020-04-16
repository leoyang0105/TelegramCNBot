using CNBot.Core.Dtos;
using CNBot.Core.Entities.Messages;
using CNBot.Core.Entities.Users;
using System.Threading.Tasks;

namespace CNBot.API.Services
{
    public interface IMessageService
    {
        Task FeedCallbackQuery(TGCallbackQueryDTO dto);
        Task FeedMessage(TGMessageDTO dto);
        Task HandleCallbackQuery(TGCallbackQueryDTO dto);
        Task HandleMessage(TGMessageDTO dto, User user);
        Task<Message> SaveMessage(TGMessageDTO dto);
    }
}