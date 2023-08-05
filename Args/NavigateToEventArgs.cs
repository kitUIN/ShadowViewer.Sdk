using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace ShadowViewer.Args
{
    /// <summary>
    /// 跳转页面事件
    /// </summary>
    public class NavigateToEventArgs : EventArgs
    {
        /// <summary>
        /// 跳转格式
        /// </summary>
        public NavigateMode Mode { get; }
        public string Id { get; }
        public Uri Url { get; }
        public Type Page { get; }
        public NavigateToEventArgs(NavigateMode mode, Type page, string id, Uri url)
        {
            Mode = mode;
            if (Mode == NavigateMode.URL)
            {
                Id = id;
                Url = url;
            }
            else
            {
                Page = page;
                Url = url;
            }
        }
        public new string ToString()
        {
            return $"[NavigateToEventArgs,Mode={Mode},Page={Page},Id={Id},Url={Url}]";
        }
    }
}
