using Admin.Module.Data.Repositories;
using Admin.Module.Domain;
using AutoMapper;
using MassTransit;
using Messages.Common;
using Messages.Institute;
using System.Runtime.ConstrainedExecution;

namespace Admin.Module.Features.Institutes
{
    public class DeleteInstituteConsumer : IConsumer<DeleteInstitute>
    {
        public IMapper Mapper { get; }
        public IInstituteRepo InstituteRepo { get; }
        public DeleteInstituteConsumer(IMapper mapper, IInstituteRepo instituteRepo)
        {
            Mapper = mapper;
            InstituteRepo = instituteRepo;
        }
        public async Task Consume(ConsumeContext<DeleteInstitute> context)
        {
            DeleteInstitute request = context.Message;
            //Get Data from Db
            await InstituteRepo.Delete(request.Id);
            await context.RespondAsync(new Result() { Succeeded = true });
        }
    }
}
