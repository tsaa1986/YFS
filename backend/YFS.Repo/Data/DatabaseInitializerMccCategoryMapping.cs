using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YFS.Core.Models;

namespace YFS.Repo.Data
{
    public static partial class DatabaseInitializer
    {
        private static void InitializeMccCategoryMapping(RepositoryContext context)
        {
            if (!context.MccCategoryMappings.Any())
            {
                var mccCategoryMappings = new List<MccCategoryMapping>()
            {
                //food
                new MccCategoryMapping { MccCode = 5441, CategoryId = 5, Description = "Monobank" },
                new MccCategoryMapping { MccCode = 5921, CategoryId = 5, Description = "Monobank" },
                new MccCategoryMapping { MccCode = 5411, CategoryId = 5, Description = "Monobank" },
                new MccCategoryMapping { MccCode = 5412, CategoryId = 5, Description = "Monobank" },
                new MccCategoryMapping { MccCode = 5451, CategoryId = 5, Description = "Monobank" },
                new MccCategoryMapping { MccCode = 5422, CategoryId = 5, Description = "Monobank" },
                new MccCategoryMapping { MccCode = 5297, CategoryId = 5, Description = "Monobank" },
                new MccCategoryMapping { MccCode = 5298, CategoryId = 5, Description = "Monobank" },
                new MccCategoryMapping { MccCode = 5331, CategoryId = 5, Description = "Monobank" },
                new MccCategoryMapping { MccCode = 5715, CategoryId = 5, Description = "Monobank" },
                new MccCategoryMapping { MccCode = 5300, CategoryId = 5, Description = "Monobank" },
                new MccCategoryMapping { MccCode = 5462, CategoryId = 5, Description = "Monobank" },
                new MccCategoryMapping { MccCode = 5399, CategoryId = 5, Description = "Monobank" },
                new MccCategoryMapping { MccCode = 5499, CategoryId = 5, Description = "Monobank" },
                new MccCategoryMapping { MccCode = 5311, CategoryId = 5, Description = "Monobank" },
                //clothing
                new MccCategoryMapping { MccCode = 7296, CategoryId = 11, Description = "Monobank" },
                new MccCategoryMapping { MccCode = 5697, CategoryId = 11, Description = "Monobank" },
                new MccCategoryMapping { MccCode = 5698, CategoryId = 11, Description = "Monobank" },
                new MccCategoryMapping { MccCode = 5699, CategoryId = 11, Description = "Monobank" },
                new MccCategoryMapping { MccCode = 5641, CategoryId = 11, Description = "Monobank" },
                new MccCategoryMapping { MccCode = 5611, CategoryId = 11, Description = "Monobank" },
                new MccCategoryMapping { MccCode = 5931, CategoryId = 11, Description = "Monobank" },
                new MccCategoryMapping { MccCode = 5137, CategoryId = 11, Description = "Monobank" },
                new MccCategoryMapping { MccCode = 5681, CategoryId = 11, Description = "Monobank" },
                new MccCategoryMapping { MccCode = 5139, CategoryId = 11, Description = "Monobank" },
                new MccCategoryMapping { MccCode = 5651, CategoryId = 11, Description = "Monobank" },
                new MccCategoryMapping { MccCode = 7251, CategoryId = 11, Description = "Monobank" },
                new MccCategoryMapping { MccCode = 5621, CategoryId = 11, Description = "Monobank" },
                new MccCategoryMapping { MccCode = 5655, CategoryId = 11, Description = "Monobank" },
                new MccCategoryMapping { MccCode = 5691, CategoryId = 11, Description = "Monobank" },
                new MccCategoryMapping { MccCode = 5948, CategoryId = 11, Description = "Monobank" },
                new MccCategoryMapping { MccCode = 5661, CategoryId = 11, Description = "Monobank" },
                new MccCategoryMapping { MccCode = 5449, CategoryId = 11, Description = "Monobank" },
                new MccCategoryMapping { MccCode = 5631, CategoryId = 11, Description = "Monobank" },
                new MccCategoryMapping { MccCode = 5131, CategoryId = 11, Description = "Monobank" },
                //mobile phone
                new MccCategoryMapping { MccCode = 4814, CategoryId = 17, Description = "Monobank" },

            };

                context.MccCategoryMappings.AddRange(mccCategoryMappings);
                context.SaveChanges();
            }
        }
    }
}
