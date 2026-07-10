using BodycamBoxCompactNavigationNewUI.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using Wpf.Ui.Abstractions.Controls;

namespace BodycamBoxCompactNavigationNewUI.Views.Pages
{
    public partial class BodycamOptimizationPage3 : INavigableView<BodycamOptimizationViewModel3>
    {
        public BodycamOptimizationViewModel3 ViewModel { get; }

        public BodycamOptimizationPage3(BodycamOptimizationViewModel3 viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            DataContext = this;
        }
    }
}