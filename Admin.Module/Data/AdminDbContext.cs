using Admin.Module.Domain;
using Admin.Module.Domain.Common;
using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Runtime.ConstrainedExecution;

namespace Admin.Module.Data
{
    public class AdminDbContext : DbContext, IAdminDbContext
    {
        public DbSet<Institute> Institutes { get; set; }

        public IDbConnection DbConnection { get { return Database.GetDbConnection(); } }

        public AdminDbContext(DbContextOptions<AdminDbContext> options)
            : base(options)
        { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Ignore(typeof(BaseEntity));
            modelBuilder.Ignore(typeof(BaseAuditableEntity));
            modelBuilder.Ignore(typeof(BaseMasterEntity));
            modelBuilder.Entity<Institute>()
                .ToTable(DbKeywords.INSTITUTES_TABLENAME);
        }
    }

}
