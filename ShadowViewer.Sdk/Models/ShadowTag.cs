using CommunityToolkit.WinUI.Helpers;
using Microsoft.UI.Xaml.Media;
using ShadowPluginLoader.Attributes;
using ShadowViewer.Sdk.Models.Interfaces;


namespace ShadowViewer.Sdk.Models
{
    /// <summary>
    /// 标签
    /// </summary>
    public class ShadowTag : IShadowTag
    {
        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Id
        /// </summary>
        [Meta(Exclude = true)]
        public long Id { get; set; }


        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public string BackgroundHex { get; set; } = null!;


        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public string ForegroundHex { get; set; } = null!;

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        [Meta(Exclude = true)]
        public Brush Background => new SolidColorBrush(BackgroundHex.ToColor());

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        [Meta(Exclude = true)]
        public Brush Foreground => new SolidColorBrush(ForegroundHex.ToColor());


        /// <summary>
        /// <inheritdoc />
        /// </summary>
        [Meta(Required = false)]
        public string? Icon { get; set; }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public string PluginId { get; set; } = null!;

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        [Meta(Exclude = true)]
        public int TagType { get; set; }


        /// <summary>
        /// <inheritdoc />
        /// </summary>
        [Meta(Exclude = true)]
        public bool AllowClick { get; }

        /// <summary>
        /// 
        /// </summary>
        public ShadowTag(string name, string backgroundHex,
            string foregroundHex, string? icon, string pluginId, 
            bool allowClick = false, int tagType = 0)
        {
            Name = name;
            BackgroundHex = backgroundHex;
            ForegroundHex = foregroundHex;
            Icon = icon;
            PluginId = pluginId;
            AllowClick = allowClick;
            TagType = tagType;
        }

        /// <summary>
        /// 
        /// </summary>
        public ShadowTag()
        {
        }

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public new string ToString()
        {
            return $"ShadowTag(name={Name},foreground={ForegroundHex},background={BackgroundHex})";
        }
    }
}