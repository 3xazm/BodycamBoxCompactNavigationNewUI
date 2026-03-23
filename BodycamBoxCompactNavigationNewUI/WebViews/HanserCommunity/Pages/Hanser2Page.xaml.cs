using System;
using System.Windows.Controls;

namespace BodycamBoxCompactNavigationNewUI.WebViews.HanserCommunity.Pages
{
    public partial class Hanser2Page : Page
    {
        public Hanser2Page()
        {
            InitializeComponent();
            Loaded += Hanser2Page_Loaded;
        }

        private async void Hanser2Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            await WebView2.EnsureCoreWebView2Async(null);

            // 只打开首页，后面全部交给网页
            WebView2.Source = new Uri("https://space.bilibili.com/13937168");
        }
    }
}