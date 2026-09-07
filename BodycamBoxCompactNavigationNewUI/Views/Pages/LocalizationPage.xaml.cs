using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.Reflection;

namespace BodycamBoxCompactNavigationNewUI.Views.Pages
{
    public partial class LocalizationPage : Page
    {
        public LocalizationPage()
        {
            InitializeComponent();
        }

        // ===============================
        // 检测 并提取内置资源到指定路径
        // ===============================
        private void ExtractEmbeddedResource(string resourceName, string outputPath)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            using Stream stream = assembly.GetManifestResourceStream(resourceName);

            if (stream == null)
            {
                MessageBox.Show(
                    "未找到内置资源：\n" + resourceName,
                    "错误",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                return;
            }

            using FileStream fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write);

            stream.CopyTo(fileStream);
        }

        // ===============================
        // 检测 Bodycam 是否运行
        // ===============================
        private bool IsGameRunning()
        {
            return Process.GetProcessesByName("Bodycam").Length > 0;
        }

        // ===============================
        // 获取游戏目录
        // ===============================
        private string GetSteamPath()
        {
            var key = Registry.CurrentUser.OpenSubKey(@"Software\Valve\Steam");
            return key?.GetValue("SteamPath") as string;
        }

        private string GetGamePaksPath()
        {
            string steamPath = GetSteamPath();

            if (string.IsNullOrEmpty(steamPath))
            {
                MessageBox.Show("未检测到 Steam！", "Bodycam 工具箱",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }

            List<string> libraries = new List<string>();

            // ✅ 主库
            libraries.Add(steamPath);

            // ✅ 额外库
            string vdfPath = Path.Combine(steamPath, @"steamapps\libraryfolders.vdf");

            if (File.Exists(vdfPath))
            {
                var lines = File.ReadAllLines(vdfPath);

                foreach (var line in lines)
                {
                    if (line.Contains("\"path\""))
                    {
                        string path = line.Split('"')[3].Replace(@"\\", @"\");
                        libraries.Add(path);
                    }
                }
            }

            foreach (var lib in libraries)
            {
                string gamePath = Path.Combine(lib, @"steamapps\common\Bodycam\Bodycam\Content\Paks");

                if (Directory.Exists(gamePath))
                {
                    return gamePath;
                }
            }

            MessageBox.Show("未找到 Bodycam 游戏目录！", "Bodycam 工具箱",
                MessageBoxButton.OK, MessageBoxImage.Warning);

            return null;
        }

        // ===============================
        // 安装汉化
        // ===============================
        private void InstallLocalization(string sourceFolder)
        {
            try
            {
                if (!Directory.Exists(sourceFolder))
                {
                    MessageBox.Show("Error 汉化包文件丢失！");
                    return;
                }

                if (IsGameRunning())
                {
                    MessageBox.Show(
                        " 请先关闭 Bodycam 游戏再添加汉化包！ ",
                        "Bodycam 工具箱",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return;
                }

                string targetPath = GetGamePaksPath();

                if (!Directory.Exists(targetPath))
                {
                    MessageBox.Show(
                        "< 未找到游戏目录！ >",
                        "Bodycam 工具箱",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                    return;
                }

                // 要复制的文件
                string[] files =
                {
                    "BodycamLocalization_P.pak",
                    "BodycamLocalization_P.ucas",
                    "BodycamLocalization_P.utoc",
                    "BodycamzhYaHeiFontsSC_P.pak",
                };

                foreach (var file in files)
                {
                    string src = Path.Combine(sourceFolder, file);
                    string dest = Path.Combine(targetPath, file);

                    if (File.Exists(src))
                    {
                        File.Copy(src, dest, true);
                    }
                }

                MessageBox.Show(
                    " 汉化包添加成功！ ",
                    "Bodycam 工具箱",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "安装失败：" + ex.Message,
                    "错误",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        // ===============================
        // 删除汉化
        // ===============================
        private void DeleteLocalization()
        {
            try
            {
                if (IsGameRunning())
                {
                    MessageBox.Show(
                        "请先关闭 Bodycam 游戏再删除汉化包！",
                        "Bodycam 工具箱",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return;
                }

                string targetPath = GetGamePaksPath();

                // ✅ 关键：判断路径是否存在
                if (string.IsNullOrEmpty(targetPath))
                {
                    MessageBox.Show(
                        " 未找到 Bodycam 游戏路径！（请确认游戏已添加） ",
                        "Bodycam 工具箱",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return;
                }

                string[] files =
                {
            "BodycamLocalization_P.pak",
            "BodycamLocalization_P.ucas",
            "BodycamLocalization_P.utoc",
            "BodycamzhYaHeiFontsSC_P.pak",
        };

                bool deletedAny = false;

                foreach (var file in files)
                {
                    string path = Path.Combine(targetPath, file);

                    if (File.Exists(path))
                    {
                        File.Delete(path);
                        deletedAny = true;
                    }
                }

                // ✅ 优化提示（更专业）
                if (deletedAny)
                {
                    MessageBox.Show(
                        "汉化包已删除！",
                        "Bodycam 工具箱",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                }
                else
                {
                    MessageBox.Show(
                        " 未检测到已添加的汉化包。 ",
                        "Bodycam 工具箱",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "删除失败：" + ex.Message,
                    "错误",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private void ExtractAndInstall(string resourceSubPath)
        {
            try
            {
                // 1. 定位到 AppData/Local/Bodycam Box Temps
                string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string tempDir = Path.Combine(localAppData, "Bodycam Box Temps", resourceSubPath.TrimStart('_'));

                // 如果文件夹不存在则创建
                if (!Directory.Exists(tempDir))
                {
                    Directory.CreateDirectory(tempDir);
                }

                // 2. 资源名称前缀 (根据你的项目命名空间)
                string prefix = "BodycamBoxCompactNavigationNewUI.playAssets." + resourceSubPath;
                string[] files = { "BodycamLocalization_P.pak", "BodycamLocalization_P.ucas", "BodycamLocalization_P.utoc" };

                // 3. 提取文件到 AppData
                foreach (var file in files)
                {
                    string outPath = Path.Combine(tempDir, file);
                    // 这里调用你之前的 ExtractEmbeddedResource 方法
                    ExtractEmbeddedResource($"{prefix}.{file}", outPath);
                }

                // 4. 执行安装逻辑：从 AppData 拷贝到 Steam 游戏目录
                InstallLocalization(tempDir);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"解压过程出错：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void zhFontlibraryInstall(string resourceSubPath)
        {
            try
            {
                // 1. 定位到 AppData/Local/Bodycam Box Temps
                string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string tempDir = Path.Combine(localAppData, "Bodycam Box Temps Library", resourceSubPath.TrimStart('_'));

                // 如果文件夹不存在则创建
                if (!Directory.Exists(tempDir))
                {
                    Directory.CreateDirectory(tempDir);
                }

                // 2. 资源名称前缀 (根据你的项目命名空间)
                string prefix = "BodycamBoxCompactNavigationNewUI.playAssets." + resourceSubPath;
                string[] files = { "BodycamzhYaHeiFontsSC_P.pak",};

                // 3. 提取文件到 AppData
                foreach (var file in files)
                {
                    string outPath = Path.Combine(tempDir, file);
                    // 这里调用你之前的 ExtractEmbeddedResource 方法
                    ExtractEmbeddedResource($"{prefix}.{file}", outPath);
                }

                // 4. 执行安装逻辑：从 AppData 拷贝到 Steam 游戏目录
                InstallLocalization(tempDir);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"解压过程出错：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ===============================
        // y2026f9f02bodycamTestv08   05 汉化按钮
        // ===============================
        private void Addy2026f9f02bodycamTestv08xxx05ri_Click(object sender, RoutedEventArgs e)
        {
            ExtractAndInstall("y2026f9f02bodycamTestv08xxx05ri");
            zhFontlibraryInstall("zhFontlibrary");
        }

        // ===============================
        // y2026f9f02bodycamTestv08   04 汉化按钮
        // ===============================
        private void Addy2026f9f02bodycamTestv08xxx04ri_Click(object sender, RoutedEventArgs e)
        {
            ExtractAndInstall("y2026f9f02bodycamTestv08xxx04ri");
            zhFontlibraryInstall("zhFontlibrary");
        }

        // ===============================
        // y2026f9f02bodycamTestv08 汉化按钮
        // ===============================
        private void Addy2026f9f02bodycamTestv08_Click(object sender, RoutedEventArgs e)
        {
            ExtractAndInstall("y2026f9f02bodycamTestv08");
            zhFontlibraryInstall("zhFontlibrary");
        }

        // ===============================
        // 2026 Arui and metaOBS 汉化按钮
        // ===============================
        private void Add2026patch_Click(object sender, RoutedEventArgs e)
        {
            // 注意：这里的路径要和你嵌入资源的层级完全一致
            // 如果是 playAssets/_2026/AruiAndmetaOBS
            ExtractAndInstall("_2026.patch");
        }

        // ===============================
        // 2026 Arui and metaOBS 汉化按钮
        // ===============================
        private void Add2026AruiAndmetaOBS_Click(object sender, RoutedEventArgs e)
        {
            // 注意：这里的路径要和你嵌入资源的层级完全一致
            // 如果是 playAssets/_2026/AruiAndmetaOBS
            ExtractAndInstall("_2026.AruiAndmetaOBS");
        }

        // ===============================
        // 2026 汉化按钮
        // ===============================
        private void Add2026_Click(object sender, RoutedEventArgs e)
        {
            ExtractAndInstall("_2026");
        }

        // ===============================
        // 2025 汉化按钮
        // ===============================
        private void Add2025_Click(object sender, RoutedEventArgs e)
        {
            ExtractAndInstall("_2025");
        }

        // ===============================
        // 删除按钮
        // ===============================
        private void RemoveLocalization_Click(object sender, RoutedEventArgs e)
        {
            DeleteLocalization();
        }
    }
}