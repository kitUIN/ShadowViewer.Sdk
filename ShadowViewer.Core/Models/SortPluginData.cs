using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowViewer.Models
{
    public class SortPluginData
    {
        public string[]? Requires { get; set; }
        public string? Id { get; set; }
        public Type? PluginType { get; set; }
    }
}
