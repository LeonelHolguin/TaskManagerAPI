using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Core.Application.ViewModels.Task
{
    public class TaskViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int SerialNumber { get; set; }
        public string? Description { get; set; }
        public int UserId { get; set; }
    }
}
