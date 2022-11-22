using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Messages.Institute
{
    public record AddInstitute
    {
        public string Name { get; set; } = string.Empty!;
        public string ShortName { get; set; } = string.Empty!;
        public string Description { get; set; } = string.Empty!;
        public string RID { get; set; } = string.Empty!;

    }

    public class AddInstituteValidator : AbstractValidator<AddInstitute>
    {
        public AddInstituteValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(p => p.ShortName).MaximumLength(10).WithMessage("Short Name is too long. Maximum length is 10 characters");
        }
    }
}
