using System;
using System.Windows.Controls;

namespace BodycamBoxCompactNavigationNewUI.WebViews.QingchenCommunity.Pages
{
    public partial class Qingchen1Page : Page
    {
        public Qingchen1Page()
        {
            InitializeComponent();
            Loaded += Qingchen1Page_Loaded;
        }

        private async void Qingchen1Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            await WebView1.EnsureCoreWebView2Async(null);

            // 只打开首页，后面全部交给网页
            WebView1.Source = new Uri("https://space.bilibili.com/219879751?spm_id_from=333.788.upinfo.head.click");
        }
    }
}