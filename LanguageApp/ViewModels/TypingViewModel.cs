using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace LanguageApp.ViewModels
{
    public class TypingViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T backingField, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingField, value))
                return false;

            backingField = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        private readonly Random random = new Random();
        private string currentLanguage;
        private Dictionary<string, string> currentDictionary;
        private KeyValuePair<string, string> currentWord;

        private string _wordToTranslate;
        public string WordToTranslate
        {
            get => _wordToTranslate;
            set => SetProperty(ref _wordToTranslate, value);
        }

        private string _userInput;
        public string UserInput
        {
            get => _userInput;
            set => SetProperty(ref _userInput, value);
        }

        private string _feedbackMessage = " ";
        public string FeedbackMessage
        {
            get => _feedbackMessage;
            set => SetProperty(ref _feedbackMessage, value);
        }

        private Color _feedbackColor;
        public Color FeedbackColor
        {
            get => _feedbackColor;
            set => SetProperty(ref _feedbackColor, value);
        }

        private bool _isNextWordEnabled;
        public bool IsNextWordEnabled
        {
            get => _isNextWordEnabled;
            set => SetProperty(ref _isNextWordEnabled, value);
        }

        private string _pageTitle;
        public string PageTitle
        {
            get => _pageTitle;
            set => SetProperty(ref _pageTitle, value);
        }

        public ICommand SubmitCommand { get; }
        public ICommand NextWordCommand { get; }


        private readonly Dictionary<string, string> SwedishTranslations = new()
        {
            { "Hej", "Hello" },
            { "Ja", "Yes" },
            { "Nej", "No" },
            { "Tack", "Thank you" },
            { "Förlåt", "Sorry" },
            { "God morgon", "Good morning" },
            { "God natt", "Good night" },
            { "Hej då", "Goodbye" },
            { "Snälla", "Please" },
            { "Ursäkta", "Excuse me" },
            { "Hur mår du?", "How are you?" },
            { "Jag förstår", "I understand" },
            { "Jag vet inte", "I don't know" },
            { "Vad heter du?", "What is your name?" },
            { "Jag heter...", "My name is..." }
        };

        private readonly Dictionary<string, string> NorwegianTranslations = new()
        {
            { "Hei", "Hello" },
            { "Ja", "Yes" },
            { "Nei", "No" },
            { "Takk", "Thank you" },
            { "Beklager", "Sorry" },
            { "God morgen", "Good morning" },
            { "God natt", "Good night" },
            { "Ha det", "Goodbye" },
            { "Vær så snill", "Please" },
            { "Unnskyld", "Excuse me" },
            { "Hvordan har du det?", "How are you?" },
            { "Jeg forstår", "I understand" },
            { "Jeg vet ikke", "I don't know" },
            { "Hva heter du?", "What is your name?" },
            { "Jeg heter...", "My name is..." }
        };

        private readonly Dictionary<string, string> FinnishTranslations = new()
        {
            { "Hei", "Hello" },
            { "Kyllä", "Yes" },
            { "Ei", "No" },
            { "Kiitos", "Thank you" },
            { "Anteeksi", "Sorry" },
            { "Hyvää huomenta", "Good morning" },
            { "Hyvää yötä", "Good night" },
            { "Hei hei", "Goodbye" },
            { "Ole hyvä", "Please" },
            { "Anteeksi, voisitko auttaa?", "Excuse me, could you help?" },
            { "Mitä kuuluu?", "How are you?" },
            { "Ymmärrän", "I understand" },
            { "En tiedä", "I don't know" },
            { "Mikä on nimesi?", "What is your name?" },
            { "Minun nimeni on...", "My name is..." }
        };

        private readonly Dictionary<string, string> DanishTranslations = new()
        {
            { "Hej", "Hello" },
            { "Ja", "Yes" },
            { "Nej", "No" },
            { "Tak", "Thank you" },
            { "Undskyld", "Sorry" },
            { "God morgen", "Good morning" },
            { "God nat", "Good night" },
            { "Farvel", "Goodbye" },
            { "Vær venlig", "Please" },
            { "Undskyld mig", "Excuse me" },
            { "Hvordan har du det?", "How are you?" },
            { "Jeg forstår", "I understand" },
            { "Jeg ved ikke", "I don't know" },
            { "Hvad hedder du?", "What is your name?" },
            { "Jeg hedder...", "My name is..." }
        };

        private readonly Dictionary<string, string> IcelandicTranslations = new()
        {
            { "Halló", "Hello" },
            { "Já", "Yes" },
            { "Nei", "No" },
            { "Takk", "Thank you" },
            { "Fyrirgefðu", "Sorry" },
            { "Góðan morgun", "Good morning" },
            { "Góða nótt", "Good night" },
            { "Bless", "Goodbye" },
            { "Vinsamlegast", "Please" },
            { "Afsakið", "Excuse me" },
            { "Hvernig hefur þú það?", "How are you?" },
            { "Ég skil", "I understand" },
            { "Ég veit ekki", "I don't know" },
            { "Hvað heitir þú?", "What is your name?" },
            { "Ég heiti...", "My name is..." }
        };

        public TypingViewModel()
        {
            currentLanguage = Preferences.Get("SelectedLanguage", "sv");
            SetDictionary();
            LoadNextWord();
            PageTitle = $"Translate : {GetLanguageFullName(currentLanguage)}";

            SubmitCommand = new Command(OnSubmit);
            NextWordCommand = new Command(OnNextWord);
        }


        private void SetDictionary()
        {
            currentDictionary = currentLanguage switch
            {
                "sv" => SwedishTranslations,
                "no" => NorwegianTranslations,
                "fi" => FinnishTranslations,
                "da" => DanishTranslations,
                "is" => IcelandicTranslations,
                _ => SwedishTranslations
            };
        }

        private void LoadNextWord()
        {
            var keys = currentDictionary.Keys.ToList();
            var randomKey = keys[random.Next(keys.Count)];
            currentWord = new KeyValuePair<string, string>(randomKey, currentDictionary[randomKey]);

            WordToTranslate = currentWord.Key;
            UserInput = string.Empty;
            FeedbackMessage = " ";
            IsNextWordEnabled = false;
        }

        private void OnSubmit()
        {
            var userInput = UserInput?.Trim();

            if (string.IsNullOrEmpty(userInput))
            {
                FeedbackMessage = "Come on, type something, don't worry about getting it wrong!😀";
                FeedbackColor = Colors.Red;
                return;
            }

            if (string.Equals(userInput, currentWord.Value, StringComparison.OrdinalIgnoreCase))
            {
                FeedbackMessage = "Correct!👍 Keep it Up !";
                FeedbackColor = Colors.Green;
            }
            else
            {
                FeedbackMessage = $"Incorrect!👎.The correct translation is: {currentWord.Value}.";
                FeedbackColor = Colors.Maroon;
            }
            IsNextWordEnabled = true;
        }

        private void OnNextWord()
        {
            LoadNextWord();
        }

        private string GetLanguageFullName(string languageCode)
        {
            return languageCode switch
            {
                "sv" => "Swedish",
                "no" => "Norwegian",
                "fi" => "Finnish",
                "da" => "Danish",
                "is" => "Icelandic",
                _ => "Swedish"
            };
        }
    }
}
