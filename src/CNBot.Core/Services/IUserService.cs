using CNBot.Core.Entities.Users;
using System.Threading.Tasks;

namespace CNBot.Core.Services
{
    public interface IUserService
    {
        Task AddCommand(UserCommand command);
        Task<UserCommand> FindLastCommand(long userId);
        Task UpdateCommand(UserCommand command);
    }
}