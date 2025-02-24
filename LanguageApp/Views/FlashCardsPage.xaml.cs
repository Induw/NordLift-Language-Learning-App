using System.Net.Http;
using System.Text.Json;
using Microsoft.Maui.Controls;

namespace LanguageApp.Views
{
    public partial class FlashCardsPage : ContentPage
    {
        private string currentLanguage = Preferences.Get("SelectedLanguage", "sv");
        private int currentWordIndex = 0;
        private readonly Random random = new();
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

        public FlashCardsPage()
        {
            InitializeComponent();
            LoadNextFlashcard();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Title = $"Flashcards in - {GetLanguageFullName(currentLanguage)}";
        }

        private async void OnTranslateClicked(object sender, EventArgs e)
        {
            var word = words[currentWordIndex];
            
            var translation = await GetTranslationAsync(word, "en", currentLanguage); 
            if (string.IsNullOrEmpty(translation))
            {
                await DisplayAlert("Error", "Failed to fetch translation.", "OK");
                return;
            }

            await RotateFlashcard(word, translation);
        }

        private void OnNextFlashcardClicked(object sender, EventArgs e)
        {
            currentWordIndex = random.Next(words.Length);
            LoadNextFlashcard();

            FlashcardFrame.BackgroundColor = GetRandomColor();
        }

        private void LoadNextFlashcard()
        {
            FlashcardLabel.Text = words[currentWordIndex];
        }

        private async Task RotateFlashcard(string word, string translation)
        {
            await FlashcardFrame.RotateYTo(90, 250);

            FlashcardLabel.Text = translation; 
            FlashcardLabel.TextColor = Colors.Black;

            await FlashcardFrame.RotateYTo(180, 250);

            FlashcardFrame.RotationY = 0;
        }
        private Color GetRandomColor()
        {
            Color[] palette =
            [
                Color.FromArgb("#E6E6FA"), // Lavender
                Color.FromArgb("#AFEEEE"), // Pale Turquoise
                Color.FromArgb("#FAFAD2"), // Light Goldenrod Yellow
                Color.FromArgb("#F5FFFA")  // Mint Cream
            ];

            return palette[random.Next(palette.Length)];
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
