using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Application.Interfaces.Services;
using TaskManager.Core.Application.ViewModels.Task;
using WebAPI.TaskManager.Controllers;

namespace TaskManager.Test.Controllers
{
    public class TaskControllerTest
    {
        private readonly Mock<ITaskService> _taskService;
        private readonly TaskController _taskController;

        public TaskControllerTest()
        {
            _taskService = new Mock<ITaskService>();

            _taskController = new TaskController(_taskService.Object);
        }


        [Fact]
        public async Task GetAllByUser_WithValidUserId_ReturnsOkResult()
        {
            // Arrange
            var userId = 1;
            var taskListVm = TestDataGenerator.GenerateTaskViewModelList(3);
            _taskService.Setup(service => service.GetAllByUserId(userId)).ReturnsAsync(taskListVm);

            // Act
            var result = await _taskController.GetAllByUser(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<TaskViewModel>>(okResult.Value);
            Assert.Equal(taskListVm.Count, returnValue.Count);
        }

        [Fact]
        public async Task GetOneById_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var taskVm = TestDataGenerator.GenerateTaskViewModel();
            _taskService.Setup(service => service.GetById(taskVm.Id)).ReturnsAsync(taskVm);

            // Act
            var result = await _taskController.GetOneById(taskVm.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<TaskViewModel>(okResult.Value);
            Assert.Equal(taskVm.Id, returnValue.Id);
        }

        [Fact]
        public async Task CreateNewTask_WithValidTaskViewModel_ReturnsCreatedResult()
        {
            // Arrange
            var taskVm = TestDataGenerator.GenerateTaskViewModel();
            _taskService.Setup(service => service.Add(taskVm)).ReturnsAsync(taskVm);

            // Act
            var result = await _taskController.CreateNewTask(taskVm);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            var returnValue = Assert.IsType<TaskViewModel>(createdResult.Value);
            Assert.Equal(taskVm.Id, returnValue.Id);
        }

        [Fact]
        public async Task Update_WithValidTaskViewModel_ReturnsNoContentResult()
        {
            // Arrange
            var taskVm = TestDataGenerator.GenerateTaskViewModel();
            _taskService.Setup(service => service.Update(taskVm)).Returns(Task.CompletedTask);

            // Act
            var result = await _taskController.Update(taskVm);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_WithValidId_ReturnsNoContentResult()
        {
            // Arrange
            var taskId = 1;
            _taskService.Setup(service => service.Delete(taskId)).Returns(Task.CompletedTask);

            // Act
            var result = await _taskController.Delete(taskId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
