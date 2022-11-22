namespace User.Module.Domain.Common
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
    }

    public abstract class BaseAuditableEntity : BaseEntity
    {
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; } = string.Empty!;
        public DateTime? ModifiedOn { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; } = string.Empty!;

    }

    public class BaseMasterEntity:BaseAuditableEntity
    {
        public string Name { get; set; } = string.Empty!;
        public string ShortName { get; set; } = string.Empty!;
        public string Description { get; set; } = string.Empty!;
        public string RID { get; set; } = string.Empty!;
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }

}
