using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YFS.Core.Models;

namespace YFS.Repo.Data
{
    public class RepositoryContext : IdentityDbContext<User>
    {
        public DbSet<AccountGroup> AccountGroups { get; set; } //= null!;

        public RepositoryContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
            .Property(b => b.CreatedOn)
            .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<AccountGroup>().HasIndex(entity => new { entity.UserId, entity.AccountGroupNameUa }).IsUnique();
            modelBuilder.Entity<AccountGroup>().HasIndex(entity => new { entity.UserId, entity.AccountGroupNameRu }).IsUnique();
            modelBuilder.Entity<AccountGroup>().HasIndex(entity => new { entity.UserId, entity.AccountGroupNameEn }).IsUnique();


            /*modelBuilder.Entity<User>().HasData(
                    new User { Name = "Tom", Age = 37 },
                    new User { Id = 2, Name = "Bob", Age = 41 }
            );*/
        } 
    }
}
