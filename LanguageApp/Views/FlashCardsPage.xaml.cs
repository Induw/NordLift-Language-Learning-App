using LanguageApp.ViewModels;
using LanguageApp.Services;
namespace LanguageApp.Views
{
    public partial class FlashCardsPage : ContentPage
    {
        public FlashCardsPage(ITranslationService translationService)
        {
            InitializeComponent();
            var viewModel = new FlashCardsPageViewModel(translationService);
            BindingContext = viewModel;
            viewModel.OnFlashcardFlipped += async (sender, _) => await RotateFlashcard();
        }

        private async Task RotateFlashcard()
        {
            await FlashcardFrame.RotateYTo(90, 250);
            await FlashcardFrame.RotateYTo(180, 250);
            FlashcardFrame.RotationY = 0;
        }

    }
}
