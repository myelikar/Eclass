using Admin.Module.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IAdminDbContext
    {
        IDbConnection DbConnection { get; }
        DbSet<Institute> Institutes { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }

}
