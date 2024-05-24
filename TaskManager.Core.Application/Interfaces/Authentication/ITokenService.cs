using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Domain.Entities;

namespace TaskManager.Core.Application.Interfaces.Authentication
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
