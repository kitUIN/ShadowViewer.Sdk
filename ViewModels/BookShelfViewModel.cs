namespace ShadowViewer.ViewModels
{
    public partial class BookShelfViewModel: ObservableObject
    {
        /// <summary>
        /// 该文件夹内是否为空
        /// </summary>
        [ObservableProperty]
        private bool isEmpty = true;
        /// <summary>
        /// 文件夹内总数量
        /// </summary>
        [ObservableProperty]
        private int folderTotalCounts;
        /// <summary>
        /// 当前文件夹名称
        /// </summary>
        public string CurrentName { get; private set; }
        /// <summary>
        /// 当前文件夹ID
        /// </summary>
        public string Path { get; private set; } = "local";
        /// <summary>
        /// 原始地址
        /// </summary>
        public Uri OriginPath { get; private set; }
        /// <summary>
        /// 排序-<see cref="ShadowSorts"/>
        /// </summary>
        public ShadowSorts Sorts { get; set; } = ShadowSorts.RZ;
        /// <summary>
        /// 该文件夹下的漫画
        /// </summary>
        public ObservableCollection<LocalComic> LocalComics { get; } = new ObservableCollection<LocalComic>();

        private static ILogger Logger { get; } = Log.ForContext<BookShelfViewModel>();
        private readonly ICallableToolKit caller;
        public BookShelfViewModel(ICallableToolKit callableToolKit)
        {
            caller = callableToolKit;
            caller.RefreshBookEvent += Caller_RefreshBookEvent;
            Logger.Debug("加载RefreshBook事件");
        }
        private void Caller_RefreshBookEvent(object sender, EventArgs e)
        {
            RefreshLocalComic();
        }
        public void Init(Uri parameter)
        {
            LocalComics.CollectionChanged += LocalComics_CollectionChanged;
            OriginPath = parameter;
            Path = parameter.AbsolutePath.Split(new char[] { '/', }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault() ?? parameter.Host;
            Logger.Information("导航到{Path},Path={P}", OriginPath, Path);
            RefreshLocalComic();
            if(Path == "local")
            {
                CurrentName = "本地";
            }
            else
            {
                CurrentName = DBHelper.Db.Queryable<LocalComic>().First(x => x.Id == Path).Name;
            }
        }

        private void LocalComics_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                {
                    if (e.OldItems != null)
                        foreach (LocalComic item in e.OldItems)
                        {
                            item.Remove();
                        }

                    break;
                }
                case NotifyCollectionChangedAction.Add:
                {
                    if (e.NewItems != null)
                        foreach (LocalComic item in e.NewItems)
                        {
                            if (item.Id is null) continue;
                            if (!DBHelper.Db.Queryable<LocalComic>().Any(x => x.Id == item.Id))
                            {
                                item.Add();
                            }
                        }

                    break;
                }
            }
            IsEmpty = LocalComics.Count == 0;
            FolderTotalCounts = LocalComics.Count;
        }
        /// <summary>
        /// 刷新
        /// </summary>
        public void RefreshLocalComic()
        {
            LocalComics.Clear();
            var comics = DBHelper.Db.Queryable<LocalComic>().Where(x => x.Parent == Path).ToList();
            switch (Sorts)
            {
                case ShadowSorts.AZ:
                    comics.Sort(ComicHelper.AZSort); break;
                case ShadowSorts.ZA:
                    comics.Sort(ComicHelper.ZASort); break;
                case ShadowSorts.CA:
                    comics.Sort(ComicHelper.CASort); break;
                case ShadowSorts.CZ:
                    comics.Sort(ComicHelper.CZSort); break;
                case ShadowSorts.RA:
                    comics.Sort(ComicHelper.RASort); break;
                case ShadowSorts.RZ:
                    comics.Sort(ComicHelper.RZSort); break;
                case ShadowSorts.PA:
                    comics.Sort(ComicHelper.PASort); break;
                case ShadowSorts.PZ:
                    comics.Sort(ComicHelper.PZSort); break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            foreach (var item in comics)
            {
                LocalComics.Add(item);
            }
        }
    }
}
