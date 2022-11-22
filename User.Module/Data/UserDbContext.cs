using User.Module.Domain;
using User.Module.Domain.Common;
using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Runtime.ConstrainedExecution;

namespace User.Module.Data
{
    public class UserDbContext : DbContext, IUserDbContext
    {
        public DbSet<CUser> Users { get; set; }

        public IDbConnection DbConnection { get { return Database.GetDbConnection(); } }

        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Ignore(typeof(BaseEntity));
            modelBuilder.Ignore(typeof(BaseAuditableEntity));
            modelBuilder.Ignore(typeof(BaseMasterEntity));
            modelBuilder.Entity<CUser>()
                .ToTable(DbKeywords.USERS_TABLENAME);
        }
    }

}
