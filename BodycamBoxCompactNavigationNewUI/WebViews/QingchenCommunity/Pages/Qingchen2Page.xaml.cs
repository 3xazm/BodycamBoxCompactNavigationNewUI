using System;
using System.Windows.Controls;

namespace BodycamBoxCompactNavigationNewUI.WebViews.QingchenCommunity.Pages
{
    public partial class Qingchen2Page : Page
    {
        public Qingchen2Page()
        {
            InitializeComponent();
            Loaded += Qingchen2Page_Loaded;
        }

        private async void Qingchen2Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            await WebView1.EnsureCoreWebView2Async(null);

            // 只打开首页，后面全部交给网页
            WebView1.Source = new Uri("https://www.bilibili.com/video/BV1vkjs6BEed/?spm_id_from=333.1387.homepage.video_card.click&vd_source=6728ed198227d53a4d99117fbf886ea1");
        }
    }
}