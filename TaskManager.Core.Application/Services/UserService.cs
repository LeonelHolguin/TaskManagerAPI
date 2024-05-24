using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Application.Interfaces.Authentication;
using TaskManager.Core.Application.Interfaces.Repositories;
using TaskManager.Core.Application.Interfaces.Services;
using TaskManager.Core.Application.ViewModels.User;
using User = TaskManager.Core.Domain.Entities.User;


namespace TaskManager.Core.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public UserService(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }


        public async Task Update(UserViewModel userVm)
        {
            var user = await _userRepository.GetByIdAsync(userVm.Id);

            user!.Name = userVm.Name!;
            user.LastName = userVm.LastName!;
            user.Email = userVm.Email!;
            user.UserName = userVm.UserName!;
            user.Password = userVm.Password!;

            await _userRepository.UpdateAsync(user);
        }

        public async Task Delete(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            await _userRepository.DeleteAsync(user!);
        }

        public async Task<LoginSuccessViewModel> Login(LoginViewModel userLogin)
        {
            var user = await _userRepository.LoginAsync(userLogin);

            if(user is null)
            {
                return new LoginSuccessViewModel();
            }

            var accessTokenGenereted = _tokenService.GenerateToken(user);

            UserAuthenticatedViewModel userAuthenticated = new()
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName
            };

            LoginSuccessViewModel loginSuccess = new()
            {
                User = userAuthenticated,
                AccessToken = accessTokenGenereted
            };

            return loginSuccess;
        }

        public async Task<UserViewModel> Register(UserViewModel userRegister)
        {
            User user = new()
            {
                Name = userRegister.Name!,
                LastName = userRegister.LastName!,
                Email = userRegister.Email!,
                UserName = userRegister.UserName!,
                Password = userRegister.Password!,
                CreatedBy = ""
            };

            user = await _userRepository.RegisterAsync(user);

            UserViewModel userVm = new()
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName,
            };

            return userVm;
        }

        public async Task<bool> UserNameExists(string userName)
        {
            return await _userRepository.UserNameExistsAsync(userName);
        }
    }
}
