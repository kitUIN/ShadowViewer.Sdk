using System;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml;
using DryIoc;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Serilog;
using ShadowViewer.Extensions;
using ShadowViewer.Helpers;
using ShadowViewer.Plugin.Local.Enums;
using ShadowViewer.Plugin.Local.Helpers;
using ShadowViewer.Plugins;
using ShadowViewer.Services;
using Windows.Storage.Pickers;
using System.Collections.Generic;
using Windows.Storage;
using ShadowViewer.Interfaces;
using ShadowViewer.Services.Interfaces;

namespace ShadowViewer.Plugin.Local.Pages
{
    
    public sealed partial class PluginPage : Page
    {
        public IPluginService PluginService { get; } = DiFactory.Services.Resolve<IPluginService>();
        public ICallableService Caller { get; } = DiFactory.Services.Resolve<ICallableService>();
        public PluginPage()
        {
            this.InitializeComponent();
        }
        
        /// <summary>
        /// 前往插件设置
        /// </summary>
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as HyperlinkButton;
            if(button!=null&& button.Tag is string tag &&PluginService.GetPlugin(tag) is { SettingsPage: not null } plugin)
            {
                Frame.Navigate(plugin.SettingsPage, null,
                    new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromRight });
            }
        }

        private   void Delete_Click(object sender, RoutedEventArgs e)
        {
            /*if (sender is not FrameworkElement { Tag: IPlugin plugin }) return;
            var contentDialog= XamlHelper.CreateMessageDialog(XamlRoot,
                LocalResourcesHelper.GetString(LocalResourceKey.DeletePlugin) + plugin.MetaData.Name,
                LocalResourcesHelper.GetString(LocalResourceKey.DeletePluginMessage));
            contentDialog.IsPrimaryButtonEnabled = true;
            contentDialog.DefaultButton = ContentDialogButton.Close;
            contentDialog.PrimaryButtonText = LocalResourcesHelper.GetString(LocalResourceKey.Confirm) ;
            contentDialog.PrimaryButtonClick += async (dialog, args) =>
            {
                var flag = await PluginService.DeleteAsync(plugin.MetaData.Id);
                    
            };
            await contentDialog.ShowAsync();
            */
        }

        private void More_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not FrameworkElement source) return;
            if (sender is FrameworkElement { Tag: IPlugin { CanOpenFolder: false, CanDelete: false } })return;
            var flyout = FlyoutBase.GetAttachedFlyout(source);
            flyout?.ShowAt(source);
        }

        private async void OpenFolder_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not FrameworkElement { Tag: IPlugin plugin }) return;
            try
            {
                var file = await plugin.GetType().Assembly.Location.GetFile();
                var folder = await file.GetParentAsync();
                folder.LaunchFolderAsync();
            }
            catch(Exception ex)
            {
                Log.Error("打开文件夹错误{Ex}", ex);
            }
        }

        private void More_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement { Tag: IPlugin { CanOpenFolder: false, CanDelete: false } } source)
            {
                source.Visibility = Visibility.Collapsed;
            }
        }

        private async void AddPluginButton_Click(object sender, RoutedEventArgs e)
        {
            var file = await FileHelper.SelectFileAsync(XamlRoot, PickerLocationId.Downloads, PickerViewMode.List, ".zip", ".rar", ".7z", ".tar");
            if (file != null)
            {
                Caller.ImportPlugin(this, new List<StorageFile>{ file });
            }
        }

        private void GoPluginTip_Click(object sender, RoutedEventArgs e)
        {
            var url = new Uri("https://github.com/kitUIN/ShadowViewer/blob/master/README.md#%E6%8F%92%E4%BB%B6%E5%88%97%E8%A1%A8");
            url.LaunchUriAsync();
        }
    }
}
