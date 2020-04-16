using CNBot.Core;
using CNBot.Core.Clients;
using CNBot.Core.Dtos;
using CNBot.Core.Entities.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CNBot.API.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserCommand> _userCommandRepository;
        public UserService(
            IRepository<User> userRepository,
            IRepository<UserCommand> userCommandRepository)
        {
            _userRepository = userRepository;
            _userCommandRepository = userCommandRepository;
        }
        public async Task<User> GetOrCreateUser(TGUserDTO dto)
        {
            var user = await _userRepository.TableNoTracking.FirstOrDefaultAsync(s => s.TGUserId == dto.Id);
            if (user == null)
            {
                var utcNow = DateTime.UtcNow;
                user = new User
                {
                    Created = utcNow,
                    FirstName = dto.FirstName,
                    IsBot = dto.IsBot,
                    LanguageCode = dto.LanguageCode,
                    LastName = dto.LastName,
                    TGUserId = dto.Id,
                    Updated = utcNow,
                    UserName = dto.Username
                };
                await _userRepository.AddAsync(user);
            }
            return user;
        }
        public async Task<UserCommand> FindLastCommand(long userId)
        {
            return await _userCommandRepository.TableNoTracking.OrderByDescending(s => s.Created).FirstOrDefaultAsync(c => c.UserId == userId);
        }
        public async Task CompleteCommand(UserCommand command)
        {
            command.Completed = true;
            await _userCommandRepository.UpdateAsync(command);
        }
        public async Task AddCommand(UserCommand command)
        {
            await _userCommandRepository.AddAsync(command);
        }

        public async Task<UserCommand> FindLastCommand(long tgUserId, long tgMessageId)
        {
            return await _userCommandRepository.TableNoTracking.OrderByDescending(s => s.Created)
                .FirstOrDefaultAsync(c => c.User.TGUserId == tgUserId && c.TGMessageId == tgMessageId);
        }
    }
}
