namespace LanguageApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Controls;

public partial class PairsPage : ContentPage
{
    private Dictionary<string, string> WordPairs = new()
        {
            { "Hej", "Hello" }, { "Ja", "Yes" }, { "Nej", "No" }, { "Tack", "Thank you" },
            { "Förlåt", "Sorry" }, { "God morgon", "Good morning" }, { "God natt", "Good night" },
            { "Hej då", "Goodbye" }, { "Vän", "Friend" }, { "Familj", "Family" },
            { "Kärlek", "Love" }, { "Mat", "Food" }, { "Vatten", "Water" }, { "Bil", "Car" },
            { "Hus", "House" }
        };

    private Dictionary<Button, string> ButtonPairs = new();
    private Button SelectedLeftButton = null;
    private Button SelectedRightButton = null;

    public PairsPage()
    {
        InitializeComponent();
        LoadNewPairs();
    }

    private void LoadNewPairs()
    {
        PairsGrid.Children.Clear();
        ButtonPairs.Clear();
        ErrorMessage.IsVisible = false;

        var randomPairs = WordPairs.OrderBy(x => Guid.NewGuid()).Take(5).ToList();
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
        var button = new Button
        {
            Text = text,
            BackgroundColor = Colors.White,
            TextColor = Colors.Black,
            BorderColor = Colors.Gray,
            BorderWidth = 1,
            CornerRadius = 10,
            Padding = new Thickness(10),
            WidthRequest = 120
        };

        button.Clicked += (sender, args) => HandleSelection((Button)sender, isLeftColumn);
        ButtonPairs[button] = text;
        return button;
    }

    private void HandleSelection(Button button, bool isLeftColumn)
    {
        if (isLeftColumn)
        {
            SelectedLeftButton = button;
        }
        else
        {
            SelectedRightButton = button;
        }
        button.BorderColor = Colors.CornflowerBlue;
        button.TextColor = Colors.DarkBlue;
        button.FontAttributes = FontAttributes.Bold;
        if (SelectedLeftButton != null && SelectedRightButton != null)
        {
            ValidatePair();
        }
    }

    private void ValidatePair()
    {
        string leftText = ButtonPairs[SelectedLeftButton];
        string rightText = ButtonPairs[SelectedRightButton];

        if (WordPairs.ContainsKey(leftText) && WordPairs[leftText] == rightText)
        {
            PairsGrid.Remove(SelectedLeftButton);
            PairsGrid.Remove(SelectedRightButton);
            SelectedLeftButton = null;
            SelectedRightButton = null;

            if (!PairsGrid.Children.Any())
            {
                LoadNewPairs();
            }
        }
        else
        {
            SelectedLeftButton.BorderColor = Colors.Red;
            SelectedRightButton.BorderColor = Colors.Red;
            SelectedLeftButton.WidthRequest = 120 ;
            SelectedRightButton.WidthRequest = 120;
            ErrorMessage.Text = "Incorrect! Try again.";
            ErrorMessage.IsVisible = true;

            this.Dispatcher.DispatchDelayed(TimeSpan.FromSeconds(1), () =>
            {
                ResetButtonStyle(SelectedLeftButton);
                ResetButtonStyle(SelectedRightButton);
                ErrorMessage.IsVisible = false;
                SelectedLeftButton = null;
                SelectedRightButton = null;
            });

        }
    }
    private void ResetButtonStyle(Button button)
    {
        if (button != null)
        {
            button.BackgroundColor = Colors.White;
            button.BorderColor = Colors.Gray;
            button.TextColor = Colors.Black;

        }
    }
}
