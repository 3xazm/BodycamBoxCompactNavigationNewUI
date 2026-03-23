using System;
using System.Windows.Controls;

namespace BodycamBoxCompactNavigationNewUI.WebViews.HanserCommunity.Pages
{
    public partial class Hanser3Page : Page
    {
        public Hanser3Page()
        {
            InitializeComponent();
            Loaded += Hanser2Page_Loaded;
        }

        private async void Hanser2Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            await WebView3.EnsureCoreWebView2Async(null);

            // 只打开首页，后面全部交给网页
            WebView3.Source = new Uri("https://www.bilibili.com/video/BV1xHSNB7EhU/?spm_id_from=333.337.search-card.all.click&vd_source=3e5cd3336ed40dca41f7b1b10378383f");
        }
    }
}