using Admin.Module.Data.Repositories;
using Admin.Module.Domain;
using AutoMapper;
using MassTransit;
using Messages.Common;
using Messages.Institute;
using System.Runtime.ConstrainedExecution;

namespace Admin.Module.Features.Institutes
{
    public class GetInstituteListConsumer : IConsumer<GetInstituteList>
    {
        public IMapper Mapper { get; }
        public IInstituteRepo InstituteRepo { get; }
        public GetInstituteListConsumer(IMapper mapper, IInstituteRepo instituteRepo)
        {
            Mapper = mapper;
            InstituteRepo = instituteRepo;
        }
        public async Task Consume(ConsumeContext<GetInstituteList> context)
        {
            GetInstituteList request = context.Message;
            //Calculate offset number to skip in reading db records
            int offset = (request.pageNo - 1) * request.pageSize;
            //Get Data from Db
            IEnumerable<Institute> list = await InstituteRepo.GetAll(offset, request.pageSize);
            //Map to InstituteDTO
            List<InstituteDTO> InstituteDTOList = Mapper.Map<List<InstituteDTO>>(list);
            //Get Count
            int total = await InstituteRepo.Count();

            await context.RespondAsync(new PaginatedList<InstituteDTO>(InstituteDTOList, total, request.pageNo, request.pageSize));
        }
    }
}
