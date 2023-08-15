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
        public object Parameter { get; }
        public Uri Url { get; }
        public Type Page { get; }
        public bool Force { get; }
        public NavigateToEventArgs(Type page, object parameter, bool force)
        {
            Page = page;
            Parameter = parameter;
            Force = force;
        }
        public new string ToString()
        {
            return $"[NavigateToEventArgs,Page={Page},Url={Url}]";
        }
    }
}
