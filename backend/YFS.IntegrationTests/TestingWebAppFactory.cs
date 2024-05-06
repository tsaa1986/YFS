using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System.Text;
using YFS.Repo.Data;

namespace YFS.IntegrationTests
{
    public class TestingWebAppFactory<TStartup> : WebApplicationFactory<Program>
    {
        private static string? _jwtTokenForDemoUser;
        public TestingWebAppFactory()
        {
            if (_jwtTokenForDemoUser == null)
            {
                _jwtTokenForDemoUser = GetJwtTokenForDemoUserAsync().GetAwaiter().GetResult();
            }
        }
        public static string GetJwtTokenForDemoUser()
        {
            return _jwtTokenForDemoUser;
        }
        private async Task<string> GetJwtTokenForDemoUserAsync()
        {
            // Create a HttpClient to make the request
            var client = CreateClient();

            // Prepare the request payload (if required)
            var requestContent = new StringContent(
                "{ \"username\": \"Demo\", \"password\": \"demo123$qweR\" }",
                Encoding.UTF8,
                "application/json"
            );

            // Make the request to the authentication endpoint
            var response = await client.PostAsync("/api/Authentication/sign-in", requestContent);

            // Ensure the request was successful
            response.EnsureSuccessStatusCode();

            // Extract the JWT token from the response
            var content = await response.Content.ReadAsStringAsync();
            var token = JObject.Parse(content)["token"].ToString();

            return token;
        }
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
