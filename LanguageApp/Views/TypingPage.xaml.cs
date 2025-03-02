using Microsoft.Maui.Controls;
using LanguageApp.ViewModels;
using LanguageApp.Services;

namespace LanguageApp.Views
{
    public partial class TypingPage : ContentPage
    {
        public TypingPage(ITranslationService translationService)
        {
            InitializeComponent();
            BindingContext = new TypingViewModel(translationService);
        }
    }
}