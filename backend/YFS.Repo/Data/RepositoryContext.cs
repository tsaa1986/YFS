using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YFS.Core.Models;

namespace YFS.Repo.Data
{
    public class RepositoryContext : IdentityDbContext<User>
    {
        public DbSet<AccountGroup> AccountGroups { get; set; } //= null!;
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountType> AccountsType { get; set; }
        public DbSet<Currency> Currencies { get; set; } //= null!;

        public RepositoryContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AccountGroup>().HasIndex(entity => new { entity.UserId, entity.AccountGroupNameUa }).IsUnique();
            modelBuilder.Entity<AccountGroup>().HasIndex(entity => new { entity.UserId, entity.AccountGroupNameRu }).IsUnique();
            modelBuilder.Entity<AccountGroup>().HasIndex(entity => new { entity.UserId, entity.AccountGroupNameEn }).IsUnique();

            modelBuilder.Entity<AccountType>().Property(b => b.CreatedOn).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<AccountType>().Property(b => b.TypeOrederBy).HasDefaultValueSql("0");

            modelBuilder.Entity<Currency>().Property(p => p.Id).ValueGeneratedNever();

            modelBuilder.Entity<User>().Property(b => b.CreatedOn).HasDefaultValueSql("getdate()");

            modelBuilder.ApplyConfiguration(new AccountTypeData());
            modelBuilder.ApplyConfiguration(new CurrencyData());


            /*modelBuilder.Entity<User>().HasData(
                    new User { Name = "Tom", Age = 37 },
                    new User { Id = 2, Name = "Bob", Age = 41 }
            );*/
        } 
    }
}
