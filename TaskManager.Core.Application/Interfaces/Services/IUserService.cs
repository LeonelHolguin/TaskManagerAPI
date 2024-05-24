using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Application.ViewModels.User;

namespace TaskManager.Core.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task Update(UserViewModel userVm);
        Task Delete(int id);
        Task<LoginSuccessViewModel> Login(LoginViewModel userLogin);
        Task<UserViewModel> Register(UserViewModel userRegister);
        Task<bool> UserNameExists(string userName);
    }
}
