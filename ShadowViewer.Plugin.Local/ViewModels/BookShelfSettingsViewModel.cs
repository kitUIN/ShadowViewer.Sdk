using CommunityToolkit.Mvvm.ComponentModel;
using ShadowViewer.Configs;
using ShadowViewer.Helpers;
using ShadowViewer.Plugin.Local.Enums;

namespace ShadowViewer.Plugin.Local.ViewModels
{
    partial class BookShelfSettingsViewModel : ObservableObject
    {
        /// <summary>
        /// 删除二次确认
        /// </summary>
        [ObservableProperty]
        private bool isRememberDeleteFilesWithComicDelete = !ConfigHelper.GetBoolean("IsRememberDeleteFilesWithComicDelete");

        [ObservableProperty] private bool isDeleteFilesWithComicDelete = ConfigHelper.GetBoolean("IsDeleteFilesWithComicDelete");
        [ObservableProperty] private bool isBookShelfInfoBar = ConfigHelper.GetBoolean("IsBookShelfInfoBar");
        [ObservableProperty] private bool isImportAgain = ConfigHelper.GetBoolean("IsImportAgain");

        partial void OnIsImportAgainChanged(bool oldValue, bool newValue)
        {
            if (oldValue != newValue)
            {
                ConfigHelper.Set(LocalSettingKey.IsRememberDeleteFilesWithComicDelete, IsRememberDeleteFilesWithComicDelete);
            }   
        }

        partial void OnIsBookShelfInfoBarChanged(bool oldValue, bool newValue)
        {
            if (oldValue != newValue)
            {
                ConfigHelper.Set(LocalSettingKey.IsBookShelfInfoBar, IsBookShelfInfoBar);
            }
        }
        partial void OnIsDeleteFilesWithComicDeleteChanged(bool oldValue, bool newValue)
        {
            if (oldValue != newValue)
            {
                ConfigHelper.Set(LocalSettingKey.IsDeleteFilesWithComicDelete, IsDeleteFilesWithComicDelete);
            }
        }

        partial void OnIsRememberDeleteFilesWithComicDeleteChanged(bool oldValue, bool newValue)
        {
            if (oldValue != newValue)
            {
                ConfigHelper.Set(LocalSettingKey.IsRememberDeleteFilesWithComicDelete, IsRememberDeleteFilesWithComicDelete);
            }
        }
    }
}
