using LanguageApp.Views;

namespace LanguageApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ProgressPage), typeof(ProgressPage));
            Routing.RegisterRoute(nameof(TypingPage), typeof(TypingPage));
            Routing.RegisterRoute(nameof(FlashCardsPage), typeof(FlashCardsPage));
        }
    }
}
