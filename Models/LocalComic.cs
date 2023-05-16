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
        [SugarColumn(ColumnDataType = "Nchar(32)", IsPrimaryKey = true, IsNullable =false)]
        public string Id
        {
            get => id;
            set => SetProperty(ref id, value,  propertyName: nameof(Id));
        }
        /// <summary>
        /// 名称
        /// </summary>
        [SugarColumn(ColumnDataType = "Nvarchar(2048)")]
        public string Name
        {
            get => name;
            set
            {   var oldValue = name;
                SetProperty(ref name, value, propertyName: nameof(Name));
                if(oldValue != null)
                {
                    OnChanged();
                }
            }
        }
        /// <summary>
        /// 作者
        /// </summary>
        [SugarColumn(ColumnDataType = "Nvarchar(2048)")]
        public string Author
        {
            get => author;
            set
            {
                var oldValue = author;
                SetProperty(ref author, value, propertyName: nameof(Author));
                if (oldValue != null)
                {
                    OnChanged();
                }
            }

        }
        /// <summary>
        /// 缩略图地址
        /// </summary>
        [SugarColumn(ColumnDataType = "Nvarchar(2048)")]
        public string Img
        {
            get => img;
            set
            {
                var oldValue = img;
                SetProperty(ref img, value, propertyName: nameof(Img));
                if (oldValue != null)
                {
                    OnChanged();
                }
            }
        }
        /// <summary>
        /// 阅读进度(0-100%)
        /// </summary>
        [SugarColumn(ColumnDataType = "Nvarchar(5)")]
        public string Percent
        {
            get => percent;
            set
            {
                var oldValue = percent;
                SetProperty(ref percent, value, propertyName: nameof(Percent));
                if (oldValue != null)
                {
                    OnChanged();
                }
            }
        }
        /// <summary>
        /// 汉化组
        /// </summary>
        [SugarColumn(ColumnDataType = "Nvarchar(2048)")]
        public string Group
        {
            get => group;
            set
            {
                var oldValue = group;
                SetProperty(ref group, value, propertyName: nameof(Group));
                if (oldValue != null)
                {
                    OnChanged();
                }
            }
        }
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnDataType = "Ntext")]
        public string Remark
        {
            get => remark;
            set
            {
                var oldValue = remark;
                SetProperty(ref remark, value, propertyName: nameof(Remark));
                if (oldValue != null)
                {
                    OnChanged();
                }
            }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get => createTime;
            set
            {
                var oldValue = createTime;
                SetProperty(ref createTime, value, propertyName: nameof(CreateTime));
                if (oldValue != default)
                {
                    OnChanged();
                }
            }
        }
        /// <summary>
        /// 最后阅读时间
        /// </summary>
        public DateTime LastReadTime
        {
            get => lastReadTime;
            set
            {
                var oldValue = createTime;
                SetProperty(ref lastReadTime, value, propertyName: nameof(LastReadTime));
                if (oldValue != default)
                {
                    OnChanged();
                }
            }
        }

        /// <summary>
        /// 父文件夹
        /// </summary>
        [SugarColumn(ColumnDataType = "Nvarchar(2048)")]
        public string Parent
        {
            get => parent;
            set
            {
                var oldValue = parent;
                SetProperty(ref parent, value, propertyName: nameof(Parent));
                if (oldValue != default)
                {
                    OnChanged();
                }
            }
        }
        private ObservableCollection<string> tags = new ObservableCollection<string>();
        /// <summary>
        /// 标签
        /// </summary>
        [SugarColumn(IsJson = true, ColumnDataType = "Ntext")]
        public ObservableCollection<string> Tags
        {
            get => tags;
            set{
                tags = value;
                if (tags != null)
                {
                    tags.CollectionChanged += Tags_CollectionChanged;
                }
            }
        }
        /// <summary>
        /// 所属
        /// </summary>
        [SugarColumn(ColumnDataType = "Nvarchar(2048)")]
        public string Affiliation
        {
            get => affiliation;
            set
            {
                var oldValue = affiliation;
                SetProperty(ref affiliation, value, propertyName: nameof(Affiliation));
                if (oldValue != default)
                {
                    OnChanged();
                }
            }
        }
        /// <summary>
        /// 链接对象
        /// </summary>
        [SugarColumn(ColumnDataType = "Nvarchar(2048)")]
        public string Link
        {
            get => link;
            set
            {
                var oldValue = link;
                SetProperty(ref link, value, propertyName: nameof(Link));
                if (oldValue != default)
                {
                    OnChanged();
                }
            }
        }
        /// <summary>
        /// 文件大小
        /// </summary>
        public long Size
        {
            get => size;
            set 
            {
                var oldValue = size;
                SetProperty(ref size, value, propertyName: nameof(Size));
                if (oldValue != default)
                {
                    OnChanged();
                }
                SizeString = ShowSize(size);
            }
        }
        /// <summary>
        /// 文件大小(String)
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string SizeString
        {
            get => sizeString;
            set => SetProperty(ref sizeString, value,  propertyName: nameof(SizeString));
        }
        /// <summary>
        /// 是否是文件夹
        /// </summary>
        public bool IsFolder
        {
            get => isFolder;
            set => SetProperty(ref isFolder, value,  propertyName: nameof(IsFolder));
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
        public LocalComic() {  }
        private void Tags_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            DBHelper.Update(this);
        }
        private void OnChanged()
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
