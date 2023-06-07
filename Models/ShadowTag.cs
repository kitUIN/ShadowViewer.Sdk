using Serilog.Core;
using SqlSugar;

namespace ShadowViewer.Models
{
    public class ShadowTag: IDataBaseItem
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

        /// <summary>
        /// 能否修改
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public bool IsEnable { get; set; } = true;
        /// <summary>
        /// 是否显示图标
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public bool IsIcon { get; set; } = false;
        /// <summary>
        /// 图标
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string Icon { get; set; }
        public string Log()
        {
            return $"ShadowTag(name={name},foreground={ForegroundHex},background={BackgroundHex})";
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Add()
        {
            if (!Query().Any(x => x.Name == Name))
            {
                DBHelper.Add(this);
                Logger.Information("添加{Log}", Log());
            }
            else
            {
                Update();
            }
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Update()
        {
            DBHelper.Update(this);
            Logger.Information("更新{Log}", Log());
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Remove()
        {
            Remove(this.Name);
        }
        /// <summary>
        /// 从数据库中删除
        /// </summary>
        public static void Remove(string name)
        {
            DBHelper.Remove(new ShadowTag { Name = name });
            Logger.Information("删除ShadowTag:{Name}", name);
        }
        public static ISugarQueryable<ShadowTag> Query()
        {
            return DBHelper.Db.Queryable<ShadowTag>();
        }
        [SugarColumn(IsIgnore = true)]
        public static ILogger Logger { get; } = Serilog.Log.ForContext<ShadowTag>();
    }
}
