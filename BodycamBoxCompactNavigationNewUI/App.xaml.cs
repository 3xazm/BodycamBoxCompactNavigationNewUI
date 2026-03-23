using BodycamBoxCompactNavigationNewUI.Services;
using BodycamBoxCompactNavigationNewUI.ViewModels.Pages;
using BodycamBoxCompactNavigationNewUI.ViewModels.Windows;
using BodycamBoxCompactNavigationNewUI.Views.Pages;
using BodycamBoxCompactNavigationNewUI.Views.Windows;
using BodycamBoxCompactNavigationNewUI.WebViews.Bodycambxscn.Pages;
using BodycamBoxCompactNavigationNewUI.WebViews.HanserCommunity.Pages;
using BodycamBoxCompactNavigationNewUI.WebViews.AruiCommunity.Pages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Reflection;
using System.Windows.Threading;
using Wpf.Ui;
using Wpf.Ui.DependencyInjection;

namespace BodycamBoxCompactNavigationNewUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        // The.NET Generic Host provides dependency injection, configuration, logging, and other services.
        // https://docs.microsoft.com/dotnet/core/extensions/generic-host
        // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
        // https://docs.microsoft.com/dotnet/core/extensions/configuration
        // https://docs.microsoft.com/dotnet/core/extensions/logging
        private static readonly IHost _host = Host
            .CreateDefaultBuilder()
            .ConfigureAppConfiguration(c => { c.SetBasePath(Path.GetDirectoryName(AppContext.BaseDirectory)); })
            .ConfigureServices((context, services) =>
            {
                services.AddNavigationViewPageProvider();

                services.AddHostedService<ApplicationHostService>();

                // Theme manipulation
                services.AddSingleton<IThemeService, ThemeService>();

                // TaskBar manipulation
                services.AddSingleton<ITaskBarService, TaskBarService>();

                // Service containing navigation, same as INavigationWindow... but without window
                services.AddSingleton<INavigationService, NavigationService>();

                // Main window with navigation
                services.AddSingleton<INavigationWindow, MainWindow>();
                services.AddSingleton<MainWindowViewModel>();

                services.AddSingleton<DashboardPage>();
                services.AddSingleton<DashboardViewModel>();

                //一删掉的DataPage和DataViewModel
                //services.AddSingleton<DataPage>();
                //services.AddSingleton<DataViewModel>();

                services.AddSingleton<ResolutionPage>();
                services.AddSingleton<ResolutionViewModel>();
                services.AddSingleton<BackupPage>();
                services.AddSingleton<BackupViewModel>();
                services.AddSingleton<LocalizationPage>();
                services.AddSingleton<LocalizationViewModel>();

                // 👇 你新加的页面（必须注册！）
                services.AddSingleton<MatchDataPage>();

                // WebView.Bodycambxscn
                services.AddSingleton<Web1Page>();
                services.AddSingleton<Web2Page>();
                //WebView.HanserCommunity
                services.AddSingleton<Hanser1Page>();
                services.AddSingleton<Hanser2Page>();
                services.AddSingleton<Hanser3Page>();
                //WebView.AruiCommunity
                services.AddSingleton<Arui1Page>();
                services.AddSingleton<Arui2Page>();

                //设置
                services.AddSingleton<SettingsPage>();
                services.AddSingleton<SettingsViewModel>();
            }).Build();

        /// <summary>
        /// Gets services.
        /// </summary>
        public static IServiceProvider Services
        {
            get { return _host.Services; }
        }

        /// <summary>
        /// Occurs when the application is loading.
        /// </summary>
        private async void OnStartup(object sender, StartupEventArgs e)
        {
            await _host.StartAsync();
        }

        /// <summary>
        /// Occurs when the application is closing.
        /// </summary>
        private async void OnExit(object sender, ExitEventArgs e)
        {
            await _host.StopAsync();

            _host.Dispose();
        }

        /// <summary>
        /// Occurs when an exception is thrown by an application but not handled.
        /// </summary>
        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // For more info see https://docs.microsoft.com/en-us/dotnet/api/system.windows.application.dispatcherunhandledexception?view=windowsdesktop-6.0
        }
    }
}
