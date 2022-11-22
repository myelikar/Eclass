using Admin.Module.Data.Repositories;
using Admin.Module.Domain;
using AutoMapper;
using MassTransit;
using Messages.Common;
using Messages.Institute;

namespace Admin.Module.Features.Institutes
{
    public class UpdateInstituteConsumer : IConsumer<UpdateInstitute>
    {
        public UpdateInstituteConsumer(IMapper mapper, IInstituteRepo instituteRepo)
        {
            Mapper = mapper;
            InstituteRepo = instituteRepo;
        }

        public IMapper Mapper { get; }
        public IInstituteRepo InstituteRepo { get; }

        public async Task Consume(ConsumeContext<UpdateInstitute> context)
        {
            Institute institute = Mapper.Map<Institute>(context.Message);
            await InstituteRepo.Update(institute);
            await context.RespondAsync(new Result() { Id = institute.Id, Succeeded = true });
        }
    }
}
