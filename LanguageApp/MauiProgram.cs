using Microsoft.Extensions.Logging;

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

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
