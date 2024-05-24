using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Application.Interfaces.Services;
using TaskManager.Core.Application.Services;

namespace TaskManager.Core.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            #region services
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ITaskService, TaskService>();
            #endregion
        }
    }
}
