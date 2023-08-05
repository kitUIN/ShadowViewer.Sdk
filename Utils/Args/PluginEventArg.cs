using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowViewer.Utils.Args
{
    public class PluginEventArg
    {
        public string PluginId { get; set; }
        public bool IsEnabled { get; set; }
        public PluginEventArg(string id,bool isEnabled )
        {
            PluginId = id;
            IsEnabled = isEnabled;


        }
    }
}
