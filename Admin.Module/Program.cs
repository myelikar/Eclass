using Admin.Module.Data;
using Admin.Module.Data.Repositories;
using Admin.Module.Data.Repositories.Common;
using Admin.Module.Domain.Common;
using Admin.Module.Features.Institutes;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

string BaseDirPath = Directory.GetCurrentDirectory();
//Set Environment Variable of BASEDIR for Serilog Files
Environment.SetEnvironmentVariable("BASEDIR", BaseDirPath);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AdminDbContext>(options =>
              options.UseSqlite($"Filename={BaseDirPath}\\..\\Data\\DemoDb.db",
              b => b.MigrationsAssembly(typeof(AdminDbContext).Assembly.FullName))
              .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
              , ServiceLifetime.Singleton);

builder.Services.AddScoped<IInstituteRepo, InstituteRepo>();

builder.Services.AddAutoMapper(typeof(BaseEntity));
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<AddInstituteConsumer>();
    x.AddConsumer<GetInstituteListConsumer>();
    x.AddConsumer<UpdateInstituteConsumer>();
    x.AddConsumer<GetInstituteConsumer>();
    x.AddConsumer<DeleteInstituteConsumer>();
    // automatically create endpoints for any registered consumers
    x.SetKebabCaseEndpointNameFormatter();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);

    });
});

var app = builder.Build();
app.Services.GetRequiredService<AdminDbContext>().Database.EnsureCreated();
app.Run();
