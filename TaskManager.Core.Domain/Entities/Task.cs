using TaskManager.Core.Domain.Common;

namespace TaskManager.Core.Domain.Entities
{
    public class Task : AuditableBaseEntity
    {
        public required string Name { get; set; }
        public int SerialNumber { get; set; }
        public required string Description { get; set; }
        public int UserId { get; set; }

        public User? User { get; set; }
    }
}
