using DryIoc;
using SqlSugar;
using System;
using System.Diagnostics;

namespace ShadowViewer.ViewModels
{
    public partial class PicViewModel : ObservableObject
    {
        
        private static ILogger Logger = Log.ForContext<PicViewModel>();
        public ObservableCollection<ShadowPicture> Images { get; set; } = new ObservableCollection<ShadowPicture>();
        public LocalComic Comic { get; private set; }
        [ObservableProperty]
        private int currentEpisodeIndex=-1;
        public ObservableCollection<IShadowEpisode> Episodes { get;   } = new ObservableCollection<IShadowEpisode>();
        [ObservableProperty]
        private int maximumColumns = 1;
        [ObservableProperty]
        private int imageWidth = 800;
        [ObservableProperty]
        private int currentPage = 1;
        [ObservableProperty]
        private bool isMenu;
        public string Affiliation { get; set; }
        private ISqlSugarClient Db { get; }
        public PicViewModel(ISqlSugarClient sqlSugarClient) 
        {
            Db = sqlSugarClient;
        }

        public void Init(PicViewArg arg)
        {
            Affiliation = arg.Affiliation;
            Images.Clear();
            Episodes.Clear();
            if (arg.Affiliation == "Local" && arg.Parameter is LocalComic comic)
            {
                LoadImageFormLocalComic(comic);
            }
            else
            {

            }
        }
        /// <summary>
        /// 从本地漫画加载图片
        /// </summary>
        private void LoadImageFormLocalComic(LocalComic comic)
        {
            Comic = comic;
            var orders = new List<int>();
            Db.Queryable<LocalEpisode>().Where(x => x.ComicId == Comic.Id).OrderBy(x => x.Order).ForEach(x =>
            {
                orders.Add(x.Order);
                Episodes.Add(new ShadowEpisode(){ Body = x,Title = x.Name});
            });
            orders.Sort();
            if (CurrentEpisodeIndex == -1) 
                CurrentEpisodeIndex = orders[0];
        }
        private void LoadLocalImage( PicViewModel viewModel, int oldValue, int newValue)
        {
            if(oldValue ==  newValue) return;
            Images.Clear();
            var index = 0;
            if (viewModel.Episodes.Count > 0 && viewModel.Episodes[newValue].Body is LocalEpisode episode)
            {
                Debug.WriteLine(episode.Order);
                foreach (LocalPicture item in Db.Queryable<LocalPicture>().Where(x => x.EpisodeId == episode.Id).OrderBy(x => x.Name).ToList())
                {
                    viewModel.Images.Add(new ShadowPicture(++index, item.Img));
                }
            }
        }
        partial void OnCurrentEpisodeIndexChanged(int oldValue, int newValue)
        {
            
            if (Affiliation == "Local")
            {
                
                LoadLocalImage( this,oldValue,newValue);
            }
        }
        
    }
}
