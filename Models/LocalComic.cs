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
        private int episodeCounts;
        private int counts;
        private string sizeString;
        private bool isFolder = false;
        private bool isTemp = false;
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
            {   string oldValue = name;
                SetProperty(ref name, value, propertyName: nameof(Name));
                if(oldValue != null && oldValue != value)
                {
                    Update();
                    Log.Information("Comic[{Id}]{Field}({Old}->{New})", Id, nameof(Name), oldValue, Name);
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
                string oldValue = author;
                SetProperty(ref author, value, propertyName: nameof(Author));
                if (oldValue != null && oldValue != value)
                {
                    Update();
                    Log.Information("Comic[{Id}]{Field}({Old}->{New})", Id, nameof(Author), oldValue, Author);
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
                string oldValue = img;
                SetProperty(ref img, value, propertyName: nameof(Img));
                if (oldValue != null && oldValue != value)
                {
                    Update();
                    Log.Information("Comic[{Id}]{Field}({Old}->{New})", Id, nameof(Img), oldValue, Img);
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
                string oldValue = percent;
                SetProperty(ref percent, value, propertyName: nameof(Percent));
                if (oldValue != null && oldValue != value)
                {
                    Update();
                    Log.Information("Comic[{Id}]{Field}({Old}->{New})", Id, nameof(Percent), oldValue, Percent);
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
                string oldValue = group;
                SetProperty(ref group, value, propertyName: nameof(Group));
                if (oldValue != null && oldValue != value)
                {
                    Update();
                    Log.Information("Comic[{Id}]{Field}({Old}->{New})", Id, nameof(Group), oldValue, Group);
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
                string oldValue = remark;
                SetProperty(ref remark, value, propertyName: nameof(Remark));
                if (oldValue != null && oldValue != value)
                {
                    Update();
                    Log.Information("Comic[{Id}]{Field}({Old}->{New})", Id, nameof(Remark), oldValue, Remark);
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
                DateTime oldValue = createTime;
                SetProperty(ref createTime, value, propertyName: nameof(CreateTime));
                if (oldValue != default && oldValue!= value)
                {
                    Update();
                    Log.Information("Comic[{Id}]{Field}({Old}->{New})", Id, nameof(CreateTime), oldValue, CreateTime);
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
                DateTime oldValue = lastReadTime;
                SetProperty(ref lastReadTime, value, propertyName: nameof(LastReadTime));
                if (oldValue != default && oldValue!= value)
                {
                    Update();
                    Log.Information("Comic[{Id}]{Field}({Old}->{New})", Id, nameof(LastReadTime), oldValue, LastReadTime);
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
                string oldValue = parent;
                SetProperty(ref parent, value, propertyName: nameof(Parent));
                if (oldValue != default && oldValue!= value)
                {
                    Update();
                    Log.Information("Comic[{Id}]{Field}({Old}->{New})", Id, nameof(Parent), oldValue, Parent);
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
                    Log.Debug("添加Tag响应");
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
                string oldValue = affiliation;
                SetProperty(ref affiliation, value, propertyName: nameof(Affiliation));
                if (oldValue != default && oldValue!= value)
                {
                    Update();
                    Log.Information("Comic[{Id}]{Field}({Old}->{New})", Id, nameof(Affiliation), oldValue, Affiliation);
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
                string oldValue = link;
                SetProperty(ref link, value, propertyName: nameof(Link));
                if (oldValue != default && oldValue!= value)
                {
                    Update();
                    Log.Information("Comic[{Id}]{Field}({Old}->{New})", Id, nameof(Link), oldValue, Link);
                }
            }
        }
        
        public bool IsTemp
        {
            get => isTemp;
            set
            {
                bool oldValue = isTemp;
                SetProperty(ref isTemp, value, propertyName: nameof(IsTemp));
                if (oldValue != default && oldValue!= value)
                {
                    Update();
                    Log.Information("Comic[{Id}]{Field}({Old}->{New})", Id, nameof(IsTemp), oldValue.ToString(), IsTemp.ToString());
                }
            }
        }
        public int EpisodeCounts
        {
            get => episodeCounts;
            set
            {
                int oldValue = episodeCounts;
                SetProperty(ref episodeCounts, value, propertyName: nameof(EpisodeCounts));
                if (oldValue != default && oldValue!= value)
                {
                    Update();
                    Log.Information("Comic[{Id}]{Field}({Old}->{New})", Id, nameof(EpisodeCounts), oldValue, EpisodeCounts);
                }
            }
        }
        public int Counts
        {
            get => counts;
            set
            {
                int oldValue = counts;
                SetProperty(ref counts, value, propertyName: nameof(Counts));
                if (oldValue != default && oldValue!= value)
                {
                    Update();
                    Log.Information("Comic[{Id}]{Field}({Old}->{New})", Id, nameof(Counts), oldValue, Counts);
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
                long oldValue = size;
                SetProperty(ref size, value, propertyName: nameof(Size));
                if (oldValue != default && oldValue!= value)
                {
                    Update();
                    Log.Information("Comic[{Id}]{Field}({Old}->{New})", Id, nameof(Size), oldValue, Size);
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
            string percent="0%", string tags = "" , string affiliation = "Local", string img="", long size=0, bool isFolder=false,bool isTemp=false)
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
            this.isTemp = isTemp;
            Tags.CollectionChanged += Tags_CollectionChanged;
        }
        public LocalComic() {  }
        private void Tags_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
           Update();
        }
        public void Update()
        {
            DBHelper.Update(this);
            Log.Information("更新Comic:{ComicId}", Id);
        }
        public void Add()
        {
            DBHelper.Add(this);
            Log.Information("添加Comic:{ComicId}", Id);
        }
        private static ObservableCollection<string> LoadTags(string tags)
        {
            HashSet<string> res = new HashSet<string>();
            foreach (string tag in tags.Split(","))
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
