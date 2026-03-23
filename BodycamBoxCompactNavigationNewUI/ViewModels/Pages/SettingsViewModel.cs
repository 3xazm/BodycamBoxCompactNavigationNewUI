using BodycamBoxCompactNavigationNewUI.Views.Windows;
using Wpf.Ui.Abstractions.Controls;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace BodycamBoxCompactNavigationNewUI.ViewModels.Pages
{
    public partial class SettingsViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        [ObservableProperty]
        private string _appVersion = String.Empty;

        [ObservableProperty]
        private ApplicationTheme _currentTheme = ApplicationTheme.Unknown;

        public Task OnNavigatedToAsync()
        {
            if (!_isInitialized)
                InitializeViewModel();

            return Task.CompletedTask;
        }

        public Task OnNavigatedFromAsync() => Task.CompletedTask;

        private void InitializeViewModel()
        {
            CurrentTheme = ApplicationThemeManager.GetAppTheme();
            AppVersion = $"UiDesktopApp1 - {GetAssemblyVersion()}";

            _isInitialized = true;
        }

        private string GetAssemblyVersion()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString()
                ?? String.Empty;
        }

        [RelayCommand]
        private void OnChangeTheme(string parameter)
        {
            var window = (MainWindow)Application.Current.MainWindow;

            switch (parameter)
            {
                case "theme_light":
                    ApplicationThemeManager.Apply(ApplicationTheme.Light);
                    window.WindowBackdropType = WindowBackdropType.Mica;
                    CurrentTheme = ApplicationTheme.Light;
                    break;

                case "theme_dark":
                    ApplicationThemeManager.Apply(ApplicationTheme.Dark);
                    window.WindowBackdropType = WindowBackdropType.Mica;
                    CurrentTheme = ApplicationTheme.Dark;
                    break;

                case "theme_acrylic":
                    // 不改变主题，只改背景
                    window.WindowBackdropType = WindowBackdropType.Acrylic;
                    break;
            }
        }
    }
}
