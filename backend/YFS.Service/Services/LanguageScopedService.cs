using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YFS.Core.Enums;

namespace YFS.Service.Services
{
    public class LanguageScopedService
    {
        public Language Language { get; set; } = Language.English; // Default language

        public void SetLanguage(string languageCode)
        {
            if (Enum.TryParse<Language>(languageCode, true, out var language))
            {
                Language = language;
            }
            else
            {
                Language = Language.English; // Fallback to default language
            }
        }
    }
}
