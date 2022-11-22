using User.Module.Data.Repositories;
using User.Module.Domain;
using AutoMapper;
using MassTransit;
using Messages.Common;
using Messages.User;

namespace User.Module.Features.Users
{
    public class UpdateUserConsumer : IConsumer<UpdateUser>
    {
        public UpdateUserConsumer(IMapper mapper, IUserRepo userRepo)
        {
            Mapper = mapper;
            UserRepo = userRepo;
        }

        public IMapper Mapper { get; }
        public IUserRepo UserRepo { get; }

        public async Task Consume(ConsumeContext<UpdateUser> context)
        {
            CUser User = Mapper.Map<CUser>(context.Message);
            await UserRepo.Update(User);
            await context.RespondAsync(new Result() { Id = User.Id, Succeeded = true });
        }
    }
}
