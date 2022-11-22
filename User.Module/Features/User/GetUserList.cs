using User.Module.Data.Repositories;
using User.Module.Domain;
using AutoMapper;
using MassTransit;
using Messages.Common;
using Messages.User;
using System.Runtime.ConstrainedExecution;

namespace User.Module.Features.Users
{
    public class GetUserListConsumer : IConsumer<GetUserList>
    {
        public IMapper Mapper { get; }
        public IUserRepo UserRepo { get; }
        public GetUserListConsumer(IMapper mapper, IUserRepo userRepo)
        {
            Mapper = mapper;
            UserRepo = userRepo;
        }
        public async Task Consume(ConsumeContext<GetUserList> context)
        {
            GetUserList request = context.Message;
            //Calculate offset number to skip in reading db records
            int offset = (request.pageNo - 1) * request.pageSize;
            //Get Data from Db
            IEnumerable<CUser> list = await UserRepo.GetAll(offset, request.pageSize);
            //Map to UserDTO
            List<UserDTO> UserDTOList = Mapper.Map<List<UserDTO>>(list);
            //Get Count
            int total = await UserRepo.Count();

            await context.RespondAsync(new PaginatedList<UserDTO>(UserDTOList, total, request.pageNo, request.pageSize));
        }
    }
}
