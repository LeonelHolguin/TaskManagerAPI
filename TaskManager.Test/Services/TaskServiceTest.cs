using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Application.Interfaces.Repositories;
using TaskManager.Core.Application.Services;
using entityTask = TaskManager.Core.Domain.Entities.Task;

namespace TaskManager.Test.Services
{
    public class TaskServiceTest
    {
        private readonly Mock<ITaskRepository> _taskRepository;
        private readonly TaskService _taskService;

        public TaskServiceTest() 
        { 
            _taskRepository = new Mock<ITaskRepository>();

            _taskService = new TaskService(_taskRepository.Object);
        }

        [Fact]
        public async Task GetAllByUserId_WithValidUserId_ReturnsTaskViewModelList()
        {
            // Arrange
            var userId = 1;
            var taskList = TestDataGenerator.GenerateTaskList(3);
            _taskRepository.Setup(repo => repo.GetAllByUserIdAsync(userId)).ReturnsAsync(taskList);

            // Act
            var result = await _taskService.GetAllByUserId(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(taskList.Count, result.Count);
        }

        [Fact]
        public async Task GetById_WithValidId_ReturnsTaskViewModel()
        {
            // Arrange
            var task = TestDataGenerator.GenerateTask();
            _taskRepository.Setup(repo => repo.GetByIdAsync(task.Id)).ReturnsAsync(task);

            // Act
            var result = await _taskService.GetById(task.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(task.Id, result.Id);
        }

        [Fact]
        public async Task Add_WithValidTaskViewModel_ReturnsTaskViewModel()
        {
            // Arrange
            var taskVm = TestDataGenerator.GenerateTaskViewModel();
            var task = TestDataGenerator.GenerateTask();
            task.Id = taskVm.Id;
            _taskRepository.Setup(repo => repo.AddAsync(It.IsAny<entityTask>())).ReturnsAsync(task);

            // Act
            var result = await _taskService.Add(taskVm);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(taskVm.Id, result.Id);
        }

        [Fact]
        public async Task Update_WithValidTaskViewModel_UpdatesTask()
        {
            // Arrange
            var taskVm = TestDataGenerator.GenerateTaskViewModel();
            var task = TestDataGenerator.GenerateTask();
            task.Id = taskVm.Id;
            _taskRepository.Setup(repo => repo.GetByIdAsync(taskVm.Id)).ReturnsAsync(task);
            _taskRepository.Setup(repo => repo.UpdateAsync(It.IsAny<entityTask>())).Returns(Task.CompletedTask);

            // Act
            await _taskService.Update(taskVm);

            // Assert
            _taskRepository.Verify(repo => repo.UpdateAsync(It.Is<entityTask>(t => t.Id == taskVm.Id)), Times.Once);
        }

        [Fact]
        public async Task Delete_WithValidId_DeletesTask()
        {
            // Arrange
            var task = TestDataGenerator.GenerateTask();
            _taskRepository.Setup(repo => repo.GetByIdAsync(task.Id)).ReturnsAsync(task);
            _taskRepository.Setup(repo => repo.DeleteAsync(It.IsAny<entityTask>())).Returns(Task.CompletedTask);

            // Act
            await _taskService.Delete(task.Id);

            // Assert
            _taskRepository.Verify(repo => repo.DeleteAsync(It.Is<entityTask>(t => t.Id == task.Id)), Times.Once);
        }
    }
}
