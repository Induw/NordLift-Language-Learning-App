using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LanguageApp.Views;

namespace LanguageApp.ViewModels
{
    public class MainPageViewModel : ObservableObject
    {
        private double _pickerFontSize = 18; 
        public double PickerFontSize
        {
            get => _pickerFontSize;
            set => SetProperty(ref _pickerFontSize, value);
        }

        private string? _selectedLanguage;
        private string ?_selectedFlag;
        public bool IsFlagVisible => !string.IsNullOrEmpty(SelectedFlag);

        private readonly Dictionary<string, string> LanguageFlags = new()
        {
            { "Swedish", "sweden_flag.png" },
            { "Norwegian", "norway_flag.png" },
            { "Finnish", "finland_flag.png" },
            { "Danish", "denmark_flag.png" },
            { "Icelandic", "iceland_flag.png" }
        };

        public ObservableCollection<string> Languages { get; } = new()
        {
            "Swedish",
            "Norwegian",
            "Finnish",
            "Danish",
            "Icelandic"
        };

        public string ?SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                SetProperty(ref _selectedLanguage, value);

                if (!string.IsNullOrEmpty(value))
                {
                    PickerFontSize = string.IsNullOrEmpty(value) ? 14 : 22;
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
                    LanguageFlags.TryGetValue(value, out var flag);
                    SelectedFlag = flag;
                }
            }
        }
        public string? SelectedFlag
        {
            get => _selectedFlag;
            private set
            {
                if (SetProperty(ref _selectedFlag, value))
                {
                    OnPropertyChanged(nameof(IsFlagVisible));
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

