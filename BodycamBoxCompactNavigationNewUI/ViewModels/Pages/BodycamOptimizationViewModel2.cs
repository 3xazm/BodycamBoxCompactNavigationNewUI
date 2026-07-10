using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;

namespace BodycamBoxCompactNavigationNewUI.ViewModels.Pages
{
    public partial class BodycamOptimizationViewModel2 : ObservableObject
    {
        // ----- 1. 文本与颜色属性定义 -----

        [ObservableProperty]
        private string _faceMosaicStatus = "检测中...";

        [ObservableProperty]
        private Brush _faceMosaicStatusBrush = Brushes.Gray;

        [ObservableProperty]
        private string _fullDynamicBlurStatus = "检测中...";

        [ObservableProperty]
        private Brush _fullDynamicBlurStatusBrush = Brushes.Gray;

        private string? _luaPath;

        // ----- 2. 构造函数 -----
        public BodycamOptimizationViewModel2()
        {
            InitializeLuaPath();
            LoadAllStatuses();
        }

        // 核心功能：刷新按钮绑定的命令
        [RelayCommand]
        private void Refresh()
        {
            InitializeLuaPath(); // 重新定位文件
            LoadAllStatuses();   // 重新读取数值与上色
        }

        // ----- 3. 全盘符自动定位算法 -----
        private void InitializeLuaPath()
        {
            string? gameRoot = FindBodycamSteamPath();
            if (!string.IsNullOrEmpty(gameRoot))
            {
                _luaPath = Path.Combine(gameRoot, @"Bodycam\Binaries\Win64\Mods\BodycamOptimizer\Scripts\main.lua");
                if (File.Exists(_luaPath)) return;
            }

            string fallbackPath1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Bodycam\Binaries\Win64\Mods\BodycamOptimizer\Scripts\main.lua");
            if (File.Exists(fallbackPath1))
            {
                _luaPath = fallbackPath1;
                return;
            }

            string fallbackPath2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Mods\BodycamOptimizer\Scripts\main.lua");
            if (File.Exists(fallbackPath2))
            {
                _luaPath = fallbackPath2;
                return;
            }

            _luaPath = null;
        }

