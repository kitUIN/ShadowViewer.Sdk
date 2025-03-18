using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.WinUI.Animations;
using Microsoft.UI.Xaml.Controls;
using ShadowViewer.Core.Models.Interfaces;

namespace ShadowViewer.Core.Models
{
    public partial class ShadowNavigationItem : ObservableObject, IShadowNavigationItem
    {
        /// <summary />
        /// <param name="pluginId"></param>
        /// <param name="id"></param>
        /// <param name="icon"></param>
        /// <param name="content"></param>
        public ShadowNavigationItem(string pluginId, string id, IconElement? icon, object? content,
            AnimationBuilder? startAnimation = null)
        {
            Icon = icon;
            Id = id;
            Content = content;
            PluginId = pluginId;
            StartAnimation = startAnimation;
        }

        /// <summary>
        /// 内容
        /// </summary>
        [ObservableProperty] private object? content;

        /// <summary>
        /// 图标
        /// </summary>
        [ObservableProperty] private IconElement? icon;


        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public string? Id { get; }

        /// <inheritdoc />
        public AnimationBuilder? StartAnimation { get; }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public string PluginId { get; }
    }
}