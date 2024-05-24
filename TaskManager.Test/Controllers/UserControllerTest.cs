using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Application.Interfaces.Services;
using TaskManager.Core.Application.ViewModels.User;
using WebAPI.TaskManager.Controllers;

namespace TaskManager.Test.Controllers
{
    public class UserControllerTest
    {
        private readonly Mock<IUserService> _userService;
        private readonly UserController _userController;

        public UserControllerTest()
        {
            _userService = new Mock<IUserService>();

            _userController = new UserController(_userService.Object);
        }


        [Fact]
        public async Task Register_WithValidUserViewModel_ReturnsCreatedResult()
        {
            // Arrange
            var userVm = TestDataGenerator.GenerateUserViewModel();
            _userService.Setup(service => service.UserNameExists(userVm.UserName!)).ReturnsAsync(false);
            _userService.Setup(service => service.Register(userVm)).ReturnsAsync(userVm);

            // Act
            var result = await _userController.Register(userVm);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            var returnValue = Assert.IsType<UserViewModel>(createdResult.Value);
            Assert.Equal(userVm.UserName, returnValue.UserName);
        }

        [Fact]
        public async Task Login_WithValidCredentials_ReturnsOkResult()
        {
            // Arrange
            var loginVm = TestDataGenerator.GenerateLoginViewModel();
            var loginSuccessVm = TestDataGenerator.GenerateLoginSuccessViewModel();
            _userService.Setup(service => service.Login(loginVm)).ReturnsAsync(loginSuccessVm);

            // Act
            var result = await _userController.Login(loginVm);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<LoginSuccessViewModel>(okResult.Value);
            Assert.Equal(loginSuccessVm.User!.UserName, returnValue.User!.UserName);
        }

        [Fact]
        public async Task Update_WithValidUserViewModel_ReturnsNoContentResult()
        {
            // Arrange
            var userVm = TestDataGenerator.GenerateUserViewModel();
            _userService.Setup(service => service.Update(userVm)).Returns(Task.CompletedTask);

            // Act
            var result = await _userController.Update(userVm);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_WithValidId_ReturnsNoContentResult()
        {
            // Arrange
            var userId = 1;
            _userService.Setup(service => service.Delete(userId)).Returns(Task.CompletedTask);

            // Act
            var result = await _userController.Delete(userId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