        private string? FindBodycamSteamPath()
        {
            try
            {
                string? steamPath = Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Software\Valve\Steam", "SteamPath", null) as string;
                if (string.IsNullOrEmpty(steamPath))
                {
                    steamPath = Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Valve\Steam", "InstallPath", null) as string;
                }

                if (string.IsNullOrEmpty(steamPath)) return null;
                steamPath = steamPath.Replace('/', '\\');

                string defaultPath = Path.Combine(steamPath, @"steamapps\common\Bodycam");
                if (Directory.Exists(defaultPath)) return defaultPath;

                string vdfPath = Path.Combine(steamPath, @"steamapps\libraryfolders.vdf");
                if (File.Exists(vdfPath))
                {
                    string vdfContent = File.ReadAllText(vdfPath);
                    var matches = Regex.Matches(vdfContent, @"""path""\s+""([^""]+)""");
                    foreach (Match match in matches)
                    {
                        string libPath = match.Groups[1].Value.Replace(@"\\", @"\");
                        string gamePath = Path.Combine(libPath, @"steamapps\common\Bodycam");
                        if (Directory.Exists(gamePath)) return gamePath;
                    }
                }
            }
            catch { }
            return null;
        }

        // ----- 4. 数据加载与颜色赋予逻辑 -----
        public void LoadAllStatuses()
        {
            if (string.IsNullOrEmpty(_luaPath) || !File.Exists(_luaPath))
            {
                FaceMosaicStatus = "状态：未检测到优化模块 (请先前往[添加优化]页面启用)";
                FaceMosaicStatusBrush = Brushes.Gold;

                FullDynamicBlurStatus = "状态：未检测到优化模块 (请先前往[添加优化]页面启用)";
                FullDynamicBlurStatusBrush = Brushes.Gold;
                return;
            }

            try
            {
                string text = File.ReadAllText(_luaPath);

                // 脸部马赛克状态
                Match faceMatch = Regex.Match(text, @"local\s+FaceMosaicEnabled\s*=\s*(true|false)");
                if (faceMatch.Success)
                {
                    bool isOn = faceMatch.Groups[1].Value == "true";
                    FaceMosaicStatus = isOn ? "当前模式：已激活" : "当前模式：关闭";
                    FaceMosaicStatusBrush = isOn ? Brushes.LimeGreen : Brushes.Red;
                }
                else
                {
                    FaceMosaicStatus = "状态：Null (无法解析配置)";
                    FaceMosaicStatusBrush = Brushes.Red;
                }

                // 全局去模糊状态
                Match blurMatch = Regex.Match(text, @"local\s+FullDynamicBlurEnabled\s*=\s*(true|false)");
                if (blurMatch.Success)
                {
                    bool isOn = blurMatch.Groups[1].Value == "true";
                    FullDynamicBlurStatus = isOn ? "当前模式：已激活" : "当前模式：关闭";
                    FullDynamicBlurStatusBrush = isOn ? Brushes.LimeGreen : Brushes.Red;
                }
                else
                {
                    FullDynamicBlurStatus = "状态：Null (无法解析配置)";
                    FullDynamicBlurStatusBrush = Brushes.Red;
                }
            }
            catch
            {
                FaceMosaicStatus = "状态：Null (读取异常)";
                FaceMosaicStatusBrush = Brushes.Red;
                FullDynamicBlurStatus = "状态：Null (读取异常)";
                FullDynamicBlurStatusBrush = Brushes.Red;
            }
        }

        // ----- 5. 控制命令 -----
        [RelayCommand]
        private void ToggleFaceMosaic()
        {
            ToggleLuaSetting(
                settingName: "FaceMosaicEnabled",
                successMessageOn: "脸部马赛克模式已开启",
                successMessageOff: "脸部马赛克模式已关闭",
                updateStatusAction: (isOn) =>
                {
                    FaceMosaicStatus = isOn ? "当前模式：开启" : "当前模式：关闭";
                    FaceMosaicStatusBrush = isOn ? Brushes.LimeGreen : Brushes.Red;
                }
            );
        }

        [RelayCommand]
        private void ToggleFullDynamicBlur()
        {
            ToggleLuaSetting(
                settingName: "FullDynamicBlurEnabled",
                successMessageOn: "全局去模糊（极致超清）模式已开启",
                successMessageOff: "全局去模糊（极致超清）模式已关闭",
                updateStatusAction: (isOn) =>
                {
                    FullDynamicBlurStatus = isOn ? "当前模式：开启" : "当前模式：关闭";
                    FullDynamicBlurStatusBrush = isOn ? Brushes.LimeGreen : Brushes.Red;
                }
            );
        }

        private void ToggleLuaSetting(string settingName, string successMessageOn, string successMessageOff, Action<bool> updateStatusAction)
        {
            try
            {
                if (string.IsNullOrEmpty(_luaPath) || !File.Exists(_luaPath))
                {
                    //MessageBox.Show("未找到 main.lua 配置文件，请确认游戏是否完整安装！", "Bodycam工具箱", MessageBoxButton.OK, MessageBoxImage.Warning);
                    MessageBox.Show("未添加优化模式，请去添加！", "Bodycam工具箱", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string text = File.ReadAllText(_luaPath);
                string pattern = $@"local\s+{settingName}\s*=\s*(true|false)";
                Match match = Regex.Match(text, pattern);

                if (!match.Success)
                {
                    MessageBox.Show($"未能在配置文件中找到 [{settingName}] 项", "Bodycam工具箱", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                bool currentState = match.Groups[1].Value == "true";
                bool newState = !currentState;

                text = Regex.Replace(text, pattern, $"local {settingName} = {newState.ToString().ToLower()}");
                File.WriteAllText(_luaPath, text);

                updateStatusAction?.Invoke(newState);

                MessageBox.Show(newState ? successMessageOn : successMessageOff, "Bodycam工具箱", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"操作失败: {ex.Message}", "Bodycam工具箱", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}