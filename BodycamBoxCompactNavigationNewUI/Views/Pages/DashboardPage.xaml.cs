using BodycamBoxCompactNavigationNewUI.ViewModels.Pages;
using BodycamBoxCompactNavigationNewUI.Views.Windows;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Wpf.Ui.Abstractions.Controls;
using Wpf.Ui.Controls;

namespace BodycamBoxCompactNavigationNewUI.Views.Pages
{
    public partial class DashboardPage : INavigableView<DashboardViewModel>
    {
        public DashboardViewModel ViewModel { get; }

        public DashboardPage(DashboardViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();

            Loaded += DashboardPage_Loaded;
        }

        private void DashboardPage_Loaded(object sender, RoutedEventArgs e)
        {
            // ✅ 1. 图片圆角（你原本的功能）
            double radius = 20;

            MyImage.Clip = new RectangleGeometry(
                new Rect(0, 0, MyImage.ActualWidth, MyImage.ActualHeight),
                radius,
                radius
            );

            // ✅ 2. 找到 NavigationView（侧边栏）
            var nav = FindParent<NavigationView>(this);

            if (nav != null)
            {
                nav.PaneOpened += OnPaneChanged;
                nav.PaneClosed += OnPaneChanged;

                // 初始化一次
                OnPaneChanged(nav, EventArgs.Empty);
            }
        }

        // ✅ 3. 侧边栏展开/收起时触发
        private void OnPaneChanged(object sender, EventArgs e)
        {
            if (sender is NavigationView nav)
            {
                BannerGrid.Margin = nav.IsPaneOpen
                    ? new Thickness(50, 10, 0, 0)  //展开 默认
                    : new Thickness(125, 10, 0, 0); //收起 增加左边距
            }
        }

        private void DashboardScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0)
            {
                DashboardScrollViewer.LineDown();
            }
            else
            {
                DashboardScrollViewer.LineUp();
            }

            e.Handled = true;
        }

        // ✅ 4. 查找父控件（关键方法）
        private T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);

            while (parent != null && !(parent is T))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            return parent as T;
        }

        private void GoResolutionPage(object sender, MouseButtonEventArgs e)
        {
            var window = (MainWindow)Application.Current.MainWindow;
            window.Navigate(typeof(Views.Pages.ResolutionPage));
        }

        private void GoBackupPage(object sender, MouseButtonEventArgs e)
        {
            var window = (MainWindow)Application.Current.MainWindow;
            window.Navigate(typeof(Views.Pages.BackupPage));
        }

        private void GoLocalizationPage(object sender, MouseButtonEventArgs e)
        {
            var window = (MainWindow)Application.Current.MainWindow;
            window.Navigate(typeof(Views.Pages.LocalizationPage));
        }
        private void GoBodycamOptimizationPage(object sender, MouseButtonEventArgs e)
        {
            var window = (MainWindow)Application.Current.MainWindow;
            window.Navigate(typeof(Views.Pages.BodycamOptimizationPage));
        }

    }
}
