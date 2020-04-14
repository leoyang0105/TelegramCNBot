using CNBot.Core.Entities.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNBot.Core.Services
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
        public async Task<UserCommand> FindLastCommand(long userId)
        {
            return await _userCommandRepository.TableNoTracking.LastOrDefaultAsync(c => c.UserId == userId);
        }
        public async Task UpdateCommand(UserCommand command)
        {
            await _userCommandRepository.UpdateAsync(command);
        }
        public async Task AddCommand(UserCommand command)
        {
            await _userCommandRepository.AddAsync(command);
        }
    }
}
