using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using YFS.Core.Enums;
using YFS.Core.Utilities;
using YFS.Service.Services;

namespace YFS.Middleware
{
        public class LanguageMiddleware
        {
            private readonly RequestDelegate _next;

            public LanguageMiddleware(RequestDelegate next)
            {
                _next = next;
            }

            public async Task Invoke(HttpContext context, LanguageScopedService languageService)
            {
                string languageCode = context.Request.Headers["Accept-Language"];

                if (string.IsNullOrWhiteSpace(languageCode) || !LanguageUtility.IsValidLanguageCode(languageCode))
                {
                    languageCode = LanguageUtility.GetLanguageCode(Language.English);
                }

                languageService.SetLanguage(languageCode);

                await _next(context);
            }
        }
    }
