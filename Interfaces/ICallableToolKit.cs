namespace ShadowViewer.Interfaces
{
    /// <summary>
    /// 通知其他控件的工具类
    /// </summary>
    public interface ICallableToolKit
    {
        /// <summary>
        /// 控制页面跳转事件
        /// </summary>
        public event EventHandler<NavigateToEventArgs> NavigateToEvent;
        /// <summary>
        /// 刷新书架事件
        /// </summary>
        public event EventHandler RefreshBookEvent;
        /// <summary>
        /// 控制页面跳转
        /// </summary>
        void NavigateTo(NavigateMode mode,Type page, string id, Uri url);
        /// <summary>
        /// 刷新书架
        /// </summary>
        void RefreshBook();
    }
}
