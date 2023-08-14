using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowViewer.Args
{
    public class CurrentPageChangedEventArgs
    {
        public CurrentPageChangedMode Mode { get; }
        public int NewValue { get; }
        public int OldValue { get; }
        public CurrentPageChangedEventArgs(CurrentPageChangedMode mode, int newValue, int oldValue)
        {
            Mode = mode;
            NewValue = newValue;
            OldValue = oldValue;
        }
    }
}
