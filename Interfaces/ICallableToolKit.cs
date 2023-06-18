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
        public event EventHandler<NavigateToEventArgs> NavigateTo;
        /// <summary>
        /// 控制页面跳转
        /// </summary>
        void NavigateToPage(NavigateMode mode,Type page, string id, Uri url);
    }
}
