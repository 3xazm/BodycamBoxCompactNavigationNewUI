using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.IO;
using System.Runtime.InteropServices;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Media;

namespace BodycamBoxCompactNavigationNewUI.ViewModels.Pages
{
    public partial class ResolutionViewModel : ObservableObject
    {
        //系统分辨率
        [ObservableProperty]
        private string _currentResolution;

        //Bodycam分辨率
        [ObservableProperty]
        private string _bodycamResolution;
        //Bodycam分辨率颜色
        [ObservableProperty]
        private Brush _bodycamResolutionBrush = Brushes.Red;

        //分辨率修复状态
        [ObservableProperty]
        private string _resolutionStatus;
        //分辨率修复状态颜色
        [ObservableProperty]
        private Brush _resolutionStatusBrush = Brushes.Gray;

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
            // 初始化时加载系统分辨率和 Bodycam 分辨率
            LoadResolution();
            // 加载 Bodycam 分辨率
            LoadBodycamResolution();
            // 检查分辨率状态
            CheckResolutionStatus();
        }

        // 功能扩展 1 的构造函数  黑屏的位置
        string FindBodycamExe()
        {
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (!drive.IsReady) continue;

                try
                {
                    string result = SearchFile(drive.RootDirectory.FullName, "Bodycam.exe");

                    if (result != null)
                        return result;
                }
                catch { }
            }

