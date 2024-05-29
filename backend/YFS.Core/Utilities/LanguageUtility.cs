using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YFS.Core.Enums;

namespace YFS.Core.Utilities
{
    public static class LanguageUtility
    {
        private static readonly Dictionary<Language, string> LanguageCodeMap = new Dictionary<Language, string>
    {
        { Language.English, "en" },
        { Language.Russian, "ru" },
        { Language.Ukrainian, "ua" }
    };

        public static string GetLanguageCode(Language language)
        {
            if (!LanguageCodeMap.TryGetValue(language, out string languageCode))
            {
                throw new ArgumentException("Unsupported language.", nameof(language));
            }
            return languageCode;
        }

        public static bool IsValidLanguageCode(string languageCode)
        {
            return LanguageCodeMap.ContainsValue(languageCode);
        }

        public static string GetLanguageCodeFromRequest(HttpRequest request)
        {
            string languageCode = request.Headers["Accept-Language"];
            if (string.IsNullOrWhiteSpace(languageCode) || !IsValidLanguageCode(languageCode))
            {
                languageCode = GetLanguageCode(Language.English);
            }
            return languageCode;
        }
    }

}
