using User.Module.Domain;
using AutoMapper;
using Messages.User;

namespace User.Module.Features.Users
{
    public class UserMappings : Profile
    {
        public UserMappings()
        {
            CreateMap<AddUser, CUser>();
            CreateMap<UpdateUser, CUser>();
            CreateMap<CUser, UserDTO>();
            CreateMap<CUser, LoggedInUser>();
            CreateMap<LoggedInUser, CUser>();

        }
    }
}
