using ApiServer.Common;
using Carter;
using MassTransit;
using MassTransit.Mediator;
using Messages.Common;
using Messages.Institute;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static MassTransit.MessageHeaders;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Data;

namespace ApiServer.Modules
{
    public class ConfigModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/institute",GetInstitutes);
            app.MapGet("/api/institute/{id}", GetInstitute);
            app.MapPost("/api/institute", CreateInstitute);
            app.MapPut("/api/institute/", UpdateInstitute);
            app.MapDelete("/api/institute/{id}", DeleteInstitute);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
        private async Task<IResult> GetInstitute(int id, IRequestClient<GetInstitute> requestClient)
        {
            try
            {

                GetInstitute getInstitute = new GetInstitute() { Id = id };
                var resp = await requestClient.GetResponse<InstituteDTO>(requestClient);
                if (resp.Message == null)
                    Results.NotFound();
                return Results.Ok(resp.Message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
        private async Task<PaginatedList<InstituteDTO>> GetInstitutes(int pageSize, int pageNo, IRequestClient<GetInstituteList> requestClient)
        {
            try
            {
                GetInstituteList getInstitutes = new GetInstituteList() { pageSize = pageSize, pageNo = pageNo };

                var resp = await requestClient.GetResponse<PaginatedList<InstituteDTO>>(getInstitutes);
                return resp.Message;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
        private async Task<IResult> CreateInstitute(AddInstitute adduser, IRequestClient<AddInstitute> requestClient, AddInstituteValidator validator)
        {
            try
            {
                FluentValidation.Results.ValidationResult ValidRes = validator.Validate(adduser);
                if (!ValidRes.IsValid)
                    return Results.Problem(new ProblemDetails()
                    {
                        Detail = ValidRes.Errors[0].ErrorMessage,
                        Status = StatusCodes.Status417ExpectationFailed,
                    });

                //IRequestClient<AddInstitute> requestClient = ServiceFactory.GetService<IRequestClient<AddInstitute>>();
                var result = await requestClient.GetResponse<Result>(adduser);
                if (result.Message.Succeeded)
                    return Results.Ok(result.Message);
                else return Results.Problem(new ProblemDetails()
                {
                    Detail = "CPR001: Something went wrong! Please try again",
                    Status = StatusCodes.Status500InternalServerError,
                });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
                throw;
            }
        }
        
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
        private async Task<IResult> DeleteInstitute(int id, IRequestClient<DeleteInstitute> requestClient)
        {
            try
            {

                DeleteInstitute deleteInstitute = new DeleteInstitute() { Id = id };
                var resp = await requestClient.GetResponse<Result>(requestClient);
                if (resp.Message.Succeeded)
                    return Results.Ok(resp.Message);
                else return Results.Problem(new ProblemDetails()
                {
                    Detail = "CPR001: Something went wrong! Please try again",
                    Status = StatusCodes.Status500InternalServerError,
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
        private async Task<IResult> UpdateInstitute([FromBody] UpdateInstitute edituser, IRequestClient<UpdateInstitute> requestClient)
        {
            try
            {
                var result = await requestClient.GetResponse<Result>(edituser);
                if (result.Message.Succeeded)
                    return Results.Ok(result.Message);
                else return Results.Problem(new ProblemDetails()
                {
                    Detail = "CPR001: Something went wrong! Please try again",
                    Status = StatusCodes.Status500InternalServerError,
                });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
                throw;
            }
        }
    }
}
