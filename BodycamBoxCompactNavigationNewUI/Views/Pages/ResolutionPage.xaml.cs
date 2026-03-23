using BodycamBoxCompactNavigationNewUI.ViewModels.Pages;
using Wpf.Ui.Abstractions.Controls;

namespace BodycamBoxCompactNavigationNewUI.Views.Pages
{
    public partial class ResolutionPage : INavigableView<ResolutionViewModel>
    {
        public ResolutionViewModel ViewModel { get; }

        public ResolutionPage(ResolutionViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }
    }
}