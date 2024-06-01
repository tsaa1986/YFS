using YFS.Core.Models;

namespace YFS.Repo.Data
{
    public static partial class DatabaseInitializer
    {
        public static void Initialize(RepositoryContext context)
        {
            context.Database.EnsureCreated();

            InitializeCurrencies(context);
            InitializeMccs(context);
            InitializeAccountTypes(context);
            InitializeMccCategoryMapping(context);
        }
    }
}
