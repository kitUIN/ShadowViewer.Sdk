using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowViewer.Utils.Args
{
    /// <summary>
    /// 导入漫画发生错误事件参数
    /// </summary>
    public class ImportComicErrorEventArgs
    {
        public ImportComicError Error { get; }
        public int Index { get; set; }
        public string Message { get;}
        public IReadOnlyList<IStorageItem> Items { get;}
        public string[] Password { get; set; }
        public ImportComicErrorEventArgs(ImportComicError error, string message, IReadOnlyList<IStorageItem> items, int index, string[] password)
        {
            Error = error;
            Message = message;
            Items = items;
            Index = index;
            Password = password;
        }
    }
}
