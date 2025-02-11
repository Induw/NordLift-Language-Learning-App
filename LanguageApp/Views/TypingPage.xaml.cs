using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;

namespace LanguageApp.Views
{
    public partial class TypingPage : ContentPage
    {
        private readonly Dictionary<string, string> WordTranslations = new()
        {
            { "Hej", "Hello" },
            { "Ja", "Yes" },
            { "Nej", "No" },
            { "Tack", "Thank you" },
            { "Förlåt", "Sorry" },
            { "God morgon", "Good morning" },
            { "God natt", "Good night" },
            { "Hej då", "Goodbye" }
        };

        private readonly Random random = new();
        private KeyValuePair<string, string> currentWord;

        public TypingPage()
        {
            InitializeComponent();
            LoadNextWord();
        }

        private void LoadNextWord()
        {
            
            var keys = new List<string>(WordTranslations.Keys);
            var randomKey = keys[random.Next(keys.Count)];
            currentWord = new KeyValuePair<string, string>(randomKey, WordTranslations[randomKey]);

            WordLabel.Text = currentWord.Key; 
            FeedbackLabel.IsVisible = false;
            UserEntry.Text = string.Empty;
            NextWordButton.IsEnabled = false;
            NextWordButton.Opacity =  0.5;
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
                FeedbackLabel.Text = "Correct!👍 You're killing it!🤩";
                FeedbackLabel.TextColor = Colors.Green;
                FeedbackLabel.IsVisible = true;
                NextWordButton.IsEnabled = true;
                NextWordButton.Opacity = 1;
            }
            else
            {
                FeedbackLabel.Text = $"Incorrect !👎🙄. The correct translation is: {currentWord.Value}.";
                FeedbackLabel.TextColor = Colors.Red;
                FeedbackLabel.IsVisible = true;
                NextWordButton.IsEnabled = true;
                NextWordButton.Opacity = 1;
            }
        }

        private void OnNextWordClicked(object sender, EventArgs e)
        {
            LoadNextWord();
        }
    }
}
