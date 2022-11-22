using Admin.Module.Data.Repositories;
using Admin.Module.Domain;
using AutoMapper;
using MassTransit;
using Messages.Common;
using Messages.Institute;
using System.Runtime.ConstrainedExecution;

namespace Admin.Module.Features.Institutes
{
    public class GetInstituteConsumer : IConsumer<GetInstitute>
    {
        public IMapper Mapper { get; }
        public IInstituteRepo InstituteRepo { get; }
        public GetInstituteConsumer(IMapper mapper, IInstituteRepo instituteRepo)
        {
            Mapper = mapper;
            InstituteRepo = instituteRepo;
        }
        public async Task Consume(ConsumeContext<GetInstitute> context)
        {
            GetInstitute request = context.Message;
            //Get Data from Db
            Institute institute = await InstituteRepo.GetById(request.Id);
            //Map to InstituteDTO
            InstituteDTO InstituteDTO = Mapper.Map<InstituteDTO>(institute);
            await context.RespondAsync(InstituteDTO);
        }
    }
}
