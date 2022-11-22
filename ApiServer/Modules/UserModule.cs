using Carter;
using MassTransit;
using MassTransit.Clients;
using Messages.Common;
using Messages.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiServer.Modules
{
    public class UserModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/user/login", Login);
            app.MapGet("/api/user", GetUsers);
            app.MapGet("/api/user/{id}", GetUser);
            app.MapPost("/api/user", CreateUser);
            app.MapPut("/api/user/", UpdateUser);
            app.MapDelete("/api/user/{id}", DeleteUser);
        }

        /* Token
         eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6Im1heXVyLnllbGlrYXIiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJtYXl1ci55ZWxpa2FyQGdtYWlsLmNvbSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2dpdmVubmFtZSI6Ik1heXVyIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvc3VybmFtZSI6IlllbGlrYXIiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbmlzdHJhdG9yIiwibmJmIjoxNjY5MDU1OTMxLCJleHAiOjE2NzQyMzk5MzEsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjcwMTEvIiwiYXVkIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzAxMS8ifQ.c3xSLpjfHfBZb2GeU86I0_D-F-EMHfEwxC_WB_UWcLc
        */
        async Task<IResult> Login([FromBody] UserLogin user, IConfiguration configuration, IRequestClient<UserLogin> requestClient)
        {
            if (!string.IsNullOrEmpty(user.UserName) &&
                !string.IsNullOrEmpty(user.Password))
            {
                var resp = await requestClient.GetResponse<LoggedInUser>(user);
                LoggedInUser loggedInUser = resp.Message;
                if (loggedInUser is null) return Results.NotFound("User not found");

                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, loggedInUser.UserName),
                    new Claim(ClaimTypes.Email, loggedInUser.EmailId),
                    new Claim(ClaimTypes.GivenName, loggedInUser.FirstName),
                    new Claim(ClaimTypes.Surname, loggedInUser.LastName),
                    new Claim(ClaimTypes.Role, "Administrator") //Fixed Roles for now
                };

                var token = new JwtSecurityToken
                (
                    issuer: configuration["Jwt:Issuer"],
                    audience: configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(60),
                    notBefore: DateTime.UtcNow,
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                        SecurityAlgorithms.HmacSha256)
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                return Results.Ok(tokenString);
            }
            return Results.BadRequest("Invalid user credentials");
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
        private async Task<IResult> GetUser(int id, IRequestClient<GetUser> requestClient)
        {
            try
            {

                GetUser getUser = new GetUser() { Id = id };
                var resp = await requestClient.GetResponse<UserDTO>(requestClient);
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
        private async Task<PaginatedList<UserDTO>> GetUsers(int pageSize, int pageNo, IRequestClient<GetUserList> requestClient)
        {
            try
            {
                GetUserList getUsers = new GetUserList() { pageSize = pageSize, pageNo = pageNo };

                var resp = await requestClient.GetResponse<PaginatedList<UserDTO>>(getUsers);
                return resp.Message;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
        private async Task<IResult> CreateUser(AddUser adduser, IRequestClient<AddUser> requestClient)
        {
            try
            {
                //IRequestClient<AddUser> requestClient = ServiceFactory.GetService<IRequestClient<AddUser>>();
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
        private async Task<IResult> DeleteUser(int id, IRequestClient<DeleteUser> requestClient)
        {
            try
            {

                DeleteUser deleteUser = new DeleteUser() { Id = id };
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
        private async Task<IResult> UpdateUser([FromBody] UpdateUser edituser, IRequestClient<UpdateUser> requestClient)
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
