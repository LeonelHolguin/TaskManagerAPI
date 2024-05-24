using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Application.Interfaces.Repositories;
using TaskManager.Core.Application.Interfaces.Services;
using TaskManager.Core.Application.ViewModels.Task;
using entityTask = TaskManager.Core.Domain.Entities.Task;

namespace TaskManager.Core.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }


        public async Task<List<TaskViewModel>> GetAllByUserId(int userId)
        {
            var taskList = await _taskRepository.GetAllByUserIdAsync(userId);

            if (taskList is null)
                return new List<TaskViewModel>();

            return taskList.Select(task => new TaskViewModel 
            { 
                Id = task.Id,
                Name = task.Name,
                SerialNumber = task.SerialNumber,
                Description = task.Description,
                UserId = userId

            }).ToList();
        }

        public async Task<TaskViewModel> GetById(int id)
        {
            TaskViewModel taskVm = new();

            var task = await _taskRepository.GetByIdAsync(id);

            if (task is not null)
            {
                taskVm.Id = task.Id;
                taskVm.Name = task.Name;
                taskVm.SerialNumber = task.SerialNumber;
                taskVm.Description = task.Description;
                taskVm.UserId = task.UserId;
            }

            return taskVm;
        }

        public async Task<TaskViewModel> Add(TaskViewModel taskVm)
        {
            entityTask task = new()
            {
                Name = taskVm.Name!,
                SerialNumber = taskVm.SerialNumber,
                Description = taskVm.Description!,
                UserId = taskVm.UserId,
                CreatedBy = ""
            };

            task = await _taskRepository.AddAsync(task);

            TaskViewModel taskCreatedVm = new();

            taskCreatedVm.Id = task.Id;
            taskCreatedVm.Name = task.Name;
            taskCreatedVm.SerialNumber = task.SerialNumber;
            taskCreatedVm.Description = task.Description;
            taskCreatedVm.UserId = task.UserId;

            return taskCreatedVm;
        }

        public async Task Update(TaskViewModel taskVm)
        {
            var task = await _taskRepository.GetByIdAsync(taskVm.Id);

            task!.Name = taskVm.Name!;
            task.SerialNumber = taskVm.SerialNumber;
            task.Description = taskVm.Description!;

            await _taskRepository.UpdateAsync(task);
        }

        public async Task Delete(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            await _taskRepository.DeleteAsync(task!);
        }
    }
}
