using ApiServer.Common;
using Carter;
using FluentValidation;
using MassTransit;
using Messages.Institute;
using Messages.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
builder.Services.AddAuthorization();
//builder.Services.AddAuthorization(options =>
//{
//    options.FallbackPolicy = new AuthorizationPolicyBuilder()
//      .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
//      .RequireAuthenticatedUser()
//      .Build();
//});


builder.Services.AddCarter();
builder.Services.AddValidatorsFromAssemblyContaining<AddInstituteValidator>();

builder.Services.AddMassTransit(rmq =>
{
    rmq.UsingRabbitMq();
    //
    rmq.AddRequestClient<AddInstitute>();
    rmq.AddRequestClient<UpdateInstitute>();
    rmq.AddRequestClient<GetInstitute>();
    rmq.AddRequestClient<GetInstituteList>();
    rmq.AddRequestClient<DeleteInstitute>();
    //

    rmq.AddRequestClient<UserLogin>();
    rmq.AddRequestClient<AddUser>();
    rmq.AddRequestClient<UpdateUser>();
    rmq.AddRequestClient<GetUser>();
    rmq.AddRequestClient<GetUserList>();
    rmq.AddRequestClient<DeleteUser>();
});


var app = builder.Build();
ServiceFactory.ServiceProvider = app.Services;

app.UseSwagger();
app.UseAuthorization();
app.UseAuthentication();
app.MapCarter();

//app.UseHttpsRedirection();

app.UseSwaggerUI();
app.Run();
