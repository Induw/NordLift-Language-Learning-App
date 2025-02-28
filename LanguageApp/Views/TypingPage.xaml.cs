using Microsoft.Maui.Controls;
using LanguageApp.ViewModels;

namespace LanguageApp.Views
{
    public partial class TypingPage : ContentPage
    {
        public TypingPage()
        {
            InitializeComponent();
            BindingContext = new TypingViewModel();
        }
    }
}