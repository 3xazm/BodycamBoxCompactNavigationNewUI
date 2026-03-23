using System;
using System.Windows.Controls;

namespace BodycamBoxCompactNavigationNewUI.WebViews.Bodycambxscn.Pages
{
    public partial class Web1Page : Page
    {
        public Web1Page()
        {
            InitializeComponent();
            Loaded += Web1Page_Loaded;
        }

        private async void Web1Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            await WebView1.EnsureCoreWebView2Async(null);

            // 只打开首页，后面全部交给网页
            WebView1.Source = new Uri("https://bodycam.bxs.cn");
        }
    }
}