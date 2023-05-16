

using SqlSugar;

namespace ShadowViewer.Models
{
    public partial class LocalComic: ObservableObject
    {
        #region Private Field
        private string id;
        private string name;
        private string author;
        private string img;
        private string percent;
        private string group;
        private string remark;
        private DateTime createTime;
        private DateTime lastReadTime;
        private string parent;
        private string affiliation;
        private string link;
        private long size;
        private string sizeString;
        private bool isFolder = false;
        #endregion
        #region SQL 实体访问器
        /// <summary>
        /// ID
        /// </summary>
        [SugarColumn(ColumnDataType = "Nchar(32)", IsPrimaryKey = true)]
        public string Id
        {
            get => id;
            set => SetProperty(id, value, callback: OnChanged, propertyName: nameof(Id));
        }
        /// <summary>
        /// 名称
        /// </summary>
        [SugarColumn(ColumnDataType = "Nvarchar(2048)")]
        public string Name
        {
            get => name;
            set => SetProperty(name, value, callback: OnChanged, propertyName: nameof(Name));
        }
        /// <summary>
        /// 作者
        /// </summary>
        [SugarColumn(ColumnDataType = "Nvarchar(2048)")]
        public string Author
        {
            get => author;
            set => SetProperty(author, value, callback: OnChanged, propertyName: nameof(Author));
        }
        /// <summary>
        /// 缩略图地址
        /// </summary>
        [SugarColumn(ColumnDataType = "Nvarchar(2048)")]
        public string Img
        {
            get => img;
            set => SetProperty(img, value, callback: OnChanged, propertyName: nameof(Img));
        }
        /// <summary>
        /// 阅读进度(0-100%)
        /// </summary>
        [SugarColumn(ColumnDataType = "Nvarchar(5)")]
        public string Percent
        {
            get => percent;
            set => SetProperty(percent, value, callback: OnChanged, propertyName: nameof(Percent));
        }
        /// <summary>
        /// 汉化组
        /// </summary>
        [SugarColumn(ColumnDataType = "Nvarchar(2048)")]
        public string Group
        {
            get => group;
            set => SetProperty(group, value, callback: OnChanged, propertyName: nameof(Group));
        }
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnDataType = "Ntext")]
        public string Remark
        {
            get => remark;
            set => SetProperty(remark, value, callback: OnChanged, propertyName: nameof(Remark));
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get => createTime;
            set => SetProperty(createTime, value, callback: OnChanged, propertyName: nameof(CreateTime));
        }
        /// <summary>
        /// 最后阅读时间
        /// </summary>
        public DateTime LastReadTime
        {
            get => lastReadTime;
            set => SetProperty(lastReadTime, value, callback: OnChanged, propertyName: nameof(LastReadTime));
        }

        /// <summary>
        /// 父文件夹
        /// </summary>
        [SugarColumn(ColumnDataType = "Nvarchar(2048)")]
        public string Parent
        {
            get => parent;
            set => SetProperty(parent, value, callback: OnChanged, propertyName: nameof(Parent));
        }
        /// <summary>
        /// 标签
        /// </summary>
        [SugarColumn(IsJson = true, ColumnDataType = "Ntext")]
        public ObservableCollection<string> Tags { get; } = new ObservableCollection<string>(); 
        /// <summary>
        /// 所属
        /// </summary>
        [SugarColumn(ColumnDataType = "Nvarchar(2048)")]
        public string Affiliation
        {
            get => affiliation;
            set => SetProperty(affiliation, value, callback: OnChanged, propertyName: nameof(Affiliation));
        }
        /// <summary>
        /// 链接对象
        /// </summary>
        [SugarColumn(ColumnDataType = "Nvarchar(2048)")]
        public string Link
        {
            get => link;
            set => SetProperty(link, value, callback: OnChanged, propertyName: nameof(Link));
        }
        /// <summary>
        /// 文件大小
        /// </summary>
        public long Size
        {
            get => size;
            set => SetProperty(size, value, callback: OnChanged, propertyName: nameof(Size));
        }
        /// <summary>
        /// 文件大小(String)
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string SizeString
        {
            get => sizeString;
            set => SetProperty(sizeString, value, callback: OnChanged, propertyName: nameof(SizeString));
        }
        /// <summary>
        /// 是否是文件夹
        /// </summary>
        public bool IsFolder
        {
            get => isFolder;
            set => SetProperty(isFolder, value, callback: OnChanged, propertyName: nameof(IsFolder));
        }
        #endregion

        public LocalComic(string id, string name, DateTime createTime,
            DateTime lastReadTime, string link,string remark = "",string group = "", string author="", string parent = "local",
            string percent="0%", string tags = "" , string affiliation = "Local", string img="", long size=0, bool isFolder=false)
        {
            this.id = id;
            this.name = name;
            this.author = author;
            this.group  = group;
            this.img = img;
            this.percent = percent;
            this.remark = remark;
            this.createTime = createTime;
            this.lastReadTime = lastReadTime;
            this.parent = parent;
            Tags = LoadTags(tags);
            this.affiliation = affiliation;
            this.link = link;
            this.size = size;
            this.sizeString = ShowSize(size);
            this.isFolder = isFolder;
            Tags.CollectionChanged += Tags_CollectionChanged;
        }
        public LocalComic() { }
        private void Tags_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            DBHelper.Update(this);
        }
        private void OnChanged(string newValue)
        {
            DBHelper.Update(this);
        }
        private void OnChanged(DateTime obj)
        {
            DBHelper.Update(this);
        }
        private void OnChanged(long obj)
        {
            DBHelper.Update(this);
        }
        private void OnChanged(bool obj)
        {
            DBHelper.Update(this);
        }
        private static ObservableCollection<string> LoadTags(string tags)
        {
            var res = new HashSet<string>();
            foreach (var tag in tags.Split(","))
            {
                if (tag != "")
                {
                    res.Add(tag);
                }
            }
            return new ObservableCollection<string>(res);
        }
         
        private string ShowSize(long size)
        {
            long KB = 1024;
            long MB = KB * 1024;
            long GB = MB * 1024;
            if (size / GB >= 1)
            {
                return $"{Math.Round(size / (float)GB, 2)} GB";
            }
            else if (size / MB >= 1)
            {
                return $"{Math.Round(size / (float)MB, 2)} MB";
            }
            else if (size / KB >= 1)
            {
                return $"{Math.Round(size / (float)KB, 2)} KB";
            }
            return $"{size} B";
        }
        [SugarColumn(IsIgnore = true)]
        public string Path 
        { 
            get
            {
                if (Parent == "local")
                {
                    return "shadow://local/" + Id;
                }
                else
                {
                    return "shadow://local/" + Parent + "/" + Id;
                }
            } 
        }
        

    }
}
