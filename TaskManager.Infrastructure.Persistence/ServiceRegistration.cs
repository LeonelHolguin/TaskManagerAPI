using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Core.Application.Interfaces.Repositories;
using TaskManager.Infrastructure.Persistence.Contexts;
using TaskManager.Infrastructure.Persistence.Repositories;

namespace TaskManager.Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            #region contexts
            var connectionString = config.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationContext>(options =>
                                  options.UseOracle(connectionString,
                                    m =>
                                    {
                                        m.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName);
                                    }));
            #endregion

            #region repositories 
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ITaskRepository, TaskRepository>();
            #endregion
        }
    }
}
