using Microsoft.UI.Xaml;
using ShadowViewer.Core.Enums;

namespace ShadowViewer.Core.Args;

public class TopGridEventArg
{
    public UIElement Element { get; set; }
    public TopGridMode Mode { get; set; }

    public TopGridEventArg(UIElement element, TopGridMode mode)
    {
        Element = element;
        Mode = mode;
    }
}