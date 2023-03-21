using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YFS.Core.Models;

namespace YFS.Repo.Data
{
    internal class UserData : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(
                new User { Id = "e6891cdf-c8c8-4691-8e94-268f1301099d", FirstName = "Demo", LastName = "", UserName = "Demo", Email = "demo@email.com", 
                    PasswordHash= "AQAAAAEAACcQAAAAEMIM+GSNA+O2h3LaKhOGMuOwYm5okSD4Ff/YZYazEKQb3QZAt7FNsTDPZoPMxSwSwQ==",
                    SecurityStamp= "WS4OQVPL3BHWC35UE5GZKUBMZ7LN5ZGF",
                    ConcurrencyStamp= "a886ae1a-9256-4273-a65a-37165bca2145",
                    PhoneNumber="123456789"
                }
                );
        }
    }
}
