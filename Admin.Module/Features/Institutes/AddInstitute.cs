using Admin.Module.Data.Repositories;
using Admin.Module.Domain;
using AutoMapper;
using FluentValidation;
using MassTransit;
using MassTransit.Transports;
using Messages.Common;
using Messages.Institute;

namespace Admin.Module.Features.Institutes
{
    public class AddInstituteConsumer : IConsumer<AddInstitute>
    {
        public AddInstituteConsumer(IMapper mapper, IInstituteRepo instituteRepo)
        {
            Mapper = mapper;
            InstituteRepo = instituteRepo;
        }

        public IMapper Mapper { get; }
        public IInstituteRepo InstituteRepo { get; }

        public async Task Consume(ConsumeContext<AddInstitute> context)
        {
            AddInstitute request = context.Message;
            
            Institute institute = Mapper.Map<Institute>(context.Message);
            await InstituteRepo.Insert(institute);
            await context.RespondAsync(new Result() { Id = institute.Id, Succeeded = true });
        }
    }
}
