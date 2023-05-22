using Serilog.Core;
using SqlSugar;

namespace ShadowViewer.Models
{
    public class ShadowTag
    {
        private string name;
        private SolidColorBrush foreground;
        private SolidColorBrush background;
        [SugarColumn(IsIgnore = true)]
        public SolidColorBrush Foreground
        {
            get { return foreground; }
            set { foreground = value; }
        }
        [SugarColumn(IsIgnore = true)]
        public SolidColorBrush Background
        {
            get { return background; }
            set { background = value; }
        }
        [SugarColumn(ColumnDataType = "Nvarchar(2048)", IsPrimaryKey = true)]
        public string Name { 
            get => name;
            set
            {
                if(name != value)
                {
                    name = value;
                }
            }
        }
        [SugarColumn(ColumnDataType = "Nvarchar(2048)",ColumnName = "Background")]
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
        public ShadowTag(string name, SolidColorBrush foreground , SolidColorBrush background)
        {
            this.name = name;
            this.foreground = foreground;
            this.background = background;
        }
        public ShadowTag(string name, Color foreground, Color background) : 
            this(name, new SolidColorBrush(foreground),
                new SolidColorBrush(background)) { }
        public ShadowTag( string name, string foreground, string background) :
            this(name,new SolidColorBrush(foreground.ToColor()),
                new SolidColorBrush(background.ToColor())) { }
        public ShadowTag() { }
        public override string ToString()
        {
            return name;
        }
        public string Log()
        {
            return $"ShadowTag(name={name},foreground={ForegroundHex},background={BackgroundHex})";
        }  
        public void Add()
        {
            DBHelper.Add(this);
            Logger.Information("添加{Log}", Log());
        }
        public void Remove()
        {
            DBHelper.Remove(new ShadowTag { Name = this.Name });
            Logger.Information("删除ShadowTag:{Name}", Name);
        }
        public static void Remove(ShadowTag tag)
        {
            tag.Remove();
        }
        public static ILogger Logger { get; } = Serilog.Log.ForContext<ShadowTag>();
    }
}
