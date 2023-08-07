using Windows.ApplicationModel;
using ShadowViewer.Configs;

namespace ShadowViewer.ViewModels
{
    /// <summary>
    /// 设置
    /// </summary>
    public partial class SettingsViewModel : ObservableObject
    {
        private readonly ICallableToolKit caller;
        /// <summary>
        /// 当前版本号
        /// </summary>
        public string Version { get; }

        [ObservableProperty] private bool isDebug = Config.IsDebug;
        [ObservableProperty] private string comicsPath = Config.ComicsPath;
        [ObservableProperty] private string tempPath = Config.TempPath;
        [ObservableProperty] private string pluginsPath = Config.PluginsPath;
        [ObservableProperty] private string pluginsUri = Config.PluginsUri;

        [ObservableProperty]
        private bool isRememberDeleteFilesWithComicDelete = !Config.IsRememberDeleteFilesWithComicDelete;

        [ObservableProperty] private bool isDeleteFilesWithComicDelete = Config.IsDeleteFilesWithComicDelete;
        [ObservableProperty] private bool isBookShelfInfoBar = Config.IsBookShelfInfoBar;
        [ObservableProperty] private bool isImportAgain = Config.IsImportAgain;

        partial void OnPluginsUriChanged(string oldValue, string newValue)
        {
            if (oldValue != newValue)
            {
                Config.PluginsUri = PluginsUri;
            }
        }
        partial void OnPluginsPathChanged(string oldValue, string newValue)
        {
            if (oldValue != newValue)
            {
                Config.PluginsPath = PluginsPath;
            }
        }
        partial void OnComicsPathChanged(string oldValue, string newValue)
        {
            if (oldValue != newValue)
            {
                Config.ComicsPath = ComicsPath;
            }
        }

        partial void OnIsDebugChanged(bool oldValue, bool newValue)
        {
            if (oldValue == newValue) return;
            Config.IsDebug = IsDebug;
            caller.Debug();
        }

        partial void OnTempPathChanged(string oldValue, string newValue)
        {
            if (oldValue != newValue)
            {
                Config.TempPath = TempPath;
            }
        }


        partial void OnIsImportAgainChanged(bool oldValue, bool newValue)
        {
            if (oldValue != newValue)
            {
                Config.IsImportAgain = IsImportAgain;
            }
        }

        partial void OnIsBookShelfInfoBarChanged(bool oldValue, bool newValue)
        {
            if (oldValue != newValue)
            {
                Config.IsBookShelfInfoBar = IsBookShelfInfoBar;
            }
        }

        partial void OnIsDeleteFilesWithComicDeleteChanged(bool oldValue, bool newValue)
        {
            if (oldValue != newValue)
            {
                Config.IsDeleteFilesWithComicDelete = IsDeleteFilesWithComicDelete;
            }
        }

        partial void OnIsRememberDeleteFilesWithComicDeleteChanged(bool oldValue, bool newValue)
        {
            if (oldValue != newValue)
            {
                Config.IsRememberDeleteFilesWithComicDelete = !IsRememberDeleteFilesWithComicDelete;
            }
        }

        public SettingsViewModel(ICallableToolKit callableToolKit)
        {
            caller = callableToolKit;
            Version = $"0.0.0.1";
        }
    }
}