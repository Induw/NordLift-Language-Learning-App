using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LanguageApp.Views;

namespace LanguageApp.ViewModels
{
    public class MainPageViewModel : ObservableObject
    {
        private double _pickerFontSize = 16; 
        private string? _selectedLanguage = "sv";
        private string? _selectedLanguageDescription;
        private string _selectedFlag = "default_flag.png";
        private string _defaultDescription = "NordLift is your ultimate learning companion for mastering Nordic languages, Start learning today and unlock the beauty of Scandinavian communication! \nNOTE :The default language if you do not choose one is Swedish.";
        public bool IsFlagVisible => !string.IsNullOrEmpty(SelectedFlag);


        private readonly Dictionary<string, string> LanguageFlags = new()
        {
            { "Swedish (Svenska)", "sweden_flag.png" },
            { "Norwegian (Norsk)", "norway_flag.png" },
            { "Finnish (Suomi)", "finland_flag.png" },
            { "Danish (Dansk)", "denmark_flag.png" },
            { "Icelandic (Íslenska)", "iceland_flag.png" }

        };

        public ObservableCollection<string> Languages { get; } = new()
        {
            "Swedish (Svenska)",
            "Norwegian (Norsk)",
            "Finnish (Suomi)",
            "Danish (Dansk)",
            "Icelandic (Íslenska)"
        };
        private readonly Dictionary<string, string> LanguageDescriptions = new()
        {
            { "Swedish (Svenska)", "Swedish is known for its musical, sing-song intonation and clear enunciation, making it elegant and easy on the ear. It has influenced global design, literature, and music, giving the world ABBA and other cultural icons. Learning it connects you to a rich Scandinavian heritage and modern innovation." },

            { "Norwegian (Norsk)", "Norwegian has two official written standards—Bokmål and Nynorsk—reflecting its diverse history and dialects. It sounds as lyrical as the fjords are majestic, with a natural rhythm that’s captivating to hear. Mastering Norwegian opens doors to a culture deeply rooted in storytelling and outdoor adventure." },

            { "Finnish (Suomi)", "Finnish is part of the Uralic language family, making it uniquely different from most European languages with its complex grammar and poetic rhythm. It thrives on vowel harmony and long compound words. Its logic and beauty make it deeply rewarding for dedicated language learners." },

            { "Danish (Dansk)", "Danish is famous for its soft pronunciation and the unique 'stød'—a subtle vocal feature that gives it a rhythmic flow. Though often seen as tricky to master, its elegance mirrors Denmark’s world-renowned design aesthetic. Learning Danish brings you closer to a culture rich in innovation and history." },

            { "Icelandic (Íslenska)", "Icelandic has remained remarkably unchanged since the medieval era, preserving ancient Norse vocabulary and grammar found in Viking sagas. This heritage connects speakers to their history while allowing them to understand centuries-old texts, making it a fascinating language to learn." }
        };
        public string ?SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                SetProperty(ref _selectedLanguage, value);

                if (!string.IsNullOrEmpty(value))

                {
                    PickerFontSize = string.IsNullOrEmpty(value) ? 14 : 21;
                    var languageCode = value switch
                    {
                        "Swedish (Svenska)" => "sv",
                        "Norwegian (Norsk)" => "no",
                        "Finnish (Suomi)" => "fi",
                        "Danish (Dansk)" => "da",
                        "Icelandic (Íslenska)" => "is",
                        _ => "sv"
                    };

                    Preferences.Set("SelectedLanguage", languageCode);
                    LanguageFlags.TryGetValue(value, out var flag);
                    SelectedFlag = flag;
                    LanguageDescriptions.TryGetValue(value, out var description);
                    SelectedLanguageDescription = description ?? _defaultDescription;
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
        public string? SelectedLanguageDescription
        {
            get => _selectedLanguageDescription;
            set => SetProperty(ref _selectedLanguageDescription, value ?? _defaultDescription);
        }

        public double PickerFontSize
        {
            get => _pickerFontSize;
            set => SetProperty(ref _pickerFontSize, value);
        }

        public ICommand NavigateToFlashcardsCommand { get; }
        public ICommand NavigateToChallengesCommand { get; }
        public ICommand NavigateToProgressCommand { get; }

        public MainPageViewModel()
        {
            NavigateToFlashcardsCommand = new RelayCommand(OnNavigateToFlashcards);
            NavigateToChallengesCommand = new RelayCommand(OnNavigateToChallenges);
            NavigateToProgressCommand = new RelayCommand(OnNavigateToProgress);
            SelectedLanguageDescription = _defaultDescription;
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

