using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowViewer.Utils.Args
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
