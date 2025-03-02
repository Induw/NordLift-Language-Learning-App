namespace LanguageApp.Views;

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Controls;
using LanguageApp.Services;

public partial class PairsPage : ContentPage
{
    private readonly ITranslationService _translationService;
    private Random random = new();
    private string currentLanguage = Preferences.Get("SelectedLanguage", "sv");
    private Dictionary<Button, string> ButtonPairs = new();
    private Button SelectedLeftButton = null;
    private Button SelectedRightButton = null;
    private bool isGridFrozen = false;

    private Dictionary<string, string> SwedishWordPairs = new()
    {
        { "Hej", "Hello" }, { "Ja", "Yes" }, { "Nej", "No" }, { "Tack", "Thank you" },
        { "Förlåt", "Sorry" }, { "God morgon", "Good morning" }, { "God natt", "Good night" },
        { "Hej då", "Goodbye" }, { "Vän", "Friend" }, { "Familj", "Family" },
        { "Kärlek", "Love" }, { "Mat", "Food" }, { "Vatten", "Water" }, { "Bil", "Car" },
        { "Hus", "House" }
    };
    private Dictionary<string, string> NorwegianWordPairs = new()
    {
        { "Hei", "Hello" }, { "Ja", "Yes" }, { "Nei", "No" }, { "Takk", "Thank you" },
        { "Unnskyld", "Sorry" }, { "God morgen", "Good morning" }, { "God natt", "Good night" },
        { "Ha det", "Goodbye" }, { "Venn", "Friend" }, { "Familie", "Family" },
        { "Kjærlighet", "Love" }, { "Mat", "Food" }, { "Vann", "Water" }, { "Bil", "Car" },
        { "Hus", "House" }
    };
    private Dictionary<string, string> DanishWordPairs = new()
    {
        { "Hej", "Hello" }, { "Ja", "Yes" }, { "Nej", "No" }, { "Tak", "Thank you" },
        { "Undskyld", "Sorry" }, { "God morgen", "Good morning" }, { "God nat", "Good night" },
        { "Farvel", "Goodbye" }, { "Ven", "Friend" }, { "Familie", "Family" },
        { "Kærlighed", "Love" }, { "Mad", "Food" }, { "Vand", "Water" }, { "Bil", "Car" },
        { "Hus", "House" }
    };
    private Dictionary<string, string> FinnishWordPairs = new()
    {
        { "Hei", "Hello" }, { "Kyllä", "Yes" }, { "Ei", "No" }, { "Kiitos", "Thank you" },
        { "Anteeksi", "Sorry" }, { "Hyvää huomenta", "Good morning" }, { "Hyvää yötä", "Good night" },
        { "Näkemiin", "Goodbye" }, { "Ystävä", "Friend" }, { "Perhe", "Family" },
        { "Rakkaus", "Love" }, { "Ruoka", "Food" }, { "Vesi", "Water" }, { "Auto", "Car" },
        { "Talo", "House" }
    };

    private Dictionary<string, string> IcelandicWordPairs = new()
    {
        { "Halló", "Hello" }, { "Já", "Yes" }, { "Nei", "No" }, { "Takk", "Thank you" },
        { "Fyrirgefðu", "Sorry" }, { "Góðan morgun", "Good morning" }, { "Góða nótt", "Good night" },
        { "Bless", "Goodbye" }, { "Vinur", "Friend" }, { "Fjölskylda", "Family" },
        { "Ást", "Love" }, { "Matur", "Food" }, { "Vatn", "Water" }, { "Bíll", "Car" },
        { "Hús", "House" }
    };

    private List<string> CorrectMessages = new()
    {
        "Great job! Keep going!", "Well done!", "Awesome!", "You're doing amazing!",
        "Keep it up!", "Nice work!", "You're on fire!"
    };

    public PairsPage(ITranslationService translationService)
    {
        InitializeComponent();
        LoadNewPairs();
        _translationService = translationService;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Title = $"Word Match : {_translationService.GetLanguageFullName(currentLanguage)}";
    }

    private void LoadNewPairs()
    {
        PairsGrid.Children.Clear();
        ButtonPairs.Clear();
        Dictionary<string, string> wordPairs = GetWordPairs(currentLanguage);
        var randomPairs = wordPairs.OrderBy(x => Guid.NewGuid()).Take(5).ToList();
        var leftWords = randomPairs.Select(p => p.Key).OrderBy(x => Guid.NewGuid()).ToList();
        var rightWords = randomPairs.Select(p => p.Value).OrderBy(x => Guid.NewGuid()).ToList();

        for (int i = 0; i < leftWords.Count; i++)
        {
            var leftButton = CreateWordButton(leftWords[i], isLeftColumn: true);
            var rightButton = CreateWordButton(rightWords[i], isLeftColumn: false);
            PairsGrid.Add(leftButton, 0, i);
            PairsGrid.Add(rightButton, 1, i);
        }
    }

