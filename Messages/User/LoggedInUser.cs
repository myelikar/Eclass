using Messages.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.User
{
    public  record LoggedInUser : BaseAuditableDTO
    {
        public string FirstName { get; set; } = string.Empty!;
        public string MiddleName { get; set; } = string.Empty!;
        public string LastName { get; set; } = string.Empty!;
        public string UserName { get; set; } = string.Empty!;
        public string EmailId { get; set; } = string.Empty!;
        public string Token { get; set; } = "";
    }
}
