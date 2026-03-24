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

        /// <summary>
        /// 功能扩展 o------------------------------------------O 
        /// </summary>
        private bool IsBodycamRunning()
        {
            Process[] processes = Process.GetProcessesByName("Bodycam");
            return processes.Length > 0;
        }

        public ResolutionViewModel()
        {
            LoadResolution();
        }

        // 功能扩展 1 的构造函数 
        string FindBodycamExe()
        {
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (!drive.IsReady) continue;

                try
                {
                    string path = Path.Combine(
                        drive.RootDirectory.FullName,
                        @"Steam\steamapps\common\Bodycam\Bodycam.exe");

                    if (File.Exists(path))
                        return path;
                }
                catch { }
            }

            return null;
        }


        /// <summary>
        /// 设计思路：直接获取系统的
        /// </summary>
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

        //分辨率修复按钮
        [RelayCommand]
        private void FixResolution()
        {
            try
            {
                var processes = Process.GetProcessesByName("Bodycam");

                if (processes.Length > 0)
                {
                    System.Windows.MessageBox.Show("检测到Bodycam正在运行，请先关闭！", "Bodycam 工具箱", 
                        MessageBoxButton.OK, 
                        MessageBoxImage.Warning);

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

        //黑屏修复按钮
        [RelayCommand]
        private void FixBlackScreen()
        {
            try
            {
                // ===== 游戏运行检测 =====
                if (IsBodycamRunning())
                {
                    MessageBox.Show(
                         "当前检测到 Bodycam 正在运行。\n修改失败。\n必须关闭游戏才能修改配置。",
                         "Bodycam 工具箱",
                         MessageBoxButton.OK,
                         MessageBoxImage.Warning
                     );
                    return;

                }

                string exePath = FindBodycamExe();

                if (exePath == null)
                {
                    System.Windows.MessageBox.Show("未找到 Bodycam.exe",
                    "Black Screen Fix",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error) ; 
                    return;
                }

                string registryPath =
                    @"Software\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers";

                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(registryPath, true))
                {
                    if (key == null)
                    {
                        System.Windows.MessageBox.Show("无法打开注册表！");
                        return;
                    }

                    // 设置为：禁用全屏优化（不设置 WIN7/WIN8）
                    key.SetValue(exePath, "~ DISABLEDXMAXIMIZEDWINDOWEDMODE");

                }

                Thread.Sleep(500); // 延迟 1000 毫秒 = 1 秒
                System.Windows.MessageBox.Show("黑屏修复成功！\n", "Bodycam 工具箱", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                Thread.Sleep(1000); // 延迟 500 毫秒 = 0.5 秒
                System.Windows.MessageBox.Show("发生错误：" + ex.Message, "Bodycam 工具箱", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}