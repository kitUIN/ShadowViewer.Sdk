using Serilog;
using System.IO;
using Windows.Storage;

namespace ShadowViewer.Core.Settings
{
    /// <summary>
    /// 核心设置项
    /// </summary>
    public partial class CoreSettings
    {
        /// <summary>
        /// 
        /// </summary>
        public static CoreSettings Instance { get; } = new();

        partial void AfterInit()
        {
#if DEBUG
            IsDebug = true;
#endif
            IsDebugEvent();
        }

        private void IsDebugEvent()
        {
            var defaultPath = ApplicationData.Current.LocalFolder.Path;
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

        partial void IsDebugChanged(bool newValue)
        {
            IsDebugEvent();
        }
    }
}