    private Button CreateWordButton(string text, bool isLeftColumn)
    {
        var button = new Button { Text = text };
        ApplyDefaultStyle(button);
        button.Clicked += (sender, args) => HandleSelection((Button)sender, isLeftColumn);
        ButtonPairs[button] = text;
        return button;
    }

    private void HandleSelection(Button button, bool isLeftColumn)
    {
        if (isGridFrozen) return;

        if (isLeftColumn)
        {
            if (SelectedLeftButton != null) ResetButtonStyle(SelectedLeftButton);
            SelectedLeftButton = button;
        }
        else
        {
            if (SelectedRightButton != null) ResetButtonStyle(SelectedRightButton);
            SelectedRightButton = button;
        }
        ApplySelectedStyle(button);

        if (SelectedLeftButton != null && SelectedRightButton != null)
        {
            ValidatePair();
        }
    }

    private void ValidatePair()
    {
        isGridFrozen = true;
        string leftText = ButtonPairs[SelectedLeftButton];
        string rightText = ButtonPairs[SelectedRightButton];
        Dictionary<string, string> wordPairs = GetWordPairs(currentLanguage);

        if (wordPairs.ContainsKey(leftText) && wordPairs[leftText] == rightText)
        {
            ErrorMessage.Text = CorrectMessages[random.Next(CorrectMessages.Count)];
            ErrorMessage.TextColor = Colors.DarkGreen;

            Dispatcher.DispatchDelayed(TimeSpan.FromSeconds(0.7), () =>
            {
                PairsGrid.Remove(SelectedLeftButton);
                PairsGrid.Remove(SelectedRightButton);
                SelectedLeftButton = null;
                SelectedRightButton = null;
                ErrorMessage.Text = " ";
                isGridFrozen = false;
                if (!PairsGrid.Children.Any()) LoadNewPairs();
            });
        }
        else
        {
            ApplyErrorStyle(SelectedLeftButton);
            ApplyErrorStyle(SelectedRightButton);
            ErrorMessage.Text = "Try again! You'll get it next time!";
            ErrorMessage.TextColor = Colors.Maroon;

            Dispatcher.DispatchDelayed(TimeSpan.FromSeconds(1), () =>
            {
                ResetButtonStyle(SelectedLeftButton);
                ResetButtonStyle(SelectedRightButton);
                SelectedLeftButton = null;
                SelectedRightButton = null;
                ErrorMessage.Text = " ";
                isGridFrozen = false;
            });
        }
    }

    private void ApplyDefaultStyle(Button button)
    {
        button.Background = new LinearGradientBrush
        {
            StartPoint = new Point(0, 0),
            EndPoint = new Point(1, 1),
            GradientStops = new GradientStopCollection
            {
                new GradientStop { Color = Color.FromArgb("#E2E8F0"), Offset = 0.0f },
                new GradientStop { Color = Color.FromArgb("#CBD5E1"), Offset = 1.0f }
            }
        };
        button.TextColor = Color.FromArgb("#2D3748");
        button.FontSize = 16;
        button.CornerRadius = 12;
        button.Padding = new Thickness(12);
        button.WidthRequest = 130;
        button.BorderWidth = 0;
        button.FontAttributes = FontAttributes.None;
    }

    private void ApplySelectedStyle(Button button)
    {
        button.Background = new LinearGradientBrush
        {
            StartPoint = new Point(0, 0),
            EndPoint = new Point(1, 1),
            GradientStops = new GradientStopCollection
            {
                new GradientStop { Color = Color.FromArgb("#68D391"), Offset = 0.0f },
                new GradientStop { Color = Color.FromArgb("#38A169"), Offset = 1.0f }
            }
        };
        button.TextColor = Colors.White;
        button.FontAttributes = FontAttributes.Bold;
        button.ScaleTo(1.05, 200, Easing.CubicOut); // Scale up animation
    }

    private void ApplyErrorStyle(Button button)
    {
        button.Background = new LinearGradientBrush
        {
            StartPoint = new Point(0, 0),
            EndPoint = new Point(1, 1),
            GradientStops = new GradientStopCollection
            {
                new GradientStop { Color = Color.FromArgb("#FCA5A5"), Offset = 0.0f },
                new GradientStop { Color = Color.FromArgb("#EF4444"), Offset = 1.0f }
            }
        };
        button.TextColor = Colors.White;
        
    }

    private void ResetButtonStyle(Button button)
    {
        if (button != null)
        {
            ApplyDefaultStyle(button);
            button.Scale = 1.0; // Reset scale
        }
    }

    private Dictionary<string, string> GetWordPairs(string language)
    {
        Dictionary<string, Dictionary<string, string>> languageWordPairs = new()
        {
            { "sv", SwedishWordPairs },
            { "no", NorwegianWordPairs },
            { "fi", FinnishWordPairs },
            { "da", DanishWordPairs },
            { "is", IcelandicWordPairs }
        };
        return languageWordPairs.GetValueOrDefault(language, SwedishWordPairs);
    }

}