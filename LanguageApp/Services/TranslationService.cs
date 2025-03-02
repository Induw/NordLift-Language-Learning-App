using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LanguageApp.Services
{
    public class TranslationService : ITranslationService
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://api.mymemory.translated.net/get";

        public TranslationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetTranslationAsync(string text, string sourceLang, string targetLang)
        {
            try
            {
                var response = await _httpClient.GetStringAsync($"{ApiUrl}?q={Uri.EscapeDataString(text)}&langpair={sourceLang}|{targetLang}");
                var jsonResponse = JsonDocument.Parse(response);

                if (jsonResponse.RootElement.TryGetProperty("responseData", out var responseData) &&
                    responseData.TryGetProperty("translatedText", out var translatedText))
                {
                    return translatedText.GetString();
                }
            }
            catch (Exception)
            {
                return "Translation Error";
            }
            return string.Empty;
        }

        public string GetLanguageFullName(string languageCode)
        {
            return languageCode switch
            {
                "sv" => "Swedish",
                "no" => "Norwegian",
                "fi" => "Finnish",
                "da" => "Danish",
                "is" => "Icelandic",
                _ => "Swedish"
            };
        }
    }
    
}
