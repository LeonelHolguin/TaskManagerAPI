using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TaskManager.Core.Application.Interfaces.Repositories;
using TaskManager.Infrastructure.Persistence.Contexts;
using entityTask = TaskManager.Core.Domain.Entities.Task;


namespace TaskManager.Infrastructure.Persistence.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationContext _dbContext;

        public TaskRepository(ApplicationContext dbContext)
        { 
            _dbContext = dbContext;
        }
        public async Task<List<entityTask>?> GetAllByUserIdAsync(int userId)
        {
            return await _dbContext.Set<entityTask>()
                .Where(et => et.UserId == userId)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<entityTask?> GetByIdAsync(int id)
        {
            return await _dbContext.Set<entityTask>()
                .AsNoTracking()
                .FirstOrDefaultAsync(et => et.Id == id);
        }

        public async Task<entityTask> AddAsync(entityTask task)
        {
            await _dbContext.Set<entityTask>().AddAsync(task);
            await _dbContext.SaveChangesAsync();
            return task;
        }

        public async Task UpdateAsync(entityTask task)
        {
            _dbContext.Entry(task).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(entityTask task)
        {
            _dbContext.Entry(task).State = EntityState.Detached;
            _dbContext.Set<entityTask>().Remove(task);
            await _dbContext.SaveChangesAsync();
        }

    }
}
