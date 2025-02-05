using LanguageApp.ViewModels;
using Microsoft.Maui.Controls;

namespace LanguageApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel();
        }

    }
}