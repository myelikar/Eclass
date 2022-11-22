using User.Module.Data.Repositories;
using User.Module.Domain;
using AutoMapper;
using MassTransit;
using Messages.Common;
using Messages.User;
using System.Runtime.ConstrainedExecution;

namespace User.Module.Features.Users
{
    public class GetUserConsumer : IConsumer<GetUser>
    {
        public IMapper Mapper { get; }
        public IUserRepo UserRepo { get; }
        public GetUserConsumer(IMapper mapper, IUserRepo userRepo)
        {
            Mapper = mapper;
            UserRepo = userRepo;
        }
        public async Task Consume(ConsumeContext<GetUser> context)
        {
            GetUser request = context.Message;
            //Get Data from Db
            CUser User = await UserRepo.GetById(request.Id);
            //Map to UserDTO
            UserDTO UserDTO = Mapper.Map<UserDTO>(User);
            await context.RespondAsync(UserDTO);
        }
    }
}
