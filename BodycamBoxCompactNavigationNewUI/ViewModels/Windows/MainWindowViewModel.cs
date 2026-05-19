using BodycamBoxCompactNavigationNewUI.Views.Pages;
using System.Collections.ObjectModel;
using Wpf.Ui.Controls;
using WpfMenuItem = System.Windows.Controls.MenuItem;

namespace BodycamBoxCompactNavigationNewUI.ViewModels.Windows
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _applicationTitle = "Bodycam工具箱 V2.4.2   [win10/11]";

        [ObservableProperty]
        private ObservableCollection<object> _menuItems = new()
        {
            
            new NavigationViewItem()
            {
                Content = "主页",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
                TargetPageType = typeof(Views.Pages.DashboardPage)
            },  
            new NavigationViewItem()
            {
                Content = "分辨率",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Desktop24 },
                TargetPageType = typeof(Views.Pages.ResolutionPage)
            },       
            new NavigationViewItem()
            {
                Content = "玩家备份",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Save24 },
                TargetPageType = typeof(Views.Pages.BackupPage)
            },
            new NavigationViewItem()
            {
                Content = "汉化包",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Translate24 },
                TargetPageType = typeof(Views.Pages.LocalizationPage)
            },
            new NavigationViewItem()
            {
                Content = "Bodycam游戏比赛数据",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Globe24 },
                TargetPageType = typeof(WebViews.Bodycambxscn.Pages.Web1Page),
                MenuItems =
                {
                new NavigationViewItem()
                {
                  Content = "主页",
                  Icon = new SymbolIcon { Symbol = SymbolRegular.HomeMore24 },
                  TargetPageType = typeof(WebViews.Bodycambxscn.Pages.Web1Page) },

                new NavigationViewItem()
                {
                    Content = "游戏比赛数据分析工具",
                    Icon = new SymbolIcon { Symbol = SymbolRegular.TableSearch20 },
                    TargetPageType = typeof(WebViews.Bodycambxscn.Pages.Web2Page) },
                }
            },
            new NavigationViewItem()
            {
                Content = "Hanser -主包",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Globe24 },
                TargetPageType = typeof(WebViews.HanserCommunity.Pages.Hanser1Page),
                MenuItems =
                {
                    new NavigationViewItem()
                    {
                      Content = "小黑盒",
                      Icon = new SymbolIcon { Symbol = SymbolRegular.PersonHeart20 },
                      TargetPageType = typeof(WebViews.HanserCommunity.Pages.Hanser1Page)
                    },
                    new NavigationViewItem()
                    {
                        Content = "BiliBili",
                        Icon = new SymbolIcon { Symbol = SymbolRegular.PersonHeart20 },
                        TargetPageType = typeof(WebViews.HanserCommunity.Pages.Hanser2Page)
                    },
                    new NavigationViewItem()
                    {
                        Content = "多人僵尸村庄速通指南",
                        Icon = new SymbolIcon { Symbol = SymbolRegular.GlobeVideo48 },
                        TargetPageType = typeof(WebViews.HanserCommunity.Pages.Hanser3Page)
                    },
                }
            },
            new NavigationViewItem()
            {
                Content = "Arui阿锐 -主播",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Globe24 },
                TargetPageType = typeof(WebViews.AruiCommunity.Pages.Arui1Page),
                MenuItems =
                {
                    new NavigationViewItem()
                    {
                      Content = "BiliBili",
                      Icon = new SymbolIcon { Symbol = SymbolRegular.PersonHeart20 },
                      TargetPageType = typeof(WebViews.AruiCommunity.Pages.Arui1Page)
                    },
                    new NavigationViewItem()
                    {
                        Content = "单人全程速通僵尸村。",
                        Icon = new SymbolIcon { Symbol = SymbolRegular.GlobeVideo32 },
                        TargetPageType = typeof(WebViews.AruiCommunity.Pages.Arui2Page) 
                    },
                }
            },
        };

        [ObservableProperty]
        private ObservableCollection<object> _footerMenuItems = new()
        {
            new NavigationViewItem()
            {
                Content = "设置",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                TargetPageType = typeof(Views.Pages.SettingsPage)
            },
        };
        
        [ObservableProperty]
        private ObservableCollection<WpfMenuItem> _trayMenuItems = new()
        {
            new WpfMenuItem { Header = "Home", Tag = "tray_home" }
        };  
    }
}
