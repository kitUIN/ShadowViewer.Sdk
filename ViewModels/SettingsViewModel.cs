using ShadowViewer.Configs;

namespace ShadowViewer.ViewModels
{
    /// <summary>
    /// 设置
    /// </summary>
    public partial class SettingsViewModel : ObservableObject
    {
        public ICallableToolKit caller;
        public string Version { get => "0.6.5.0"; }
        [ObservableProperty]
        private bool isDebug = Config.IsDebug;
        [ObservableProperty]
        private string comicsPath = Config.ComicsPath;
        [ObservableProperty]
        private string tempPath = Config.TempPath;
        [ObservableProperty]
        private bool isRememberDeleteFilesWithComicDelete = !Config.IsRememberDeleteFilesWithComicDelete;
        [ObservableProperty]
        private bool isDeleteFilesWithComicDelete = Config.IsDeleteFilesWithComicDelete;
        [ObservableProperty]
        private bool isBookShelfInfoBar = Config.IsBookShelfInfoBar;
        [ObservableProperty]
        private bool isImportAgain = Config.IsImportAgain;
        [ObservableProperty]
        private bool isTopBarDetail = Config.IsTopBarDetail;
        partial void OnComicsPathChanged(string oldValue, string newValue)
        {
            if(oldValue != newValue)
            {
                Config.ComicsPath = ComicsPath;
            }
        }
        partial void OnIsDebugChanged(bool oldValue, bool newValue)
        {
            if (oldValue != newValue)
            {
                Config.IsDebug = IsDebug;
                caller.SettingsBack();
            }
        }
        partial void OnTempPathChanged(string oldValue, string newValue)
        {
            if (oldValue != newValue)
            {
                Config.TempPath = TempPath;
            }
        }
        partial void OnIsTopBarDetailChanged(bool oldValue, bool newValue)
        {
            if (oldValue != newValue)
            {
                Config.IsTopBarDetail = IsTopBarDetail;
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
        public ObservableCollection<BreadcrumbItem> Pages { get; set; } = new ObservableCollection<BreadcrumbItem> ();
        public SettingsViewModel(ICallableToolKit callableToolKit) 
        {
            caller = callableToolKit;
        }
    }
}
