using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageApp.Services
{
    public interface ITranslationService 
    {
        Task<string> GetTranslationAsync(string text,string sourceLang, string targetLang);
        public string GetLanguageFullName(string languageCode);
    }
}
