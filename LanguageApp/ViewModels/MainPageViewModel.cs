using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LanguageApp.Views;

namespace LanguageApp.ViewModels
{
    public class MainPageViewModel : ObservableObject
    {
        public ICommand NavigateToFlashcardsCommand { get; }
        public ICommand NavigateToChallengesCommand { get; }
        public ICommand NavigateToProgressCommand { get; }

        public MainPageViewModel()
        {
            NavigateToFlashcardsCommand = new RelayCommand(OnNavigateToFlashcards);
            NavigateToChallengesCommand = new RelayCommand(OnNavigateToChallenges);
            NavigateToProgressCommand = new RelayCommand(OnNavigateToProgress);
        }

        private async void OnNavigateToFlashcards()
        {
            await Shell.Current.GoToAsync(nameof(FlashCardsPage));
        }

        private async void OnNavigateToChallenges()
        {
            await Shell.Current.GoToAsync(nameof(ChallengesPage));
        }

        private async void OnNavigateToProgress()
        {
            await Shell.Current.GoToAsync(nameof(ProgressPage));
        }
    }
}

