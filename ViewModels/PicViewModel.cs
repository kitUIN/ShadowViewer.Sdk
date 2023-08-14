using DryIoc;
using SqlSugar;
using System;
using System.Diagnostics;

namespace ShadowViewer.ViewModels
{
    public partial class PicViewModel : ObservableObject
    {
        
        private ILogger Logger { get; }
        public ObservableCollection<IShadowPicture> Images { get; set; } = new ObservableCollection<IShadowPicture>();
        public LocalComic Comic { get; private set; }
        [ObservableProperty]
        private int currentEpisodeIndex=-1;
        public ObservableCollection<IShadowEpisode> Episodes { get;   } = new ObservableCollection<IShadowEpisode>();
        [ObservableProperty]
        private int currentPage = 1;
        [ObservableProperty]
        private bool isMenu;
        public string Affiliation { get; set; }
        private ISqlSugarClient Db { get; }
        private ICallableService Caller { get; }
        public PicViewModel(ICallableService callableService,ILogger logger,ISqlSugarClient sqlSugarClient)
        {
            Caller = callableService;
            Logger = logger;
            Db = sqlSugarClient;
        }

        public void Init(PicViewArg arg)
        {
            Affiliation = arg.Affiliation;
            Images.Clear();
            Episodes.Clear();
            Caller.PicturesLoadStarting(this, arg);
        }
        
        
        partial void OnCurrentEpisodeIndexChanged(int oldValue, int newValue)
        {
            Caller.CurrentEpisodeIndexChanged(this,Affiliation,oldValue,newValue);
        }
        
    }
}
