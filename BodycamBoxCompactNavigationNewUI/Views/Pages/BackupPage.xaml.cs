using BodycamBoxCompactNavigationNewUI.ViewModels.Pages;
using Wpf.Ui.Abstractions.Controls;
using System.IO;
using System.Windows;

namespace BodycamBoxCompactNavigationNewUI.Views.Pages
{
    public partial class BackupPage : INavigableView<BackupViewModel>
    {
        public BackupViewModel ViewModel { get; }

        public BackupPage(BackupViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();

            // 👇 关键在这里
            Loaded += Page_Loaded;
        }

        //防止重复加载（只加载一次）
        private bool _loaded = false;
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (_loaded) return;  // 已经加载过了，直接返回
            _loaded = true;

            foreach (DriveInfo d in DriveInfo.GetDrives())
            {
                if (d.DriveType == DriveType.Removable || d.DriveType == DriveType.Fixed)
                {
                    DriveBox1.Items.Add(d.Name);
                    DriveBox2.Items.Add(d.Name);
                }
            }
        }

        /// <summary>
        /// 功能扩展 1 
        /// </summary>
        void CopyAndRename(string sourceFolder, string originalName, string destFolder, string newName)
        {
            string src = Path.Combine(sourceFolder, originalName);

            if (File.Exists(src))
            {
                string dest = Path.Combine(destFolder, newName);
                File.Copy(src, dest, true);
            }
        }

        void RestoreFile(string sourceFolder, string backupName, string destFolder, string originalName)
        {
            string src = Path.Combine(sourceFolder, backupName);

            if (File.Exists(src))
            {
                string dest = Path.Combine(destFolder, originalName);
                File.Copy(src, dest, true);
            }
        }

        void DeleteIfExists(string folder, string file)
        {
            string path = Path.Combine(folder, file);

            if (File.Exists(path))
                File.Delete(path);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // 没选硬盘
            if (DriveBox1.SelectedItem == null)
            {
                System.Windows.MessageBox.Show(" 未选择硬盘！ ", "Bodycam 工具箱", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            string drive = DriveBox1.SelectedItem.ToString();



            //爱心提示1
            string folder = Path.Combine(drive, @"BodycamBoxCompactNavigationNewUI\PlayerServerDataConfig");
            //爱心提示2
            if (Directory.Exists(folder))
            {
                Directory.Delete(folder, true); // true = 删除里面所有文件

                Thread.Sleep(1500); // 延迟 1500 毫秒 = 1.5 秒
                System.Windows.MessageBox.Show("·····玩家的数据 和 设置数据已更新! ····· ", "Bodycam 工具箱", MessageBoxButton.OK, MessageBoxImage.Information); 
            }
            else
            {
                //MessageBox.Show("未找到 PlayerServerDataConfig 文件夹！",
                System.Windows.MessageBox.Show("新人，感谢使用软件，爱心~", "Bodycam 工具箱", MessageBoxButton.OK, MessageBoxImage.Information);
            }


            // 玩家数据路径
            string source = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                @"Bodycam\Saved\SaveGames");

            // 检测玩家数据
            if (!Directory.Exists(source))
            {
                System.Windows.MessageBox.Show("未检出到玩家数据！", "Bodycam 工具箱", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // U盘路径            
            string toolFolder = Path.Combine(drive, "BodycamBoxCompactNavigationNewUI");
            string dataFolder = Path.Combine(toolFolder, "PlayerServerDataConfig");

            Directory.CreateDirectory(toolFolder);
            Directory.CreateDirectory(dataFolder);

            // 复制并改名
            File.Copy(Path.Combine(source, "EnhancedInputUserSettings.sav"),
                      Path.Combine(dataFolder, "ServerSettingsSuser"), true);

            File.Copy(Path.Combine(source, "PlayerInfo.sav"),
                      Path.Combine(dataFolder, "GamePlayerData"), true);

            File.Copy(Path.Combine(source, "PlayerSkin.sav"),
                      Path.Combine(dataFolder, "GamePlayerSetSkins"), true);

            File.Copy(Path.Combine(source, "SystemConfig.json"),
                      Path.Combine(dataFolder, "SettingsConfigSys"), true);

            // 创建日志
            string logFile = Path.Combine(toolFolder, "tool.log");

            string log =
            "Path: C:\\Program Files (x86)\\Steam\\steamapps\\common\\Bodycam\\Bodycam\\Saved\\SaveGames\n" +
            "Time: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\n" +
            "Version: v2.0\n\n";

            File.AppendAllText(logFile, log);


            //Thread.Sleep(1000); // 延迟 1000 毫秒 = 1 秒
            System.Windows.MessageBox.Show(
                "...玩家数据保存成功！...",
                "Bodycam 工具箱",
                 MessageBoxButton.OK,
                 MessageBoxImage.Information);
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            if (DriveBox2.SelectedItem == null)
            {
                System.Windows.MessageBox.Show("未选择备份盘！", "Bodycam 工具箱", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string drive = DriveBox2.SelectedItem.ToString();

            string dataFolder = Path.Combine(drive, @"BodycamBoxCompactNavigationNewUI\PlayerServerDataConfig");

            if (!Directory.Exists(dataFolder))
            {
                System.Windows.MessageBox.Show("未找到备份数据！", "Bodycam 工具箱", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string target = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                @"Bodycam\Saved\SaveGames");

            Directory.CreateDirectory(target);

            // 删除旧文件
            DeleteIfExists(target, "EnhancedInputUserSettings.sav");
            DeleteIfExists(target, "PlayerInfo.sav");
            DeleteIfExists(target, "PlayerSkin.sav");
            DeleteIfExists(target, "SystemConfig.json");

            // 延迟 3000 毫秒 = 3 秒
            Thread.Sleep(100);

            // 写入备份数据并改回原名
            File.Copy(Path.Combine(dataFolder, "ServerSettingsSuser"),
                      Path.Combine(target, "EnhancedInputUserSettings.sav"), true);

            File.Copy(Path.Combine(dataFolder, "GamePlayerData"),
                      Path.Combine(target, "PlayerInfo.sav"), true);

            File.Copy(Path.Combine(dataFolder, "GamePlayerSetSkins"),
                      Path.Combine(target, "PlayerSkin.sav"), true);

            File.Copy(Path.Combine(dataFolder, "SettingsConfigSys"),
                      Path.Combine(target, "SystemConfig.json"), true);

            // 延迟 3000 毫秒 = 3 秒
            Thread.Sleep(3000);

            System.Windows.MessageBox.Show(
                "玩家数据写入成功！",
                 "Bodycam 工具箱",
                 MessageBoxButton.OK,
                 MessageBoxImage.Information
            );
        }
    }
}