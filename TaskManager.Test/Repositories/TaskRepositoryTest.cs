using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Infrastructure.Persistence.Contexts;
using TaskManager.Infrastructure.Persistence.Repositories;


namespace TaskManager.Test.Repositories
{
    public class TaskRepositoryTest
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
            if (await applicationContext.Tasks.CountAsync() <= 0)
            {
                await applicationContext.Tasks.AddRangeAsync(TestDataGenerator.GenerateTaskList(10));
                await applicationContext.SaveChangesAsync();
            }

            return applicationContext;
        }


        [Fact]
        public async Task GetAllByUserIdAsync_WithValidUserId_ReturnTaskList()
        {
            // Arrange
            var dbContext = await GetApplicationContext();
            var repository = new TaskRepository(dbContext);
            var userId = dbContext.Tasks.FirstOrDefault()!.UserId;

            // Act
            var TaskList = await repository.GetAllByUserIdAsync(userId);

            // Assert
            Assert.NotNull(TaskList);
            Assert.True(TaskList.Count > 0);
            Assert.IsType<List<Core.Domain.Entities.Task>>(TaskList);
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ReturnTask()
        {
            // Arrange
            var dbContext = await GetApplicationContext();
            var repository = new TaskRepository(dbContext);
            var taskFromContext = dbContext.Tasks.FirstOrDefault()!;

            // Act
            var taskFromRepo = await repository.GetByIdAsync(taskFromContext.Id);

            // Assert
            Assert.NotNull(taskFromRepo);
            Assert.Equal(taskFromContext.Id, taskFromRepo.Id);
            Assert.Equal(taskFromContext.SerialNumber, taskFromRepo.SerialNumber);
        }

        [Fact]
        public async Task AddAsync_WithValidData_ReturnAddedTask()
        {
            // Arrange
            var dbContext = await GetApplicationContext();
            var repository = new TaskRepository(dbContext);
            var taskToAdd = TestDataGenerator.GenerateTask();

            // Act
            var addedTask = await repository.AddAsync(taskToAdd);

            // Assert
            Assert.NotNull(addedTask);
            Assert.Equal(taskToAdd, addedTask);
        }

        [Fact]
        public async Task UpdateAsync_WithValidData_ReturnUpdatedTask()
        {
            // Arrange
            var dbContext = await GetApplicationContext();
            var repository = new TaskRepository(dbContext);
            var taskToUpdate = dbContext.Tasks.FirstOrDefault()!;
            string newName = "Go to Habanna";
            taskToUpdate.Name = newName;

            // Act
            await repository.UpdateAsync(taskToUpdate);
            var updatedTask = dbContext.Tasks.FirstOrDefault(t => t.Id == taskToUpdate.Id);

            // Assert
            Assert.NotNull(updatedTask);
            Assert.Equal(newName, updatedTask.Name);
        }

        [Fact]
        public async Task DeleteAsync_WithValidId_NoTaskInDb()
        {
            // Arrange
            var dbContext = await GetApplicationContext();
            var repository = new TaskRepository(dbContext);
            var taskToDelete = dbContext.Tasks.FirstOrDefault()!;

            // Act
            await repository.DeleteAsync(taskToDelete);
            var deletedTask = dbContext.Tasks.FirstOrDefault(t => t.Id == taskToDelete.Id);

            // Assert
            Assert.Null(deletedTask);
        }
    }
}
