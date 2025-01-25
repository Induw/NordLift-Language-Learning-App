using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LanguageApp.Views;

namespace LanguageApp.ViewModels
{
    public class MainPageViewModel : ObservableObject
    {
        private string _selectedLanguage;

        public ObservableCollection<string> Languages { get; } = new()
        {
            "Swedish",
            "Norwegian",
            "Finnish",
            "Danish",
            "Icelandic"
        };

        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                SetProperty(ref _selectedLanguage, value);

                if (!string.IsNullOrEmpty(value))
                {
                    var languageCode = value switch
                    {
                        "Swedish" => "sv",
                        "Norwegian" => "no",
                        "Finnish" => "fi",
                        "Danish" => "da",
                        "Icelandic" => "is",
                        _ => "sv"
                    };

                    Preferences.Set("SelectedLanguage", languageCode);
                }
            }
        }
        public ICommand NavigateToFlashcardsCommand { get; }
        public ICommand NavigateToChallengesCommand { get; }
        public ICommand NavigateToProgressCommand { get; }

        public MainPageViewModel()
        {
            NavigateToFlashcardsCommand = new RelayCommand(OnNavigateToFlashcards);
            NavigateToChallengesCommand = new RelayCommand(OnNavigateToChallenges);
            NavigateToProgressCommand = new RelayCommand(OnNavigateToProgress);
        }

        private async void OnNavigateToFlashcards()
        {
            await Shell.Current.GoToAsync(nameof(FlashCardsPage));
        }

        private async void OnNavigateToChallenges()
        {
            await Shell.Current.GoToAsync(nameof(TypingPage));
        }

        private async void OnNavigateToProgress()
        {
            await Shell.Current.GoToAsync(nameof(ProgressPage));
        }
    }
}

