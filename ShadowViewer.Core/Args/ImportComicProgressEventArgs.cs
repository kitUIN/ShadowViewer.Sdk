namespace ShadowViewer.Core.Args
{
    /// <summary>
    /// 导入漫画进度条事件参数
    /// </summary>
    public class ImportComicProgressEventArgs
    {
        /// <summary>
        /// 进度
        /// </summary>
        public double Progress { get; }
        public ImportComicProgressEventArgs(double progress)
        {
            Progress = progress;
        }
    }
}
