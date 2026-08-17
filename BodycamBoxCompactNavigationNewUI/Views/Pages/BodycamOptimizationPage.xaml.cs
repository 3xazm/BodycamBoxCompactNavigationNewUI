using BodycamBoxCompactNavigationNewUI.ViewModels.Pages;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using Wpf.Ui.Abstractions.Controls;

namespace BodycamBoxCompactNavigationNewUI.Views.Pages
{
    public partial class BodycamOptimizationPage : INavigableView<BodycamOptimizationViewModel>
    {
        private const string PackageResourcePrefix = "BodycamBoxCompactNavigationNewUI.OptimizationBC._2026n06y07r";

        private static readonly string[] OptimizationFiles =
        {
            "dwmapi.dll",
            "UE4SS.dll",
            "UE4SS-settings.ini",
            @"Mods\mods.txt",
            @"Mods\BodycamOptimizer\enabled.txt",
            @"Mods\BodycamOptimizer\Scripts\main.lua",
            @"Mods\shared\jsbProfiler\jsbProfi.lua",
            @"Mods\shared\UEHelpers\UEHelpers.lua"
        };

        public BodycamOptimizationViewModel ViewModel { get; }

        public BodycamOptimizationPage(BodycamOptimizationViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
        }

        private bool IsGameRunning()
        {
            return Process.GetProcessesByName("Bodycam").Length > 0;
        }

        private string? GetSteamPath()
        {
            using RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"Software\Valve\Steam");
            return key?.GetValue("SteamPath") as string;
        }

        private string? GetGameWin64Path()
        {
            string? steamPath = GetSteamPath();

            if (string.IsNullOrWhiteSpace(steamPath))
            {
                MessageBox.Show("未检测到 Steam！", "Bodycam 工具箱", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }

            var libraries = new List<string> { steamPath };
            string vdfPath = Path.Combine(steamPath, @"steamapps\libraryfolders.vdf");

            if (File.Exists(vdfPath))
            {
                foreach (string line in File.ReadAllLines(vdfPath))
                {
                    if (!line.Contains("\"path\""))
                    {
                        continue;
                    }

                    string[] parts = line.Split('"');
                    if (parts.Length > 3)
                    {
                        libraries.Add(parts[3].Replace(@"\\", @"\"));
                    }
                }
            }

            foreach (string library in libraries)
            {
                string gamePath = Path.Combine(library, @"steamapps\common\Bodycam\Bodycam\Binaries\Win64");
                if (Directory.Exists(gamePath))
                {
                    return gamePath;
                }
            }

            MessageBox.Show("未找到 Bodycam 游戏目录！", "Bodycam 工具箱", MessageBoxButton.OK, MessageBoxImage.Warning);
            return null;
        }

        private string GetResourceName(string relativePath)
        {
            return $"{PackageResourcePrefix}.{relativePath.Replace('\\', '.')}";
        }

        private bool HasOptimizationPackage()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            var resources = new HashSet<string>(assembly.GetManifestResourceNames());

            foreach (string file in OptimizationFiles)
            {
                if (!resources.Contains(GetResourceName(file)))
                {
                    return false;
                }
            }

            return true;
        }

        private void ExtractOptimizationFile(string relativePath, string targetRoot)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = GetResourceName(relativePath);
            string outputPath = Path.Combine(targetRoot, relativePath);
            string? outputDirectory = Path.GetDirectoryName(outputPath);

            if (!string.IsNullOrEmpty(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            using Stream? stream = assembly.GetManifestResourceStream(resourceName);

            if (stream == null)
            {
                throw new FileNotFoundException("Bodycam优化包丢失", resourceName);
            }

            using FileStream fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write);
            stream.CopyTo(fileStream);
        }

        private void AddOptimization_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!HasOptimizationPackage())
                {
                    MessageBox.Show("Bodycam优化包丢失 添加失败！", "Bodycam 工具箱", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (IsGameRunning())
                {
                    MessageBox.Show("请先关闭 Bodycam 游戏再添加优化！", "Bodycam 工具箱", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string? targetPath = GetGameWin64Path();

                if (string.IsNullOrEmpty(targetPath))
                {
                    return;
                }

                foreach (string file in OptimizationFiles)
                {
                    ExtractOptimizationFile(file, targetPath);
                }

                MessageBox.Show("Bodycam优化添加成功！", "Bodycam 工具箱", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bodycam优化添加失败：" + ex.Message, "Bodycam 工具箱", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RemoveOptimization_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsGameRunning())
                {
                    MessageBox.Show("请先关闭 Bodycam 游戏再移除优化！", "Bodycam 工具箱", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string? targetPath = GetGameWin64Path();

                if (string.IsNullOrEmpty(targetPath))
                {
                    return;
                }

                string modsPath = Path.Combine(targetPath, @"Mods\BodycamOptimizer");
                if (Directory.Exists(modsPath))
                {
                    Directory.Delete(modsPath, true);
                }

                string[] rootFiles =
                {
                    "dwmapi.dll",
                    "UE4SS.dll",
                    "UE4SS-settings.ini"
                };

                foreach (string file in rootFiles)
                {
                    string path = Path.Combine(targetPath, file);
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }

                MessageBox.Show("Bodycam优化已移除！", "Bodycam 工具箱", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bodycam优化移除失败：" + ex.Message, "Bodycam 工具箱", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
