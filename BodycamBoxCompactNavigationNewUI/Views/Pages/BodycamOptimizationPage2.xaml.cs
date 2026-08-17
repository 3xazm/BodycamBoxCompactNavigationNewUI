using BodycamBoxCompactNavigationNewUI.ViewModels.Pages;
using System.Windows.Controls;
using Wpf.Ui.Abstractions.Controls;
using BCmodelRefres2 = BodycamBoxCompactNavigationNewUI.ViewModels.Pages;

namespace BodycamBoxCompactNavigationNewUI.Views.Pages
{
    public partial class BodycamOptimizationPage2 : Page, INavigableView<BodycamOptimizationViewModel2>
    {
        public BodycamOptimizationViewModel2 ViewModel { get; }

        public BodycamOptimizationPage2(BodycamOptimizationViewModel2 viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent(); // 负责加载前端 XAML 界面
            DataContext = this;    // 配合前端的 {Binding ViewModel.XXX} 语法

            this.Loaded += (s, e) =>
            {
                BCmodelRefres2.BodycamOptimizationViewModel2.Current?.Refresh();
                BCmodelRefres2.BodycamOptimizationViewModel2.Current?.InitializeLuaPath();
                BCmodelRefres2.BodycamOptimizationViewModel2.Current?.LoadAllStatuses();
                ViewModel.Refresh();
            };
        }
    }
}