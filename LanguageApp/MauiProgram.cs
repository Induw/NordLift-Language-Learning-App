using Microsoft.Extensions.Logging;
using LanguageApp.Services;

namespace LanguageApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("SegoeUI.ttf", "SegoeUI");
                    fonts.AddFont("SegoeUI-Bold.ttf", "SegoeUIBold");
                });
            builder.Services.AddSingleton<HttpClient>();
            builder.Services.AddSingleton<ITranslationService, TranslationService>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
