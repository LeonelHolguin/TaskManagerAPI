using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Application.Helpers;
using TaskManager.Core.Application.Interfaces.Repositories;
using TaskManager.Core.Application.ViewModels.User;
using TaskManager.Infrastructure.Persistence.Contexts;
using User = TaskManager.Core.Domain.Entities.User;


namespace TaskManager.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _dbContext;

        public UserRepository(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<User?> GetByIdAsync(int id)
        {
            return await _dbContext.Set<User>()
                .AsNoTracking()
                .FirstOrDefaultAsync(user => user.Id == id);
        }

        public async Task DeleteAsync(User user)
        {
            _dbContext.Entry(user).State = EntityState.Detached;
            _dbContext.Set<User>().Remove(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<User?> LoginAsync(LoginViewModel userLogin)
        {
            string passwordEncrypt = PasswordEncryption.ComputeShad256Hash(userLogin.Password);

            User? user = await _dbContext.Set<User>()
                .FirstOrDefaultAsync(user => user.UserName == userLogin.UserName && user.Password == passwordEncrypt);

            return user;
        }

        public async Task<User> RegisterAsync(User userRegister)
        {
            userRegister.Password = PasswordEncryption.ComputeShad256Hash(userRegister.Password);

            await _dbContext.Set<User>().AddAsync(userRegister);
            await _dbContext.SaveChangesAsync();
            return userRegister;
        }

        public async Task UpdateAsync(User user)
        {
            user.Password = PasswordEncryption.ComputeShad256Hash(user.Password);

            _dbContext.Entry(user).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> UserNameExistsAsync(string userName)
        {
            var user = await _dbContext.Set<User>().FirstOrDefaultAsync(user => user.UserName == userName);
            return user is not null;
        }
    }
}
