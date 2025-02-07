using System;

namespace ShadowViewer.Core.Models
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
