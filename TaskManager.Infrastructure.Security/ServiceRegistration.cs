using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Application.Interfaces.Authentication;
using TaskManager.Core.Application.Interfaces.Services;
using TaskManager.Core.Application.Services;
using TaskManager.Infrastructure.Security.Authentication;

namespace TaskManager.Infrastructure.Security
{
    public static class ServiceRegistration
    {
        public static void AddSecutiryLayer(this IServiceCollection services, IConfiguration config)
        {
            #region authentication
            services.AddTransient<ITokenService>(provider => new TokenService(config));
            #endregion
        }
    }
}
