using System; // 🌟 补上了这个，解决 Exception、Uri 报错问题
using System.Windows;
using System.Windows.Media.Imaging;
using BodycamBoxCompactNavigationNewUI.ViewModels.Windows;
using BodycamBoxCompactNavigationNewUI.Views.Pages;
using System.Windows.Controls;
using Wpf.Ui;
using Wpf.Ui.Abstractions;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace BodycamBoxCompactNavigationNewUI.Views.Windows
{
    public partial class MainWindow : INavigationWindow
    {
        public MainWindowViewModel ViewModel { get; }

        public MainWindow(
            MainWindowViewModel viewModel,
            INavigationViewPageProvider navigationViewPageProvider,
            INavigationService navigationService
        )
        {
            ViewModel = viewModel;
            DataContext = this;

            SystemThemeWatcher.Watch(this);

            InitializeComponent();

            // 🌟 核心：当主窗口加载完毕后，用 C# 强行把导航控件内部的白墙砸碎
            this.Loaded += (s, e) =>
            {
                if (RootNavigation != null)
                {
                    // 1. 强行将组件本身的背景设为透明
                    RootNavigation.Background = Brushes.Transparent;

                    // 2. 挖掘内部真正承载 Page 的 Frame
                    // Wpf.Ui 的 NavigationView 内部使用了一个名叫 "TemplateFrame" 的 Frame 控件
                    if (RootNavigation.Template.FindName("TemplateFrame", RootNavigation) is Frame frame)
                    {
                        frame.Background = Brushes.Transparent;
                    }
                }
            };


            // 2. 重新补上这行核心监听（千万不能删！）
            // 它负责拉起全局主题钩子，让 SettingsViewModel 能够合法读取和同步主题
            Wpf.Ui.Appearance.SystemThemeWatcher.Watch(this);

            // 3. 在有了 Watch 监听之后，再安全地应用你初始化好的深色模式和 Acrylic 渲染
            Wpf.Ui.Appearance.ApplicationThemeManager.Apply(Wpf.Ui.Appearance.ApplicationTheme.Dark);


            // 在这里添加设置图标的代码         
            try
            {          
                // 1. 定义图标路径 (注意路径要和你的项目结构对应)
                // 假设你的图标放在项目根目录的 Assets 文件夹下
                Uri iconUri = new Uri("pack://application:,,,/Assets/qvertionmiku.ico", UriKind.RelativeOrAbsolute);

                // 2. 设置窗口左上角和任务栏图标
                this.Icon = BitmapFrame.Create(iconUri);

                // 3. 如果你用了 ui:TitleBar，也要给它塞一个图标
                if (TitleBar != null)
                {
                    TitleBar.Icon = new ImageIcon
                    {
                        Source = new BitmapImage(iconUri)
                    };
                }
            }                    
            catch (Exception ex)
            {
                // 如果路径错了，这里会抓住异常，保证程序起码能跑起来
                System.Diagnostics.Debug.WriteLine($"图标加载失败: {ex.Message}");
            }
               
            SetPageService(navigationViewPageProvider);

            navigationService.SetNavigationControl(RootNavigation);

        }

        #region INavigationWindow methods

        public INavigationView GetNavigation() => RootNavigation;

        public bool Navigate(Type pageType) => RootNavigation.Navigate(pageType);

        public void SetPageService(INavigationViewPageProvider navigationViewPageProvider) => RootNavigation.SetPageProviderService(navigationViewPageProvider);

        public void ShowWindow() => Show();

        public void CloseWindow() => Close();

        #endregion INavigationWindow methods

        /// <summary>
        /// Raises the closed event.
        /// </summary>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Make sure that closing this window will begin the process of closing the application.
            Application.Current.Shutdown();
        }

        INavigationView INavigationWindow.GetNavigation()
        {
            throw new NotImplementedException();
        }

        public void SetServiceProvider(IServiceProvider serviceProvider)
        {
            throw new NotImplementedException();
        }

        private void NavigationView_OnItemInvoked(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is NavigationViewItem item)
            {
                var pageType = item.TargetPageType;
                var parameter = item.Tag;

                if (pageType != null)
                {
                    RootNavigation.Navigate(pageType, parameter);
                }
            }
        }
    }
}
