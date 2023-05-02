using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using YFS.Core.Models;

namespace YFS.Repo.Data
{
    public class RepositoryContext : IdentityDbContext<User>
    {
        public DbSet<AccountGroup> AccountGroups { get; set; } = null!;
        public DbSet<Account> Accounts { get; set; } = null!;
        public DbSet<AccountType> AccountTypes { get; set; } = null!;
        public DbSet<Currency> Currencies { get; set; } = null!;
        public DbSet<Operation> Operations { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Bank> Banks { get; set; } = null!;

        public RepositoryContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Account>()
                //.HasMany(a => a.User).WithOne(a => a.Id)//.HasForeignKey(a => a.UserId).OnDelete(DeleteBehavior.Cascade);
                .Property(b => b.Favorites).HasDefaultValueSql("0");
             
            modelBuilder.Entity<AccountGroup>().HasIndex(entity => new { entity.UserId, entity.AccountGroupNameUa }).IsUnique();
            modelBuilder.Entity<AccountGroup>().HasIndex(entity => new { entity.UserId, entity.AccountGroupNameRu }).IsUnique();
            modelBuilder.Entity<AccountGroup>().HasIndex(entity => new { entity.UserId, entity.AccountGroupNameEn }).IsUnique();
            modelBuilder.Entity<AccountGroup>().HasMany(a => a.Accounts).WithOne().OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Bank>().HasMany(ac => ac.Accounts).WithOne(b => b.Bank).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AccountType>().Property(at => at.CreatedOn).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<AccountType>().Property(at => at.TypeOrederBy).HasDefaultValueSql("0");
            modelBuilder.Entity<AccountType>().HasMany(a => a.Accounts).WithOne().OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Currency>().HasMany(a => a.Accounts).WithOne(c => c.Currency).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Currency>().Property(c => c.CurrencyId).ValueGeneratedNever();

            modelBuilder.Entity<User>().Property(b => b.CreatedOn).HasDefaultValueSql("getdate()");

            //modelBuilder.ApplyConfiguration(new UserData());
            modelBuilder.ApplyConfiguration(new CurrencyData());
            modelBuilder.ApplyConfiguration(new AccountTypeData());
            modelBuilder.ApplyConfiguration(new CategoryData());
            modelBuilder.ApplyConfiguration(new BankData());

        } 
    }
}
