using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Application.ViewModels.User;
using User = TaskManager.Core.Domain.Entities.User;

namespace TaskManager.Core.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task<User?> LoginAsync(LoginViewModel userLogin);
        Task<User> RegisterAsync(User userRegister);
        Task<bool> UserNameExistsAsync(string userName);
    }
}
