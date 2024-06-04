using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using YFS.Core.Models;
using YFS.Core.Models.MonoIntegration;
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
        public DbSet<AccountTypeTranslation> AccountTypeTranslations { get; set; } = null!;
        public DbSet<Currency> Currencies { get; set; } = null!;
        public DbSet<Operation> Operations { get; set; } = null!;
        public DbSet<OperationItem> OperationItem { get; set; } = null!;
        public DbSet<Tag> Tags { get; set; } = null!;
        public DbSet<OperationTag> OperationTags { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Bank> Banks { get; set; } = null!;
        public DbSet<BankSyncHistory> BankSyncHistories { get; set; } = null!;
        public DbSet<ApiToken> ApiTokens { get; set; } = null!;
        public DbSet<MerchantCategoryCode> Mccs { get; set; } = null!;
        public DbSet<MccCategoryMapping> MccCategoryMappings { get; set; } = null!;
        public DbSet<AccountSyncSettings> AccountSyncSettings { get; set; } = null!;
        public DbSet<MonoSyncRule> MonoSyncRules { get; set; } = null!;

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

            //modelBuilder.Ignore<CategoryTranslation>();

            //modelBuilder.Entity<Account>().HasOne(a => a.User).WithOne();//.HasForeignKey(a => a.UserId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Account>()
                        .Property(b => b.Favorites)
                        .HasDefaultValueSql("0");
            modelBuilder.Entity<AccountBalance>().Property(ab => ab.LastUpdateTime).HasDefaultValueSql("CURRENT_TIMESTAMP");
            modelBuilder.Entity<AccountBalance>().Property(ab => ab.Balance).HasDefaultValueSql("0.0");

            modelBuilder.Entity<AccountGroup>()
                        .HasMany(ag => ag.Translations)
                        .WithOne(agt => agt.AccountGroup)
                        .HasForeignKey(agt => agt.AccountGroupId);
            modelBuilder.Entity<AccountGroupTranslation>()
                        .HasIndex(agt => new { agt.AccountGroupId, agt.LanguageCode })
                        .IsUnique();
            modelBuilder.Entity<AccountGroup>().HasMany(a => a.Accounts).WithOne().OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Bank>().HasMany(ac => ac.Accounts).WithOne(b => b.Bank).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Account>()
                .HasOne(a => a.Bank) 
                .WithMany(b => b.Accounts) 
                .HasForeignKey(a => a.Bank_GLMFO);
            modelBuilder.Entity<AccountType>().Property(at => at.CreatedOn).HasDefaultValueSql("CURRENT_TIMESTAMP");
            modelBuilder.Entity<AccountType>().Property(at => at.TypeOrderBy).HasDefaultValueSql("0");
            //modelBuilder.Entity<AccountType>().HasMany(a => a.Accounts).WithOne().OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Currency>().Property(c => c.CurrencyId).ValueGeneratedOnAdd();
            modelBuilder.Entity<Currency>().HasMany(a => a.Accounts).WithOne(c => c.Currency).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>().Property(b => b.CreatedOn).HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<OperationTag>()
                .HasKey(ot => new { ot.OperationId, ot.TagId });

            modelBuilder.Entity<OperationTag>()
                .HasOne(ot => ot.Operation)
                .WithMany(o => o.OperationTags)
                .HasForeignKey(ot => ot.OperationId);

            modelBuilder.Entity<OperationTag>()
                .HasOne(ot => ot.Tag)
                .WithMany(t => t.OperationTags)
                .HasForeignKey(ot => ot.TagId);

            modelBuilder.Entity<Category>()
                    .OwnsMany(c => c.Translations, t =>
                    {
                        t.ToTable("CategoryTranslations"); 
                        t.HasKey(ct => ct.Id); 
                        t.Property(ct => ct.Name).IsRequired().HasMaxLength(100).HasColumnType("VARCHAR");
                        t.Property(ct => ct.LanguageCode).IsRequired().HasMaxLength(10).HasColumnType("VARCHAR");
                        // Other configuration if needed
                    });

            modelBuilder.Entity<MccCategoryMapping>()
                .HasIndex(e => new { e.MccCode, e.CategoryId, e.Description })
                .IsUnique();

            modelBuilder.Entity<MonoSyncTransaction>()
                .HasIndex(mst => new { mst.MonobankTransactionId, mst.OperationId })
                .IsUnique();

            modelBuilder.Entity<MonoSyncTransaction>()
                .HasOne(mst => mst.Operation)
                .WithOne(o => o.MonoSyncTransaction)
                .HasForeignKey<MonoSyncTransaction>(mst => mst.OperationId);

            modelBuilder.ApplyConfiguration(new AccountTypeData());
            modelBuilder.ApplyConfiguration(new CategoryData());
            modelBuilder.ApplyConfiguration(new BankData());
        } 
    }
}
