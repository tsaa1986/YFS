using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using YFS.Repo.Data;

namespace YFS.IntegrationTests
{
    public class TestingWebAppFactory<TStartup> : WebApplicationFactory<Program> //where TEntryPoint : Program
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            //builder.UseContentRoot(".");

            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<RepositoryContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                services.AddDbContext<RepositoryContext>(options =>
                {
                    options.UseInMemoryDatabase("YFSTest");
                });

                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                using (var appContext = scope.ServiceProvider.GetRequiredService<RepositoryContext>())
                {
                    try
                    {
                        appContext.Database.EnsureCreated();
                    }
                    catch (Exception ex)
                    {
                        //Log errors or do anything you think it's needed
                        throw;
                    }
                }
            });
        }
    }


}
