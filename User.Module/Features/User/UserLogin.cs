using AutoMapper;
using MassTransit;
using Messages.User;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using User.Module.Data.Repositories;
using User.Module.Domain;

namespace User.Module.Features.Users
{
    public class UserLoginConsumer : IConsumer<UserLogin>
    {
        private string tokenKey = "Millicent_EClass_2.0";
        public UserLoginConsumer(IMapper mapper, IUserRepo userRepo)
        {
            Mapper = mapper;
            UserRepo = userRepo;
        }

        public IMapper Mapper { get; }
        public IUserRepo UserRepo { get; }

        public async Task Consume(ConsumeContext<UserLogin> context)
        {
            CUser user = await UserRepo.ValidLogin(context.Message);
            if (user == null)
                throw new Exception("Invalid Login");

            LoggedInUser loggedInUser = Mapper.Map<LoggedInUser>(user);
            await context.RespondAsync(loggedInUser);
        }
    }
}
