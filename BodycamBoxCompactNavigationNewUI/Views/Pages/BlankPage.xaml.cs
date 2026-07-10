using BodycamBoxCompactNavigationNewUI.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using Wpf.Ui.Abstractions.Controls;

namespace BodycamBoxCompactNavigationNewUI.Views.Pages
{
    public partial class BlankPage : INavigableView<BlankViewModel>
    {
        public BlankViewModel ViewModel { get; }
        public BlankPage(BlankViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
            DataContext = this;
        }
    }
}