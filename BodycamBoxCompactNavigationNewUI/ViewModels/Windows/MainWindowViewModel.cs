using BodycamBoxCompactNavigationNewUI.Views.Pages;
using System.Collections.ObjectModel;
using Wpf.Ui.Controls;
using WpfMenuItem = System.Windows.Controls.MenuItem;

namespace BodycamBoxCompactNavigationNewUI.ViewModels.Windows
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _applicationTitle = "Bodycam工具箱 V2.5.2   q群1: 865048887    q群2: 470710001 ";

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
                Content = "分辨率与其他",
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
                Content = "Bodycam模式",                                                                
                Icon = new SymbolIcon { Symbol = SymbolRegular.TopSpeed24 },
                TargetPageType = typeof(Views.Pages.BodycamOptimizationPage),
                MenuItems =
                {
                    new NavigationViewItem()
                    {
                        Content = "模式添加",
                        Icon = new SymbolIcon { Symbol = SymbolRegular.AddSquareMultiple16 },
                        TargetPageType = typeof(Views.Pages.BodycamOptimizationPage)
                    },
                    new NavigationViewItem()
                    {
                        Content = "其他模式",
                        Icon = new SymbolIcon { Symbol = SymbolRegular.AppsAddIn20 },
                        TargetPageType = typeof(Views.Pages.BodycamOptimizationPage2)
                    },
                    /*
                    new NavigationViewItem()
                    {
                        Content = "如何使用？",
                        Icon = new SymbolIcon { Symbol = SymbolRegular.BookQuestionMark24 },
                        TargetPageType = typeof(Views.Pages.BodycamOptimizationPage3)
                    },    
                    new NavigationViewItem()
                    {
                        Content = "游戏问题/建议",
                        Icon = new SymbolIcon { Symbol = SymbolRegular.Attach12 },
                        TargetPageType = typeof(Views.Pages.BodycamOptimizationPage4)
                    }*/
                }
            },
            
            //分割线 1
            new NavigationViewItemSeparator(),
            new NavigationViewItem()
            {
                Content = "1.倾晨哟",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Globe24 },
                //TargetPageType = typeof(WebViews.QingchenCommunity.Pages.Qingchen1Page),
                MenuItems =
                {
                    new NavigationViewItem()
                    {
                        Content = "哔哩哔哩主页",
                        Icon = new SymbolIcon { Symbol = SymbolRegular.PersonHeart20 },
                        TargetPageType = typeof(WebViews.QingchenCommunity.Pages.Qingchen1Page),
                    },
                    new NavigationViewItem()
                    {
                        //抖音
                        Content = "抖音主页",
                        Icon = new SymbolIcon { Symbol = SymbolRegular.PersonHeart20 },
                        TargetPageType = typeof(WebViews.QingchenCommunity.Pages.Qingchen3Page),
                    },
                }
            },         
            new NavigationViewItem()
            {
                Content = "2.Hanser主包",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Globe24 },
                //TargetPageType = typeof(WebViews.HanserCommunity.Pages.Hanser1Page),
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
                Content = "3.Arui阿锐 主播",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Globe24 },
                //TargetPageType = typeof(WebViews.AruiCommunity.Pages.Arui1Page),
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

             //分割线 2
            new NavigationViewItemSeparator(),
            new NavigationViewItem()
            {
                Content = "Bodycam比赛数据",
                Icon = new SymbolIcon { Symbol = SymbolRegular.DataPie24 },
                //TargetPageType = typeof(WebViews.Bodycambxscn.Pages.Web1Page),
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

        };



        [ObservableProperty]
        private ObservableCollection<object> _footerMenuItems = new()
        {
            new NavigationViewItem()
            {
                Content = "如何使用工具箱?",
                Icon = new SymbolIcon { Symbol = SymbolRegular.CalendarInfo16 },
                TargetPageType = typeof(WebViews.QingchenCommunity.Pages.Qingchen2Page)
            },
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
