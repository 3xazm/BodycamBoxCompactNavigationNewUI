using System;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Media; // 🌟 核心引入：需要用到 Color 和 SolidColorBrush
using BodycamBoxCompactNavigationNewUI.Views.Windows;
using Wpf.Ui.Abstractions.Controls;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace BodycamBoxCompactNavigationNewUI.ViewModels.Pages
{
    /// <summary>
    /// 自定义 4 种独立的主题与渲染模式枚举
    /// 完美对应前端 XAML 中的 ConverterParameter (Light, Dark, AcrylicLight, AcrylicDark)
    /// </summary>
    public enum CustomTheme
    {
        Light,
        Dark,
        AcrylicLight,
        AcrylicDark
    }

    public partial class SettingsViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        [ObservableProperty]
        private string _appVersion = String.Empty;

        // 🌟 将原本的 ApplicationTheme 升级为我们的自定义四态枚举
        [ObservableProperty]
        private CustomTheme _currentTheme = CustomTheme.AcrylicDark; // 默认给个初始值

        public Task OnNavigatedToAsync()
        {
            if (!_isInitialized)
                InitializeViewModel();
            else
                RefreshCurrentThemeState(); // 每次切回页面时，重新刷新勾选状态

            return Task.CompletedTask;
        }

        public Task OnNavigatedFromAsync() => Task.CompletedTask;

        private void InitializeViewModel()
        {
            AppVersion = $"UiDesktopApp1 - {GetAssemblyVersion()}";
            RefreshCurrentThemeState();
            _isInitialized = true;
        }

        /// <summary>
        /// 核心读取逻辑：通过“主色调 + 窗口底色”逆向判断当前处于 4 种模式中的哪一种
        /// </summary>
        private void RefreshCurrentThemeState()
        {
            var window = Application.Current.MainWindow as MainWindow;
            var sysTheme = ApplicationThemeManager.GetAppTheme();

            if (window != null)
            {
                // 如果窗口当前的背景渲染是 Acrylic (液态玻璃)
                if (window.WindowBackdropType == WindowBackdropType.Acrylic)
                {
                    CurrentTheme = sysTheme == ApplicationTheme.Light
                        ? CustomTheme.AcrylicLight
                        : CustomTheme.AcrylicDark;
                }
                else // 普通标准模式
                {
                    CurrentTheme = sysTheme == ApplicationTheme.Light
                        ? CustomTheme.Light
                        : CustomTheme.Dark;
                }
            }
            else
            {
                // 异常回退兼容
                CurrentTheme = sysTheme == ApplicationTheme.Light ? CustomTheme.Light : CustomTheme.Dark;
            }
        }

        private string GetAssemblyVersion()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString()
                ?? String.Empty;
        }

        /// <summary>
        /// 核心一键切换逻辑：完美掌控 4 种独立单选按钮
        /// </summary>
        [RelayCommand]
        private void OnChangeTheme(string parameter)
        {
            var window = Application.Current.MainWindow as MainWindow;
            if (window == null) return;

            switch (parameter)
            {
                case "theme_light":
                    // 1. 标准浅色模式：显式传入 Mica，切浅色 + 还原普通底色 (Mica)
                    ApplicationThemeManager.Apply(ApplicationTheme.Light, WindowBackdropType.Mica);
                    window.WindowBackdropType = WindowBackdropType  .Mica;
                    CurrentTheme = CustomTheme.Light;
                    break;

                case "theme_dark":
                    // 2. 标准深色模式：显式传入 Mica，切深色 + 还原普通底色 (Mica)
                    ApplicationThemeManager.Apply(ApplicationTheme.Dark, WindowBackdropType.Mica);
                    window.WindowBackdropType = WindowBackdropType.Mica;
                    CurrentTheme = CustomTheme.Dark;
                    break;

                case "theme_acrylic_light":
                    // 3. Acrylic浅色：【核心修复】显式传入 Acrylic，切浅色 + 强行注入液态玻璃特效画刷
                    ApplicationThemeManager.Apply(ApplicationTheme.Light, WindowBackdropType.Acrylic);
                    window.WindowBackdropType = WindowBackdropType.Acrylic;
                    CurrentTheme = CustomTheme.AcrylicLight;
                    break;

                case "theme_acrylic_dark":
                    // 4. Acrylic深色：【核心修复】显式传入 Acrylic，切深色 + 强行注入液态玻璃特效画刷
                    ApplicationThemeManager.Apply(ApplicationTheme.Dark, WindowBackdropType.Acrylic);
                    window.WindowBackdropType = WindowBackdropType.Acrylic;
                    CurrentTheme = CustomTheme.AcrylicDark;
                    break;
            }
        }
    }
}