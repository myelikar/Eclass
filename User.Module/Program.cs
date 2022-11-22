using MassTransit;
using Microsoft.EntityFrameworkCore;
using User.Module.Data;
using User.Module.Data.Repositories;
using User.Module.Domain.Common;
using User.Module.Features.Users;

string BaseDirPath = Directory.GetCurrentDirectory();
//Set Environment Variable of BASEDIR for Serilog Files
Environment.SetEnvironmentVariable("BASEDIR", BaseDirPath);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UserDbContext>(options =>
              options.UseSqlite($"Filename={BaseDirPath}\\..\\Data\\DemoDb.db",
              b => b.MigrationsAssembly(typeof(UserDbContext).Assembly.FullName))
              .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
              , ServiceLifetime.Singleton);

builder.Services.AddScoped<IUserRepo, UserRepo>();

builder.Services.AddAutoMapper(typeof(BaseEntity));
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<UserLoginConsumer>();
    x.AddConsumer<AddUserConsumer>();
    x.AddConsumer<GetUserListConsumer>();
    x.AddConsumer<UpdateUserConsumer>();
    x.AddConsumer<GetUserConsumer>();
    x.AddConsumer<DeleteUserConsumer>();
    // automatically create endpoints for any registered consumers
    x.SetKebabCaseEndpointNameFormatter();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);

    });
});

var app = builder.Build();
app.Services.GetRequiredService<UserDbContext>().Database.EnsureCreated();
app.Run();
