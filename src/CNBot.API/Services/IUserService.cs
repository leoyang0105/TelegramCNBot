using CNBot.Core.Dtos;
using CNBot.Core.Entities.Users;
using System.Threading.Tasks;

namespace CNBot.API.Services
{
    public interface IUserService
    {
        Task AddCommand(UserCommand command);
        Task<UserCommand> FindLastCommand(long userId);
        Task<UserCommand> FindLastCommand(long tgUserId, long tgMessageId);
        Task<User> GetOrCreateUser(TGUserDTO dto);
        Task CompleteCommand(UserCommand command);
    }
}