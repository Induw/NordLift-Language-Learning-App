using System;
using System.ComponentModel;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace LanguageApp.ViewModels
{
    public class FlashCardsPageViewModel : INotifyPropertyChanged
    {
        private readonly Random _random = new();
        private string _currentLanguage;
        private string _displayedText;
        private bool IsFlipped;
        private string _originalText;
        private Color _flashcardColor;
        private string _title;
        private string[] words = {
            "Hello",
            "Yes",
            "No",
            "Please",
            "Thank you",
            "Sorry",
            "Excuse me",
            "Good morning",
            "Good night",
            "Goodbye",
            "See you later",
            "How are you?",
            "I am fine",
            "What is your name?",
            "My name is...",
            "Nice to meet you",
            "Where are you from?",
            "I am from...",
            "I don’t understand",
            "Can you help me?",
            "What is this?",
            "How much is it?",
            "I like this",
            "I don’t like this",
            "Do you like it?",
            "I am hungry",
            "I am thirsty",
            "I am tired",
            "This is good",
            "This is bad",
            "Where is the bathroom?",
            "I am lost",
            "I need help",
            "Can you repeat that?",
            "What does this mean?",
            "It’s okay",
            "Be careful",
            "I love you",
            "I am happy",
            "I am sad",
            "Who is this?",
            "Where is it?",
            "How do I get there?",
            "I need water",
            "I need food",
            "I have a question",
            "What time is it?",
            "Let’s go",
            "Wait a moment",
            "Come here",
            "Go there",
            "Stop",
            "Help me, please",
            "I don’t know",
            "What do you think?",
            "Is it far?",
            "Is it near?",
            "One ticket, please",
            "Do you speak English?",
            "I speak a little",
            "It’s beautiful",
            "It’s expensive",
            "It’s cheap",
            "It’s too much",
            "I am learning",
            "Let’s practice",
            "Good job",
            "Try again",
            "Do you understand?"
        };

        public event PropertyChangedEventHandler PropertyChanged;
        private string currentLanguage = Preferences.Get("SelectedLanguage", "sv");

        public event EventHandler<string> OnFlashcardFlipped;

        public FlashCardsPageViewModel()
        {
            _currentLanguage = Preferences.Get("SelectedLanguage", "sv");
            LoadNextFlashcard();
            FlashcardColor = GetRandomColor();
            Title = $"Flashcards in - {GetLanguageFullName(_currentLanguage)}"; 
        }

        public string DisplayedText
        {
            get => _displayedText;
            set
            {
                if (_displayedText != value)
                {
                    _displayedText = value;
                    OnPropertyChanged(nameof(DisplayedText));
                }
            }
        }
        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged(nameof(Title));
                }
            }
        }
        public Color FlashcardColor
        {
            get => _flashcardColor;
            set
            {
                if (_flashcardColor != value)
                {
                    _flashcardColor = value;
                    OnPropertyChanged(nameof(FlashcardColor));
                }
            }
        }

        public ICommand TranslateCommand => new Command(async () => await FlipFlashcard());
        public ICommand NextFlashcardCommand => new Command(() => LoadNextFlashcard());

        private void LoadNextFlashcard()
        {
            string newWord;
            do
            {
                newWord = words[_random.Next(words.Length)];
            } while (newWord == _originalText);

            _originalText = newWord;
            DisplayedText = newWord;
            FlashcardColor = GetRandomColor();
            IsFlipped = false; 
        }
        private async Task FlipFlashcard()
        {

            {
                if (!IsFlipped)
                {
                    string translation = await GetTranslationAsync(_originalText, "en", _currentLanguage);
                    if (!string.IsNullOrEmpty(translation))
                    {
                        DisplayedText = translation;
                        IsFlipped = true;
                        OnFlashcardFlipped?.Invoke(this, translation);
                    }
                }
                else
                {
                    DisplayedText = _originalText;
                    IsFlipped = false;
                    OnFlashcardFlipped?.Invoke(this, _originalText);
                }
            }
        }

        private async Task<string> GetTranslationAsync(string text, string sourceLang, string targetLang)
        {
            const string apiUrl = "https://api.mymemory.translated.net/get";
            try
            {
                using var client = new HttpClient();
                var response = await client.GetStringAsync($"{apiUrl}?q={Uri.EscapeDataString(text)}&langpair={sourceLang}|{targetLang}");
                var jsonResponse = JsonDocument.Parse(response);

                if (jsonResponse.RootElement.TryGetProperty("responseData", out var responseData) &&
                    responseData.TryGetProperty("translatedText", out var translatedText))
                {
                    return translatedText.GetString();
                }
            }
            catch (Exception)
            {
                return "Translation Error";
            }
            return string.Empty;
        }

        private Color GetRandomColor()
        {
            Color[] palette =
            {
                Color.FromArgb("#E6E6FA"), 
                Color.FromArgb("#AFEEEE"), 
                Color.FromArgb("#FAFAD2"), 
                Color.FromArgb("#F5FFFA") 
            };

            return palette[_random.Next(palette.Length)];
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


        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

