using Admin.Module.Domain;
using AutoMapper;
using Messages.Institute;

namespace Admin.Module.Features.Institutes
{
    public class InstituteMappings : Profile
    {
        public InstituteMappings()
        {
            CreateMap<AddInstitute, Institute>();
            CreateMap<UpdateInstitute, Institute>();
            CreateMap<Institute, InstituteDTO>();
        }
    }
}
