using Serilog;
using ShadowViewer.Core.Extensions;
using ShadowViewer.Core.Helpers;
using System.IO;
using Windows.Storage;

namespace ShadowViewer.Core
{
    /// <summary>
    /// 核心设置项
    /// </summary>
    public class CoreSettings
    {
        /// <summary>
        /// 设置项初始化
        /// </summary>
        public static void Init()
        {
            var defaultPath = ConfigHelper.IsPackaged
                ? ApplicationData.Current.LocalFolder.Path
                : System.Environment.CurrentDirectory;
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
#if DEBUG
            IsDebug = true;
#endif
            IsDebugEvent();
            if (!Directory.Exists(ComicsPath)) Directory.CreateDirectory(ComicsPath);
            if (!Directory.Exists(TempPath)) Directory.CreateDirectory(TempPath);
            if (!Directory.Exists(PluginsPath)) Directory.CreateDirectory(PluginsPath);
            // EnabledPlugins.CollectionChanged += EnabledPlugins_CollectionChanged;
        }

        #region 主程序设置

        /// <summary>
        /// 漫画缓存文件夹地址
        /// </summary>
        public static string ComicsPath
        {
            get => ConfigHelper.GetString("ComicsPath")!;
            set => ConfigHelper.Set("ComicsPath", value);
        }

        /// <summary>
        /// 插件文件夹地址
        /// </summary>
        public static string PluginsPath
        {
            get => ConfigHelper.GetString("PluginsPath")!;
            set => ConfigHelper.Set("PluginsPath", value);
        }

        /// <summary>
        /// 插件列表网址
        /// </summary>
        public static string PluginsUri
        {
            get => ConfigHelper.GetString("PluginsUri")!;
            set => ConfigHelper.Set("PluginsUri", value);
        }

        /// <summary>
        /// 临时文件夹地址
        /// </summary>
        public static string TempPath
        {
            get => ConfigHelper.GetString("TempPath")!;
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

        private static void IsDebugEvent()
        {
            var defaultPath = ConfigHelper.IsPackaged
                ? ApplicationData.Current.LocalFolder.Path
                : System.Environment.CurrentDirectory;
            if (IsDebug)
            {
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.File(Path.Combine(defaultPath, "Logs", "ShadowViewer.log"),
                        outputTemplate:
                        "{Timestamp:HH:mm:ss.fff} [{Level:u4}] {SourceContext} | {Message:lj} {Exception}{NewLine}",
                        rollingInterval: RollingInterval.Day, shared: true)
                    .CreateLogger();
                Log.ForContext<CoreSettings>().Debug("调试模式开启");
            }
            else
            {
                Log.ForContext<CoreSettings>().Debug("调试模式关闭");
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.File(Path.Combine(defaultPath, "Logs", "ShadowViewer.log"),
                        outputTemplate:
                        "{Timestamp:HH:mm:ss.fff} [{Level:u4}] {SourceContext} | {Message:lj} {Exception}{NewLine}",
                        rollingInterval: RollingInterval.Day, shared: true)
                    .CreateLogger();
            }
        }

        #endregion
    }
}