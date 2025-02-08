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
        private string _selectedFlag = "default_flag.png";
        public bool IsFlagVisible => !string.IsNullOrEmpty(SelectedFlag);

        private readonly Dictionary<string, string> LanguageFlags = new()
        {
            { "Swedish (Svenska)", "sweden_flag.png" },
            { "Norwegian (Nosrk)", "norway_flag.png" },
            { "Finnish (Suomi)", "finland_flag.png" },
            { "Danish (Dansk)", "denmark_flag.png" },
            { "Icelandic (Íslenska)", "iceland_flag.png" }

        };

        public ObservableCollection<string> Languages { get; } = new()
        {
            "Swedish (Svenska)",
            "Norwegian (Nosrk)",
            "Finnish (Suomi)",
            "Danish (Dansk)",
            "Icelandic (Íslenska)"
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
                        "Swedish (Svenska)" => "sv",
                        "Norwegian (Nosrk)" => "no",
                        "Finnish (Suomi)" => "fi",
                        "Danish (Dansk)" => "da",
                        "Icelandic (Íslenska)" => "is",
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
                if (SetProperty(ref _selectedFlag, value ?? "default_flag.png"))
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

