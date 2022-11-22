using User.Module.Data.Repositories;
using User.Module.Domain;
using AutoMapper;
using MassTransit;
using Messages.Common;
using Messages.User;
using System.Runtime.ConstrainedExecution;

namespace User.Module.Features.Users
{
    public class DeleteUserConsumer : IConsumer<DeleteUser>
    {
        public IMapper Mapper { get; }
        public IUserRepo UserRepo { get; }
        public DeleteUserConsumer(IMapper mapper, IUserRepo userRepo)
        {
            Mapper = mapper;
            UserRepo = userRepo;
        }
        public async Task Consume(ConsumeContext<DeleteUser> context)
        {
            DeleteUser request = context.Message;
            //Get Data from Db
            await UserRepo.Delete(request.Id);
            await context.RespondAsync(new Result() { Succeeded = true });
        }
    }
}
