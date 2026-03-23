using System;
using System.Windows.Controls;

namespace BodycamBoxCompactNavigationNewUI.WebViews.AruiCommunity.Pages
{
    public partial class Arui1Page : Page
    {
        public Arui1Page()
        {
            InitializeComponent();
            Loaded += Arui1Page_Loaded;
        }

        private async void Arui1Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            await WebView1.EnsureCoreWebView2Async(null);

            // 只打开首页，后面全部交给网页
            WebView1.Source = new Uri("https://space.bilibili.com/53770129?spm_id_from=333.1387.follow.user_card.click");
        }
    }
}