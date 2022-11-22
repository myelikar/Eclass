using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.Common
{
    public record BaseAuditableDTO
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; } = string.Empty!;
        public DateTime? ModifiedOn { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; } = string.Empty!;
        
    }

    public record BaseMasterDTO:BaseAuditableDTO
    {
        public string Name { get; set; } = string.Empty!;
        public string ShortName { get; set; } = string.Empty!;
        public string Description { get; set; } = string.Empty!;
        public string RID { get; set; } = string.Empty!;
        public bool IsActive { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
    }
}
