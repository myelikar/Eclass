using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Messages.User
{
    public record AddUser
    {
        public string FirstName { get; set; } = string.Empty!;
        public string MiddleName { get; set; } = string.Empty!;
        public string LastName { get; set; } = string.Empty!;
        public string MobileNo { get; set; } = string.Empty!;
        public string EmailId { get; set; } = string.Empty!;
        public string UserName { get; set; } = string.Empty!;
        public string Password { get; set; } = string.Empty!;

    }


    public class AddClassValidator : AbstractValidator<AddUser>
    {

    }
}
