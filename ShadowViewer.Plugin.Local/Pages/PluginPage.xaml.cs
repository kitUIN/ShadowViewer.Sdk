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

namespace ShadowViewer.Plugin.Local.Pages
{
    
    public sealed partial class PluginPage : Page
    {
        public PluginService PluginService { get; }
        public PluginPage()
        {
            this.InitializeComponent();
            PluginService = DiFactory.Services.Resolve<PluginService>();
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
    }
}
