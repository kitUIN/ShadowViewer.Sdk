using CommunityToolkit.WinUI.Helpers;
using Microsoft.UI.Xaml.Media;
using ShadowPluginLoader.MetaAttributes;
using ShadowViewer.Core.Models.Interfaces;
using SqlSugar;

namespace ShadowViewer.Core.Models
{
    /// <summary>
    /// 标签
    /// </summary>
    [SugarIndex("unique_shadow_tag_name", nameof(Name), OrderByType.Asc, true)]
    public class ShadowTag : IShadowTag
    {
        /// <summary>
        /// <inheritdoc />
        /// </summary>
        [SugarColumn(ColumnDataType = "Nvarchar(255)", IsNullable = false)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        [Meta(Exclude = true)]
        public long Id { get; set; }


        /// <summary>
        /// <inheritdoc />
        /// </summary>
        [SugarColumn(ColumnDataType = "Nvarchar(9)")]
        public string BackgroundHex { get; set; } = null!;


        /// <summary>
        /// <inheritdoc />
        /// </summary>
        [SugarColumn(ColumnDataType = "Nvarchar(9)")]
        public string ForegroundHex { get; set; } = null!;

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Meta(Exclude = true)]
        public Brush Background => new SolidColorBrush(BackgroundHex.ToColor());

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Meta(Exclude = true)]
        public Brush Foreground => new SolidColorBrush(ForegroundHex.ToColor());


        /// <summary>
        /// <inheritdoc />
        /// </summary>
        [SugarColumn(ColumnDescription = "图标", IsNullable = true)]
        [Meta(Required = false)]
        public string? Icon { get; set; }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        [SugarColumn(ColumnDescription = "图标")]
        public string PluginId { get; set; } = null!;

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        [Meta(Exclude = true)]
        public int TagType { get; set; }


        /// <summary>
        /// <inheritdoc />
        /// </summary>
        [SugarColumn(IsIgnore = true)]
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