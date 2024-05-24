using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Application.ViewModels.User;
using TaskManager.Infrastructure.Persistence.Contexts;
using TaskManager.Infrastructure.Persistence.Repositories;
using User = TaskManager.Core.Domain.Entities.User;


namespace TaskManager.Test.Repositories
{
    public class UserRepositoryTest
    {
        private async Task<ApplicationContext> GetApplicationContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "TestUser")

            }, "TestAuthentication"));

            var httpContextAccessor = new HttpContextAccessor
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            var applicationContext = new ApplicationContext(options, httpContextAccessor);
            applicationContext.Database.EnsureCreated();
            if (await applicationContext.Users.CountAsync() <= 0)
            {
                await applicationContext.Users.AddRangeAsync(TestDataGenerator.GenerateUserList(5));
                await applicationContext.SaveChangesAsync();
            }

            return applicationContext;
        }


        [Fact]
        public async Task GetByIdAsync_WithValidId_ReturnUser()
        {
            // Arrange
            var dbContext = await GetApplicationContext();
            var repository = new UserRepository(dbContext);
            var userFromContext = dbContext.Users.FirstOrDefault()!;

            // Act
            var userFromRepo = await repository.GetByIdAsync(userFromContext.Id);

            // Assert
            Assert.NotNull(userFromRepo);
            Assert.Equal(userFromContext.Id, userFromRepo.Id);
        }

        [Fact]
        public async Task DeleteAsync_WithValidUser_NoUserInDb()
        {
            // Arrange
            var dbContext = await GetApplicationContext();
            var repository = new UserRepository(dbContext);
            var userToDelete = dbContext.Users.FirstOrDefault()!;

            // Act
            await repository.DeleteAsync(userToDelete);
            var deletedUser = dbContext.Users.FirstOrDefault(u => u.Id == userToDelete.Id);

            // Assert
            Assert.Null(deletedUser);
        }

        [Fact]
        public async Task LoginAsync_WithValidCredentials_ReturnUser()
        {
            // Arrange
            var dbContext = await GetApplicationContext();
            var repository = new UserRepository(dbContext);
            var usernameForTest = dbContext.Users.FirstOrDefault()!.UserName;
            var userLogin = new LoginViewModel() { UserName = usernameForTest, Password = "test"};

            // Act
            var userFromRepo = await repository.LoginAsync(userLogin);

            // Assert
            Assert.NotNull(userFromRepo);
            Assert.Equal(userLogin.UserName, userFromRepo.UserName);
        }

        [Fact]
        public async Task RegisterAsync_WithValidUser_ReturnRegisteredUser()
        {
            // Arrange
            var dbContext = await GetApplicationContext();
            var repository = new UserRepository(dbContext);
            var userRegister = TestDataGenerator.GenerateUser();

            // Act
            var registeredUser = await repository.RegisterAsync(userRegister);

            // Assert
            Assert.NotNull(registeredUser);
            Assert.Equal(userRegister.UserName, registeredUser.UserName);
        }

        [Fact]
        public async Task UpdateAsync_WithValidUser_ReturnUpdatedUser()
        {
            // Arrange
            var dbContext = await GetApplicationContext();
            var repository = new UserRepository(dbContext);
            var userToUpdate = dbContext.Users.FirstOrDefault()!;
            var newUsernName = "UpdatedUserName";
            userToUpdate.UserName = newUsernName;

            // Act
            await repository.UpdateAsync(userToUpdate);
            var updatedUser = dbContext.Users.FirstOrDefault(u => u.Id == userToUpdate.Id);

            // Assert
            Assert.NotNull(updatedUser);
            Assert.Equal(newUsernName, updatedUser.UserName);
        }

        [Fact]
        public async Task UserNameExistsAsync_WithExistingUserName_ReturnTrue()
        {
            // Arrange
            var dbContext = await GetApplicationContext();
            var repository = new UserRepository(dbContext);
            var existingUserName = dbContext.Users.FirstOrDefault()!.UserName;

            // Act
            var userNameExists = await repository.UserNameExistsAsync(existingUserName);

            // Assert
            Assert.True(userNameExists);
        }

        [Fact]
        public async Task UserNameExistsAsync_WithNotExistingUserName_ReturnTrue()
        {
            // Arrange
            var dbContext = await GetApplicationContext();
            var repository = new UserRepository(dbContext);

            // Act
            var userNameNotExists = !await repository.UserNameExistsAsync("userNameFalse");

            // Assert
            Assert.True(userNameNotExists);
        }

    }
}
