
namespace TaskManager.Core.Domain.Common
{
    public class AuditableBaseEntity
    {
        public virtual int Id { get; set; }
        public required string CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
