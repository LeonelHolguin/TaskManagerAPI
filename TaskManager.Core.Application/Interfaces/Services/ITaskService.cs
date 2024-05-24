using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Application.ViewModels.Task;

namespace TaskManager.Core.Application.Interfaces.Services
{
    public interface ITaskService
    {
        Task<List<TaskViewModel>> GetAllByUserId(int userId);
        Task<TaskViewModel> GetById(int id);
        Task<TaskViewModel> Add(TaskViewModel taskVm);
        Task Update(TaskViewModel taskVm);
        Task Delete(int id);
    }
}
