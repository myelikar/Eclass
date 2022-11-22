using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.User
{
    public record UpdateUser
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty!;
        public string MiddleName { get; set; } = string.Empty!;
        public string LastName { get; set; } = string.Empty!;
        public string MobileNo { get; set; } = string.Empty!;
        public string EmailId { get; set; } = string.Empty!;
        public string UserName { get; set; } = string.Empty!;
        public string Password { get; set; } = string.Empty!;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; } = string.Empty!;
        public DateTime? ModifiedOn { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; } = string.Empty!;
    }
}
