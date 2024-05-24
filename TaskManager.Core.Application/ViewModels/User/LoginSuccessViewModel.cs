using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Core.Application.ViewModels.User
{
    public class LoginSuccessViewModel
    {
        public UserAuthenticatedViewModel? User { get; set; }
        public string? AccessToken { get; set; }
    }
}
