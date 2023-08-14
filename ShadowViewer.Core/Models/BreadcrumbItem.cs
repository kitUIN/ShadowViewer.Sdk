using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowViewer.Models
{
    public class BreadcrumbItem
    {
        public string Title { get; set; }
        public Type Type { get; set; }
        public BreadcrumbItem(string title, Type type)
        {
            Title = title;
            Type = type;
        }
    }
}
