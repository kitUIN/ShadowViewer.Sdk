using ShadowViewer.Utils.Args;

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
        /// 导入漫画完成事件
        /// </summary>
        public event EventHandler ImportComicCompletedEvent;
        /// <summary>
        /// 导入漫画事件
        /// </summary>
        public event EventHandler<ImportComicEventArgs> ImportComicEvent;
        /// <summary>
        /// 导入漫画[错误]事件
        /// </summary>
        public event EventHandler<ImportComicErrorEventArgs> ImportComicErrorEvent;
        /// <summary>
        /// 导入漫画[缩略图]事件
        /// </summary>
        public event EventHandler<ImportComicThumbEventArgs> ImportComicThumbEvent;
        /// <summary>
        /// 导入漫画[进度]事件
        /// </summary>
        public event EventHandler<ImportComicProgressEventArgs> ImportComicProgressEvent;
        /// <summary>
        /// 调试事件
        /// </summary>
        public event EventHandler DebugEvent;
        /// <summary>
        /// 设置页面后退
        /// </summary>
        public event EventHandler SettingsBackEvent;
        /// <summary>
        /// 设置主页面后退
        /// </summary>
        public event EventHandler<MainBackEventArgs> MainBackEvent;
        /// <summary>
        /// 控制页面跳转
        /// </summary>
        void NavigateTo(NavigateMode mode,Type page, string id, Uri url);
        /// <summary>
        /// 刷新书架
        /// </summary>
        void RefreshBook();
        /// <summary>
        /// 导入漫画
        /// </summary>
        void ImportComic(IReadOnlyList<IStorageItem> items, string[] passwords, int index);
        /// <summary>
        /// 导入漫画[错误]
        /// </summary>
        void ImportComicError(ImportComicError error,string message, IReadOnlyList<IStorageItem> items, int index, string[] password);
        /// <summary>
        /// 导入漫画[缩略图]
        /// </summary>
        void ImportComicThumb(MemoryStream stream);
        /// <summary>
        /// 导入漫画[进度]
        /// </summary>
        void ImportComicProgress(double progress);
        /// <summary>
        /// 导入漫画完成
        /// </summary>
        void ImportComicCompleted();
        /// <summary>
        /// 调试
        /// </summary>
        void Debug();
        /// <summary>
        /// 设置界面后退
        /// </summary>
        void SettingsBack();
        /// <summary>
        /// 设置主界面后退
        /// </summary>
        void MainBack(bool force);
    }
}
