using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowViewer.Core.Args
{

    public class MainBackEventArgs
    {
        public bool Force { get; }
        public MainBackEventArgs(bool force)
        {
            Force = force;
        }
    }
}
