using BodycamBoxCompactNavigationNewUI.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using Wpf.Ui.Abstractions.Controls;

namespace BodycamBoxCompactNavigationNewUI.Views.Pages
{
    public partial class BodycamOptimizationPage4 : INavigableView<BodycamOptimizationViewModel4>
    {
        public BodycamOptimizationViewModel4 ViewModel { get; }
        public BodycamOptimizationPage4(BodycamOptimizationViewModel4 viewModel)
        {
            ViewModel = viewModel;

            // 🛠️ 帮你补齐的关键初始化操作
            InitializeComponent();
            DataContext = this;
        }
    }
}