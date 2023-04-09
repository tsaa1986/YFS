using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YFS.Core.Models;

namespace YFS.Repo.Data
{
    public class BankData : IEntityTypeConfiguration<Bank>
    {
        public void Configure(EntityTypeBuilder<Bank> builder)
        {
            builder.HasData(
                new Bank {
                    Id = 1,
                    Name = "Demo Bank",     
                }
            );
        }
    }
}
