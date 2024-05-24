using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Application.Interfaces.Authentication;
using TaskManager.Core.Application.Interfaces.Repositories;
using TaskManager.Core.Application.Services;
using User = TaskManager.Core.Domain.Entities.User;

namespace TaskManager.Test.Services
{
    public class UserServiceTest
    {
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<ITokenService> _tokenService;
        private readonly UserService _userService;

        public UserServiceTest()
        {
            _userRepository = new Mock<IUserRepository>();
            _tokenService = new Mock<ITokenService>();

            _userService = new UserService(_userRepository.Object, _tokenService.Object);
        }


        [Fact]
        public async Task Update_WithValidUserViewModel_UpdatesUser()
        {
            // Arrange
            var userVm = TestDataGenerator.GenerateUserViewModel();
            var user = TestDataGenerator.GenerateUser();
            user.Id = userVm.Id;
            _userRepository.Setup(repo => repo.GetByIdAsync(userVm.Id)).ReturnsAsync(user);
            _userRepository.Setup(repo => repo.UpdateAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            // Act
            await _userService.Update(userVm);

            // Assert
            _userRepository.Verify(repo => repo.UpdateAsync(It.Is<User>(u => u.Name == userVm.Name)), Times.Once);
        }

        [Fact]
        public async Task Delete_WithValidId_DeletesUser()
        {
            // Arrange
            var user = TestDataGenerator.GenerateUser();
            _userRepository.Setup(repo => repo.GetByIdAsync(user.Id)).ReturnsAsync(user);
            _userRepository.Setup(repo => repo.DeleteAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            // Act
            await _userService.Delete(user.Id);

            // Assert
            _userRepository.Verify(repo => repo.DeleteAsync(It.Is<User>(u => u.Id == user.Id)), Times.Once);
        }

        [Fact]
        public async Task Login_WithValidCredentials_ReturnsLoginSuccessViewModel()
        {
            // Arrange
            var loginVm = TestDataGenerator.GenerateLoginViewModel();
            var user = TestDataGenerator.GenerateUser();
            user.UserName = loginVm.UserName;
            var token = "testToken";
            _userRepository.Setup(repo => repo.LoginAsync(loginVm)).ReturnsAsync(user);
            _tokenService.Setup(service => service.GenerateToken(user)).Returns(token);

            // Act
            var loginUser = await _userService.Login(loginVm);

            // Assert
            Assert.NotNull(loginUser);
            Assert.Equal(token, loginUser.AccessToken);
            Assert.Equal(user.UserName, loginUser.User!.UserName);
        }

        [Fact]
        public async Task Register_WithValidUserViewModel_ReturnsUserViewModel()
        {
            // Arrange
            var userVm = TestDataGenerator.GenerateUserViewModel();
            var user = TestDataGenerator.GenerateUser();
            user.UserName = userVm.UserName!;
            _userRepository.Setup(repo => repo.RegisterAsync(It.IsAny<User>())).ReturnsAsync(user);

            // Act
            var result = await _userService.Register(userVm);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.UserName, result.UserName);
        }

        [Fact]
        public async Task UserNameExists_WithExistingUserName_ReturnsTrue()
        {
            // Arrange
            var user = TestDataGenerator.GenerateUser();
            _userRepository.Setup(repo => repo.UserNameExistsAsync(user.UserName)).ReturnsAsync(true);

            // Act
            var result = await _userService.UserNameExists(user.UserName);

            // Assert
            Assert.True(result);
        }
    }
}
