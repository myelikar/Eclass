using Admin.Module.Data.Repositories.Common;
using Admin.Module.Domain;

namespace Admin.Module.Data.Repositories
{
    public interface IInstituteRepo : IBaseRepository<Institute>
    {

    }
    public class InstituteRepo : BaseRepository<Institute>, IInstituteRepo
    {
        public InstituteRepo(AdminDbContext _context) : base(_context)
        {

        }
    }
}
