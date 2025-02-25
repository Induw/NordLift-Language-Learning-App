using LanguageApp.ViewModels;

namespace LanguageApp.Views
{
    public partial class FlashCardsPage : ContentPage
    {
        public FlashCardsPage()
        {
            InitializeComponent();

            if (BindingContext is FlashCardsPageViewModel viewModel)
            {
                viewModel.OnFlashcardFlipped += async (sender, _) =>
                {
                    await RotateFlashcard();
                };
            }
        }

        private async Task RotateFlashcard()
        {
            await FlashcardFrame.RotateYTo(90, 250);
            await FlashcardFrame.RotateYTo(180, 250);
            FlashcardFrame.RotationY = 0;
        }

    }
}
