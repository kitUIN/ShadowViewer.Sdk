using ShadowViewer.Extensions;

namespace ShadowViewer.Configs
{
    public class Config
    {
        public static void Init()
        {
            var defaultPath = ConfigHelper.IsPackaged ? ApplicationData.Current.LocalFolder.Path : System.Environment.CurrentDirectory;
            if (!ConfigHelper.Contains("ComicsPath"))
            {
                ComicsPath = Path.Combine(defaultPath, "Comics");
            }
            if (!ConfigHelper.Contains("TempPath"))
            {
                TempPath = Path.Combine(defaultPath, "Temps");
            }
            if (!ConfigHelper.Contains("PluginsPath"))
            {
                PluginsPath = Path.Combine(defaultPath, "Plugins");
            }
            if (!ConfigHelper.Contains("IsBookShelfInfoBar"))
            {
                IsBookShelfInfoBar = true;
            }
            IsDebugEvent();
            ComicsPath.CreateDirectory();
            TempPath.CreateDirectory();
            PluginsPath.CreateDirectory();
            // EnabledPlugins.CollectionChanged += EnabledPlugins_CollectionChanged;
        }
        #region 主程序设置
        /// <summary>
        /// 漫画缓存文件夹地址
        /// </summary>
        public static string ComicsPath
        {
            get => ConfigHelper.GetString("ComicsPath");
            set => ConfigHelper.Set("ComicsPath", value);
        }
        /// <summary>
        /// 插件文件夹地址
        /// </summary>
        public static string PluginsPath
        {
            get => ConfigHelper.GetString("PluginsPath");
            set => ConfigHelper.Set("PluginsPath", value);
        }
        /// <summary>
        /// 插件列表网址
        /// </summary>
        public static string PluginsUri
        {
            get => ConfigHelper.GetString("PluginsUri");
            set => ConfigHelper.Set("PluginsUri", value);
        }
        /// <summary>
        /// 临时文件夹地址
        /// </summary>
        public static string TempPath
        {
            get => ConfigHelper.GetString("TempPath");
            set => ConfigHelper.Set("TempPath", value);
        }
        /// <summary>
        /// 调试模式
        /// </summary>
        public static bool IsDebug
        {
            get => ConfigHelper.GetBoolean("IsDebug");
            set
            {
                if (IsDebug != value)
                {
                    ConfigHelper.Set("IsDebug", value);
                    IsDebugEvent();
                }
            }
        }
        /// <summary>
        /// 删除二次确认
        /// </summary>
        public static bool IsRememberDeleteFilesWithComicDelete
        {
            get => ConfigHelper.GetBoolean("IsRememberDeleteFilesWithComicDelete");
            set => ConfigHelper.Set("IsRememberDeleteFilesWithComicDelete", value);
        }
        /// <summary>
        /// 删除漫画同时删除漫画缓存
        /// </summary>
        public static bool IsDeleteFilesWithComicDelete
        {
            get => ConfigHelper.GetBoolean("IsDeleteFilesWithComicDelete");
            set => ConfigHelper.Set("IsDeleteFilesWithComicDelete", value);
        }
        /// <summary>
        /// 书架顶部左下信息栏
        /// </summary>
        public static bool IsBookShelfInfoBar
        {
            get => ConfigHelper.GetBoolean("IsBookShelfInfoBar");
            set => ConfigHelper.Set("IsBookShelfInfoBar", value);
        }
        /// <summary>
        /// 允许相同文件夹导入
        /// </summary>
        public static bool IsImportAgain
        {
            get => ConfigHelper.GetBoolean("IsImportAgain");
            set => ConfigHelper.Set("IsImportAgain", value);
        } 
        /// <summary>
        /// 书架-样式-详细/简约
        /// </summary>
        public static bool BookStyleDetail
        {
            get => ConfigHelper.GetBoolean("BookStyleDetail");
            set => ConfigHelper.Set("BookStyleDetail", value);
        }
        private static void IsDebugEvent()
        {
            var defaultPath = ConfigHelper.IsPackaged ? ApplicationData.Current.LocalFolder.Path : System.Environment.CurrentDirectory;
            if (IsDebug)
            {
                Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(Path.Combine(defaultPath, "Logs", "ShadowViewer.log"), outputTemplate: "{Timestamp:MM-dd HH:mm:ss.fff} [{Level:u4}] {SourceContext} | {Message:lj} {Exception}{NewLine}", rollingInterval: RollingInterval.Day, shared: true)
                .CreateLogger();
                Log.ForContext<Config>().Debug("调试模式开启");
            }
            else
            {
                Log.ForContext<Config>().Debug("调试模式关闭");
                Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File(Path.Combine(defaultPath, "Logs", "ShadowViewer.log"), outputTemplate: "{Timestamp:MM-dd HH:mm:ss.fff} [{Level:u4}] {SourceContext} | {Message:lj} {Exception}{NewLine}", rollingInterval: RollingInterval.Day, shared: true)
                .CreateLogger();
            }
        }
        
        #endregion

    }
}
