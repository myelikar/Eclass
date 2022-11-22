using User.Module.Domain.Common;

namespace User.Module.Domain
{
    public class CUser : BaseAuditableEntity
    {
        public string FirstName { get; set; } = string.Empty!;
        public string MiddleName { get; set; } = string.Empty!;
        public string LastName { get; set; } = string.Empty!;
        public string MobileNo { get; set; } = string.Empty!;
        public string EmailId { get; set; } = string.Empty!;
        public string UserName { get; set; } = string.Empty!;
        public string Password { get; set; } = string.Empty!;
    }
}
