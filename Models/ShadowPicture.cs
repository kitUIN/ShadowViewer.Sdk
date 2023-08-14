using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowViewer.Models
{
    public partial class ShadowPicture : ObservableObject
    {
        [ObservableProperty]
        private int index;
        [ObservableProperty]
        private bool isInViewport;
        [ObservableProperty]
        private BitmapImage image;

        public ShadowPicture() { }
        public ShadowPicture(int index, BitmapImage image)
        {
            Index = index;
            Image = image;
        }
        public ShadowPicture(int index, Uri uri): this(index, new BitmapImage() { UriSource = uri }) { }
        public ShadowPicture(int index, string uri): this(index, new BitmapImage() { UriSource = new Uri(uri) }) { }

    }
}
