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
            // Add more words as needed
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
            //Ensure that the key and value are properly paired
            var keys = new List<string>(WordTranslations.Keys);
            var randomKey = keys[random.Next(keys.Count)];
            currentWord = new KeyValuePair<string, string>(randomKey, WordTranslations[randomKey]);

            WordLabel.Text = currentWord.Key; //Display the word in the Nordic language
            FeedbackLabel.IsVisible = false;
            UserEntry.Text = string.Empty;
            NextWordButton.IsEnabled = false;
        }

        private void OnSubmitClicked(object sender, EventArgs e)
        {
            var userInput = UserEntry.Text?.Trim();

            if (string.IsNullOrEmpty(userInput))
            {
                FeedbackLabel.Text = "Please enter a translation.";
                FeedbackLabel.TextColor = Colors.Red;
                FeedbackLabel.IsVisible = true;
                return;
            }

            if (string.Equals(userInput, currentWord.Value, StringComparison.OrdinalIgnoreCase))
            {
                FeedbackLabel.Text = "Correct!";
                FeedbackLabel.TextColor = Colors.Green;
                FeedbackLabel.IsVisible = true;
                NextWordButton.IsEnabled = true;
            }
            else
            {
                FeedbackLabel.Text = $"Incorrect. The correct translation is: {currentWord.Value}.";
                FeedbackLabel.TextColor = Colors.Red;
                FeedbackLabel.IsVisible = true;
            }
        }

        private void OnNextWordClicked(object sender, EventArgs e)
        {
            LoadNextWord();
        }
    }
}
