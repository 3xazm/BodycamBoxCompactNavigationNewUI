using System;
using System.Windows.Controls;

namespace BodycamBoxCompactNavigationNewUI.WebViews.AruiCommunity.Pages
{
    public partial class Arui2Page : Page
    {
        public Arui2Page()
        {
            InitializeComponent();
            Loaded += Arui2Page_Loaded;
        }

        private async void Arui2Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            await WebView1.EnsureCoreWebView2Async(null);

            // 只打开首页，后面全部交给网页
            WebView1.Source = new Uri("https://www.bilibili.com/video/BV1Pfctz6Ena/?spm_id_from=333.1387.homepage.video_card.click&vd_source=6728ed198227d53a4d99117fbf886ea1");
        }
    }
}