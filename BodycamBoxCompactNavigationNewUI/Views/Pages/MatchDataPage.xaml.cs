using System;
using System.Windows.Controls;

namespace BodycamBoxCompactNavigationNewUI.Views.Pages
{
    public partial class MatchDataPage : Page
    {
        public MatchDataPage()
        {
            InitializeComponent();
            Loaded += MatchDataPage_Loaded;
        }

        private async void MatchDataPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            await WebViewTest.EnsureCoreWebView2Async(null);

            // 只打开首页，后面全部交给网页
            WebViewTest.Source = new Uri("https://bodycam.bxs.cn");
        }
    }
}