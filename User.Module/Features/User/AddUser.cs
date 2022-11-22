using User.Module.Data.Repositories;
using User.Module.Domain;
using AutoMapper;
using MassTransit;
using MassTransit.Transports;
using Messages.Common;
using Messages.User;

namespace User.Module.Features.Users
{
    public class AddUserConsumer : IConsumer<AddUser>
    {
        public AddUserConsumer(IMapper mapper, IUserRepo userRepo)
        {
            Mapper = mapper;
            UserRepo = userRepo;
        }

        public IMapper Mapper { get; }
        public IUserRepo UserRepo { get; }

        public async Task Consume(ConsumeContext<AddUser> context)
        {
            CUser User = Mapper.Map<CUser>(context.Message);
            await UserRepo.Insert(User);
            await context.RespondAsync(new Result() { Id = User.Id, Succeeded = true });
        }
    }
}
