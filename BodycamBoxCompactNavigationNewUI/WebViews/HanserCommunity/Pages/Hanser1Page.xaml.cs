using System;
using System.Windows.Controls;

namespace BodycamBoxCompactNavigationNewUI.WebViews.HanserCommunity.Pages
{
    public partial class Hanser1Page : Page
    {
        public Hanser1Page()
        {
            InitializeComponent();
            Loaded += Hanser1Page_Loaded;
        }

        private async void Hanser1Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            await WebView1.EnsureCoreWebView2Async(null);

            // 只打开首页，后面全部交给网页
            WebView1.Source = new Uri("https://www.xiaoheihe.cn/app/user/profile/7c566a41130b");
        }
    }
}