using System.Net.Http;
using System.Text.Json;
using Microsoft.Maui.Controls;

namespace LanguageApp.Views
{
    public partial class FlashCardsPage : ContentPage
    {
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


        private int currentWordIndex = 0;
        private readonly Random random = new();

        public FlashCardsPage()
        {
            InitializeComponent();
            LoadNextFlashcard();
        }

        private async void OnTranslateClicked(object sender, EventArgs e)
        {
            var word = words[currentWordIndex];

            var translation = await GetTranslationAsync(word, "en", "sv"); //English to Swedish
            if (string.IsNullOrEmpty(translation))
            {
                await DisplayAlert("Error", "Failed to fetch translation.", "OK");
                return;
            }

            await RotateFlashcard(word, translation);
        }

        private void OnNextFlashcardClicked(object sender, EventArgs e)
        {
            currentWordIndex = (currentWordIndex + 1) % words.Length;
            LoadNextFlashcard();

            // Change flashcard color
            FlashcardFrame.BackgroundColor = GetRandomColor();
        }

        private void LoadNextFlashcard()
        {
            FlashcardLabel.Text = words[currentWordIndex];
        }

        private async Task RotateFlashcard(string word, string translation)
        {
            // Rotate out (front side)
            await FlashcardFrame.RotateYTo(90, 250);
            FlashcardLabel.Text = translation; // Show translation
            FlashcardLabel.TextColor = Colors.Black;

            // Rotate back in (back side)
            await FlashcardFrame.RotateYTo(0, 250);
        }
        private Color GetRandomColor()
        {
            // Define a palette of colors that match the CornflowerBlue aesthetic.
            Color[] palette =
            [
            Color.FromRgb(230, 230, 250),  //Lavender (#E6E6FA)
            Color.FromRgb(175, 238, 238),  //Pale Turquoise (#AFEEEE)
            Color.FromRgb(250, 250, 210),  //Light Goldenrod Yellow (#FAFAD2)
            Color.FromRgb(245, 255, 250)   //Mint Cream (#F5FFFA)
            ];

            return palette[random.Next(palette.Length)];
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
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }

            return string.Empty;
        }
    }
}
