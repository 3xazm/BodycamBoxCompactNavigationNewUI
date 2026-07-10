using System;
using System.Windows.Controls;

namespace BodycamBoxCompactNavigationNewUI.WebViews.QingchenCommunity.Pages
{
    public partial class Qingchen3Page : Page
    {
        public Qingchen3Page()
        {
            InitializeComponent();
            Loaded += Qingchen3Page_Loaded;
        }

        private async void Qingchen3Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            await WebView1.EnsureCoreWebView2Async(null);

            // 只打开首页，后面全部交给网页
            WebView1.Source = new Uri("https://v.douyin.com/r4zM5227iFg/ 0@0.com");
        }
    }
}