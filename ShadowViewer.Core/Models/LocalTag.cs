using System;
using Windows.UI;
using CommunityToolkit.WinUI.Helpers;
using DryIoc;
using Microsoft.UI.Xaml.Media;
using Serilog;
using ShadowPluginLoader.WinUI;
using SqlSugar;

namespace ShadowViewer.Core.Models
{
    /// <summary>
    /// 标签
    /// </summary>
    public class LocalTag
    {
        private string name;
        private string id;
        private long comicId;
        private SolidColorBrush foreground;
        private SolidColorBrush background;
        /// <summary>
        /// ID
        /// </summary>
        [SugarColumn(ColumnDataType = "Nchar(32)", IsPrimaryKey = true, IsNullable = false)]
        public string Id
        {
            get => id;
            set => id = value;
        }
        /// <summary>
        /// Comic ID
        /// </summary>
        public long ComicId
        {
            get => comicId;
            set => comicId = value;
        }

        [SugarColumn(IsIgnore = true)]
        public SolidColorBrush Foreground
        {
            get => foreground;
            set => foreground = value;
        }
        [SugarColumn(IsIgnore = true)]
        public SolidColorBrush Background
        {
            get => background;
            set => background = value;
        }
        [SugarColumn(ColumnDataType = "Nvarchar(2048)")]
        public string Name
        {
            get => name;
            set
            {
                if (name != value)
                {
                    name = value;
                }
            }
        }
        [SugarColumn(ColumnDataType = "Nvarchar(2048)", ColumnName = "Background")]
        public string BackgroundHex
        {
            get => background.Color.ToHex();
            set => background = new SolidColorBrush(value.ToColor());
        }
        [SugarColumn(ColumnDataType = "Nvarchar(2048)", ColumnName = "Foreground")]
        public string ForegroundHex
        {
            get => foreground.Color.ToHex();
            set => foreground = new SolidColorBrush(value.ToColor());
        }
        public LocalTag(string name, SolidColorBrush foreground, SolidColorBrush background)
        {
            id = RandomId();
            this.name = name;
            this.foreground = foreground;
            this.background = background;
        }
        public LocalTag(string name, Color foreground, Color background) :
            this(name, new SolidColorBrush(foreground),
                new SolidColorBrush(background))
        { }
        public LocalTag(string name, string foreground, string background) :
            this(name, new SolidColorBrush(foreground.ToColor()),
                new SolidColorBrush(background.ToColor()))
        { }
        public LocalTag() { }
        public LocalTag Copy()
        {
            return new LocalTag(Name, foreground, background);
        }
        /// <summary>
        /// 随机ID
        /// </summary>
        /// <returns></returns>
        public static string RandomId()
        {
            string id = Guid.NewGuid().ToString("N");
            var db = DiFactory.Services.Resolve<ISqlSugarClient>();
            while (db.Queryable<LocalTag>().Any(x => x.Id == id))
            {
                id = Guid.NewGuid().ToString("N");
            }
            return id;
        }
        /// <summary>
        /// 能否修改
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public bool IsEnable { get; set; } = true;
        [SugarColumn(IsIgnore = true)]
        public string ToolTip { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string Icon { get; set; }
        public new string ToString()
        {
            return $"LocalTag(name={name},foreground={ForegroundHex},background={BackgroundHex})";
        }

        [SugarColumn(IsIgnore = true)]
        public static ILogger Logger { get; } = Log.ForContext<LocalTag>();
    }
}
