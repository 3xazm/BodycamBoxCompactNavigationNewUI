using System;
using System.Windows.Controls;

namespace BodycamBoxCompactNavigationNewUI.WebViews.Bodycambxscn.Pages
{
    public partial class Web2Page : Page
    {
        public Web2Page()
        {
            InitializeComponent();
            Loaded += Web2Page_Loaded;
        }

        private async void Web2Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            await WebView2.EnsureCoreWebView2Async(null);

            // 只打开首页，后面全部交给网页
            WebView2.Source = new Uri("https://bodycam.bxs.cn");
        }
    }
}