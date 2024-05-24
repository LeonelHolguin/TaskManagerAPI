using TaskManager.Core.Domain.Common;

namespace TaskManager.Core.Domain.Entities
{
    public class User : AuditableBaseEntity
    {
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }

        public ICollection<Task>? Tasks { get; set; }
    }
}
