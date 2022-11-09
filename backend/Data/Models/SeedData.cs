using Microsoft.Extensions.DependencyInjection;
using System;
using YFS.Data.Repository;

namespace YFS.Data.Models
{
    public static class SeedData
    {
        public static void EnsurePopulated(IServiceProvider services)
        {
            RepositoryContext context =
            services.GetRequiredService<RepositoryContext>();
            //context.Database.Migrate();
            //if (!context.Products.Any())
            //{
            //    context.Products.AddRange(
            // ...statements omiited for brevity...
            //    );
            //    context.SaveChanges();
            //}
        }
    }
}