            return null;
        }

        private string SearchFile(string root, string fileName)
        {
            try
            {
                // 先找当前目录
                var files = Directory.GetFiles(root, fileName);
                if (files.Length > 0)
                    return files[0];

                // 再递归子目录
                var dirs = Directory.GetDirectories(root);
                foreach (var dir in dirs)
                {
                    try
                    {
                        string result = SearchFile(dir, fileName);
                        if (result != null)
                            return result;
                    }
                    catch { }
                }
            }
            catch { }

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
            CurrentResolution = $"当前你的系统分辨率为: {width} x {height}";
        }

        /// <summary>
        /// 设计思路：直接读取 Bodycam 的配置文件，获取分辨率
        /// </summary>
        // ✅  Bodycam 分辨率获取
        private void LoadBodycamResolution()
        {
            try
            {
                string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

                string iniPath = Path.Combine(localAppData, @"Bodycam\Saved\Config\Windows\GameUserSettings.ini");

                if (!File.Exists(iniPath))
                {
                    BodycamResolution = "当前你的Bodycam分辨率为: Null";
                    BodycamResolutionBrush = Brushes.Red;
                    return;
                }

                string text = File.ReadAllText(iniPath);

                Match matchX = Regex.Match(text, @"ResolutionSizeX=(\d+)");
                Match matchY = Regex.Match(text, @"ResolutionSizeY=(\d+)");

                if (matchX.Success && matchY.Success)
                {
                    int bodycamWidth = int.Parse(matchX.Groups[1].Value);
                    int bodycamHeight = int.Parse(matchY.Groups[1].Value);

                    var (systemWidth, systemHeight) = GetRealResolution();

                    BodycamResolution =
                        $"当前你的Bodycam分辨率为: {bodycamWidth} x {bodycamHeight}";

                    // 判断是否一致
                    if (bodycamWidth == systemWidth &&
                        bodycamHeight == systemHeight)
                    {
                        BodycamResolutionBrush = Brushes.LimeGreen;   // 绿色
                    }
                    else
                    {
                        BodycamResolutionBrush = Brushes.Gold;        // 黄色
                    }
                }
                else
                {
                    BodycamResolution = "当前你的Bodycam分辨率为: Null";
                    BodycamResolutionBrush = Brushes.Red;
                }
            }
            catch
            {
                BodycamResolution = "当前你的Bodycam分辨率为: Null";
                BodycamResolutionBrush = Brushes.Red;
            }
        }

        /// <summary>
        /// 设计思路：直接读取 Bodycam 的配置文件，获取分辨率，并与系统分辨率进行比较
        /// </summary>
        // ✅  Bodycam 分辨率状态
        private void CheckResolutionStatus()
        {
            try
            {
                string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

                string iniPath = Path.Combine(localAppData, @"Bodycam\Saved\Config\Windows\GameUserSettings.ini");

                if (!File.Exists(iniPath))
                {
                    ResolutionStatus = "*检查到系统与Bodycam分辨率：Null";
                    ResolutionStatusBrush = Brushes.Red;
                    return;
                }

                string text = File.ReadAllText(iniPath);

                Match matchX = Regex.Match(text, @"ResolutionSizeX=(\d+)");
                Match matchY = Regex.Match(text, @"ResolutionSizeY=(\d+)");

                if (!matchX.Success || !matchY.Success)
                {
                    ResolutionStatus = "当前状态：无法检测（未找到Bodycam配置）❌";
                    ResolutionStatusBrush = Brushes.Red;
                    return;
                }

                var (systemWidth, systemHeight) = GetRealResolution();

                int bodycamWidth = int.Parse(matchX.Groups[1].Value);
                int bodycamHeight = int.Parse(matchY.Groups[1].Value);

                if (systemWidth == bodycamWidth &&
                    systemHeight == bodycamHeight)
                {
                    ResolutionStatus = "当前状态：正常（系统与Bodycam分辨率一致）✅";
                    ResolutionStatusBrush = Brushes.LimeGreen;
                }
                else
                {
                    ResolutionStatus = "当前状态：需要修复（系统与Bodycam分辨率不一致）⚠️";
                    ResolutionStatusBrush = Brushes.Gold;
                }
            }
            catch
            {
                ResolutionStatus = "当前状态：无法检测（未找到Bodycam配置）❌";
                ResolutionStatusBrush = Brushes.Red;
            }
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

                // 重新加载分辨率状态
                CheckResolutionStatus();
                // 重新加载 Bodycam 分辨率 (刷新)
                LoadBodycamResolution();   
                Thread.Sleep(500); // 延迟 500 毫秒 = 0.5 秒   让用户有时间看到修改完成的提示
                System.Windows.MessageBox.Show("分辨率修复成功！", "Bodycam 工具箱", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                Thread.Sleep(500); // 延迟 500 毫秒 = 0.5 秒
                System.Windows.MessageBox.Show("发生错误：" + ex.Message, "Bodycam 工具箱", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        // 分辨率破坏按钮
        [RelayCommand]
        private void DestroyResolution()
        {
            try
            {
                var processes = Process.GetProcessesByName("Bodycam");

                if (processes.Length > 0)
                {
                    MessageBox.Show(
                        "检测到 Bodycam 正在运行，请先关闭！",
                        "Bodycam 工具箱",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    return;
                }

                string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

                string iniPath = Path.Combine(localAppData, @"Bodycam\Saved\Config\Windows\GameUserSettings.ini");
                string jsonPath = Path.Combine(localAppData, @"Bodycam\Saved\SaveGames\SystemConfig.json");

                const int width = 1568;
                const int height = 680;

                // 修改 GameUserSettings.ini
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

                // 修改 SystemConfig.json
                if (File.Exists(jsonPath))
                {
                    string jsonText = File.ReadAllText(jsonPath);

                    jsonText = Regex.Replace(
                        jsonText,
                        @"""DisplayResolution"":""\d+\s*x\s*\d+""",
                        $"\"DisplayResolution\":\"{width} x {height}\"");

                    File.WriteAllText(jsonPath, jsonText);
                }

                // 刷新页面显示
                LoadBodycamResolution();
                CheckResolutionStatus();

                MessageBox.Show(
                    "     ~ ·分辨率· 以被 ·破坏· ~    ",
                    "Bodycam 工具箱",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "发生错误：" + ex.Message,
                    "Bodycam 工具箱",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        //刷新一下按钮
        [RelayCommand]
        private void RefreshResolution()
        {
            // 刷新页面显示
            LoadBodycamResolution();
            CheckResolutionStatus();
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
                    MessageBoxImage.Error);
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

        //五颜六色彩虹修复
        [RelayCommand]
        private void FixRainbowScreen()
        {
            try
            {
                // Windows HDR 注册表路径
                string keyPath = @"Software\Microsoft\Windows\CurrentVersion\VideoSettings";

                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(keyPath, true))
                {
                    if (key != null)
                    {
                        // 关闭 HDR
                        key.SetValue("EnableHDRForPlayback", 0, RegistryValueKind.DWord);
                    }
                }

                // 刷新显示设置
                Process.Start(new ProcessStartInfo
                {
                    FileName = "displayswitch.exe",
                    Arguments = "/extend",
                    CreateNoWindow = true,
                    UseShellExecute = false
                });

                MessageBox.Show("彩色/闪屏异常已修复", "Bodycam 工具箱");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"彩色/闪屏异常 修复失败：{ex.Message}");
            }
        }
    }
}