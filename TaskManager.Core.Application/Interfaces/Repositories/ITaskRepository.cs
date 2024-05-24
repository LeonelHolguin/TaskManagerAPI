using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using entityTask = TaskManager.Core.Domain.Entities.Task;

namespace TaskManager.Core.Application.Interfaces.Repositories
{
    public interface ITaskRepository
    {
        Task<List<entityTask>?> GetAllByUserIdAsync(int userId);
        Task<entityTask?> GetByIdAsync(int id);
        Task<entityTask> AddAsync(entityTask task);
        Task UpdateAsync (entityTask task);
        Task DeleteAsync (entityTask task);
    }
}
