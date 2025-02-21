using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;

namespace LanguageApp.Views
{
    public partial class TypingPage : ContentPage
    {
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

        private readonly Random random = new();
        private KeyValuePair<string, string> currentWord;

        public TypingPage()
        {
            InitializeComponent();
            LoadNextWord();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            var selectedLanguage = Preferences.Get("SelectedLanguage", "sv");
            Title = $"Translate and Learn in - {GetLanguageFullName(selectedLanguage)}";
        }
        private void LoadNextWord()
        {
            var currentLanguage = Preferences.Get("SelectedLanguage", "sv");

            Dictionary<string, string> wordTranslations = currentLanguage switch
            {
                "sv" => SwedishTranslations,
                "no" => NorwegianTranslations,
                "fi" => FinnishTranslations,
                "da" => DanishTranslations,
                "is" => IcelandicTranslations,
                _ => SwedishTranslations
            };

            var keys = new List<string>(wordTranslations.Keys);
            var randomKey = keys[random.Next(keys.Count)];
            currentWord = new KeyValuePair<string, string>(randomKey, wordTranslations[randomKey]);

            WordLabel.Text = currentWord.Key;
            UserEntry.Text = string.Empty;
            NextWordButton.IsEnabled = false;
            NextWordButton.Opacity = 0.5;

        }
     

        private void OnSubmitClicked(object sender, EventArgs e)
        {
            var userInput = UserEntry.Text?.Trim();

            if (string.IsNullOrEmpty(userInput))
            {
                FeedbackLabel.Text = "Come on, type something, don't worry about getting it wrong!😀";
                FeedbackLabel.TextColor = Colors.Red;
                FeedbackLabel.IsVisible = true;
                return;
            }

            if (string.Equals(userInput, currentWord.Value, StringComparison.OrdinalIgnoreCase))
            {
                FeedbackLabel.Text = "Correct!👍 Keep it Up !";
                FeedbackLabel.TextColor = Colors.Green;
                FeedbackLabel.IsVisible = true;
                NextWordButton.IsEnabled = true;
                NextWordButton.Opacity = 1;
            }
            else
            {
                FeedbackLabel.Text = $"Incorrect !👎.The correct translation is: {currentWord.Value}.";
                FeedbackLabel.TextColor = Colors.Maroon;
                FeedbackLabel.IsVisible = true;
                NextWordButton.IsEnabled = true;
                NextWordButton.Opacity = 1;
            }
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
        private void OnNextWordClicked(object sender, EventArgs e)
        {
            LoadNextWord();
            FeedbackLabel.Text = " ";
        }
    }
}
