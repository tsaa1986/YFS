using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using YFS.Core.Models;
using YFS.Core.Models.Triggers;

namespace YFS.Repo.Data
{
    public class RepositoryContext : IdentityDbContext<User>
    {
        public DbSet<AccountGroup> AccountGroups { get; set; } = null!;
        public DbSet<Account> Accounts { get; set; } = null!;
        public DbSet<AccountBalance> AccountsBalance { get; set; } = null!;
        public DbSet<AccountMonthlyBalance> AccountsMonthlyBalance { get; set; } = null!;
        public DbSet<AccountType> AccountTypes { get; set; } = null!;
        public DbSet<Currency> Currencies { get; set; } = null!;
        public DbSet<Operation> Operations { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Bank> Banks { get; set; } = null!;
        public DbSet<BankSyncHistory> BankSyncHistories { get; set; } = null!;

        public RepositoryContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseTriggers(triggersOptions =>
            {
                triggersOptions.AddTrigger<AccountBalanceUpdateDateTrigger>();
                triggersOptions.AddTrigger<AccountMonthlyBalanceUpdateMonthBalance>();
            });

            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Account>().HasOne(a => a.User).WithOne();//.HasForeignKey(a => a.UserId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Account>().Property(b => b.Favorites).HasDefaultValueSql("0");
            modelBuilder.Entity<AccountBalance>().Property(ab => ab.LastUpdateTime).HasDefaultValueSql("CURRENT_TIMESTAMP");
            modelBuilder.Entity<AccountBalance>().Property(ab => ab.Balance).HasDefaultValueSql("0.0");

            modelBuilder.Entity<AccountGroup>().HasIndex(entity => new { entity.UserId, entity.AccountGroupNameUa }).IsUnique();
            modelBuilder.Entity<AccountGroup>().HasIndex(entity => new { entity.UserId, entity.AccountGroupNameRu }).IsUnique();
            modelBuilder.Entity<AccountGroup>().HasIndex(entity => new { entity.UserId, entity.AccountGroupNameEn }).IsUnique();
            modelBuilder.Entity<AccountGroup>().HasMany(a => a.Accounts).WithOne().OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Bank>().HasMany(ac => ac.Accounts).WithOne(b => b.Bank).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Account>()
                .HasOne(a => a.Bank) 
                .WithMany(b => b.Accounts) 
                .HasForeignKey(a => a.Bank_GLMFO);
            modelBuilder.Entity<AccountType>().Property(at => at.CreatedOn).HasDefaultValueSql("CURRENT_TIMESTAMP");
            modelBuilder.Entity<AccountType>().Property(at => at.TypeOrderBy).HasDefaultValueSql("0");
            modelBuilder.Entity<AccountType>().HasMany(a => a.Accounts).WithOne().OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Currency>().HasMany(a => a.Accounts).WithOne(c => c.Currency).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Currency>().Property(c => c.CurrencyId).ValueGeneratedNever();

            modelBuilder.Entity<User>().Property(b => b.CreatedOn).HasDefaultValueSql("CURRENT_TIMESTAMP");

            //modelBuilder.ApplyConfiguration(new UserData());
            modelBuilder.ApplyConfiguration(new CurrencyData());
            modelBuilder.ApplyConfiguration(new AccountTypeData());
            modelBuilder.ApplyConfiguration(new CategoryData());
            modelBuilder.ApplyConfiguration(new BankData());


        } 
    }
}
