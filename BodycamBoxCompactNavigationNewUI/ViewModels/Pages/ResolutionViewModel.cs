using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.IO;
using System.Runtime.InteropServices;

namespace BodycamBoxCompactNavigationNewUI.ViewModels.Pages
{
    public partial class ResolutionViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _currentResolution;

        public ResolutionViewModel()
        {
            LoadResolution();
        }

        // ✅ Win32 获取真实分辨率（不受缩放影响）
        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(int nIndex);

        private (int width, int height) GetRealResolution()
        {
            const int SM_CXSCREEN = 0;
            const int SM_CYSCREEN = 1;

            int width = GetSystemMetrics(SM_CXSCREEN);
            int height = GetSystemMetrics(SM_CYSCREEN);

            return (width, height);
        }

        private void LoadResolution()
        {
            var (width, height) = GetRealResolution();
            CurrentResolution = $"当前你的系统分辨率: {width} x {height}";
        }

        [RelayCommand]
        private void FixResolution()
        {
            try
            {
                var processes = Process.GetProcessesByName("Bodycam");

                if (processes.Length > 0)
                {
                    System.Windows.MessageBox.Show("检测到Bodycam正在运行，请先关闭！", "Bodycam 工具箱", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // ✅ 使用真实分辨率
                var (width, height) = GetRealResolution();

                string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

                string iniPath = Path.Combine(localAppData, @"Bodycam\Saved\Config\Windows\GameUserSettings.ini");
                string jsonPath = Path.Combine(localAppData, @"Bodycam\Saved\SaveGames\SystemConfig.json");

                // ===== 修改 ini =====
                if (File.Exists(iniPath))
                {
                    string text = File.ReadAllText(iniPath);

                    text = Regex.Replace(text, @"ResolutionSizeX=\d+", $"ResolutionSizeX={width}");
                    text = Regex.Replace(text, @"ResolutionSizeY=\d+", $"ResolutionSizeY={height}");
                    text = Regex.Replace(text, @"LastUserConfirmedResolutionSizeX=\d+", $"LastUserConfirmedResolutionSizeX={width}");
                    text = Regex.Replace(text, @"LastUserConfirmedResolutionSizeY=\d+", $"LastUserConfirmedResolutionSizeY={height}");
                    text = Regex.Replace(text, @"DesiredScreenWidth=\d+", $"DesiredScreenWidth={width}");
                    text = Regex.Replace(text, @"DesiredScreenHeight=\d+", $"DesiredScreenHeight={height}");
                    text = Regex.Replace(text, @"LastUserConfirmedDesiredScreenWidth=\d+", $"LastUserConfirmedDesiredScreenWidth={width}");
                    text = Regex.Replace(text, @"LastUserConfirmedDesiredScreenHeight=\d+", $"LastUserConfirmedDesiredScreenHeight={height}");

                    File.WriteAllText(iniPath, text);
                }

                // ===== 修改 JSON =====
                if (File.Exists(jsonPath))
                {
                    string jsonText = File.ReadAllText(jsonPath);

                    jsonText = Regex.Replace(jsonText,
                        @"""DisplayResolution"":""\d+\s*x\s*\d+""",
                        $"\"DisplayResolution\":\"{width} x {height}\"");

                    File.WriteAllText(jsonPath, jsonText);
                }
                Thread.Sleep(1000); // 延迟 1000 毫秒 = 1 秒
                System.Windows.MessageBox.Show("分辨率修复成功！", "Bodycam 工具箱", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                Thread.Sleep(500); // 延迟 500 毫秒 = 0.5 秒
                System.Windows.MessageBox.Show("发生错误：" + ex.Message, "Bodycam 工具箱", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        [RelayCommand]
        private void FixBlackScreen()
        {
            try
            {
                string exePath = @"C:\Program Files (x86)\Steam\steamapps\common\Bodycam\Bodycam.exe";

                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(
                    @"Software\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers", true))
                {
                    if (key == null)
                    {
                        MessageBox.Show("无法打开注册表！");
                        return;
                    }

                    key.SetValue(exePath, "~ DISABLEDXMAXIMIZEDWINDOWEDMODE");
                }
                Thread.Sleep(1000); // 延迟 1000 毫秒 = 1 秒
                System.Windows.MessageBox.Show("黑屏修复成功！", "Bodycam 工具箱", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                Thread.Sleep(500); // 延迟 500 毫秒 = 0.5 秒
                System.Windows.MessageBox.Show("发生错误：" + ex.Message, "Bodycam 工具箱", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}