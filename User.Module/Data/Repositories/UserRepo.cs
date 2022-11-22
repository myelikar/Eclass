using Messages.User;
using Microsoft.EntityFrameworkCore;
using User.Module.Data.Repositories.Common;
using User.Module.Domain;

namespace User.Module.Data.Repositories
{
    public interface IUserRepo : IBaseRepository<CUser>
    {
        Task<CUser> ValidLogin(UserLogin userLogin);

    }
    public class UserRepo : BaseRepository<CUser>, IUserRepo
    {
        public UserRepo(UserDbContext _context) : base(_context)
        {

        }

        public async Task<CUser> ValidLogin(UserLogin userLogin)
        {
            return await table.FirstOrDefaultAsync(p => p.UserName == userLogin.UserName
                                        && p.Password == userLogin.Password);
        }
    }
}
