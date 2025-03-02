using SqlSugar;
using ShadowViewer.Core.Models.Interfaces;

namespace ShadowViewer.Core.Models
{
    /// <summary>
    /// 标签
    /// </summary>
    public class ShadowTag : IShadowTag
    {
        /// <summary>
        /// <inheritdoc />
        /// </summary>
        [SugarColumn(ColumnDataType = "Nvarchar(255)")]
        public string Name { get; set; }


        /// <summary>
        /// <inheritdoc />
        /// </summary>

        [SugarColumn(IsPrimaryKey = true)]
        public long Id { get; set; }


        /// <summary>
        /// <inheritdoc />
        /// </summary>
        [SugarColumn(ColumnDataType = "Nvarchar(9)")]
        public string BackgroundHex { get; set; }


        /// <summary>
        /// <inheritdoc />
        /// </summary>
        [SugarColumn(ColumnDataType = "Nvarchar(9)")]
        public string ForegroundHex { get; set; }


        /// <summary>
        /// <inheritdoc />
        /// </summary>
        [SugarColumn(ColumnDescription = "图标", IsNullable = true)]
        public string? Icon { get; set; }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        [SugarColumn(ColumnDescription = "图标")]
        public string PluginId { get; set; }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public int TagType { get; set; }


        /// <summary>
        /// <inheritdoc />
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public bool AllowModify => TagType != 0;

        /// <summary>
        /// 
        /// </summary>
        public ShadowTag(string name, string backgroundHex, string foregroundHex, string? icon, string pluginId)
        {
            Name = name;
            BackgroundHex = backgroundHex;
            ForegroundHex = foregroundHex;
            Icon = icon;
            PluginId = pluginId;
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