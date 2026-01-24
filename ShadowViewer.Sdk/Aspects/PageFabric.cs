using Metalama.Framework.Aspects;
using Metalama.Framework.Fabrics;
using Metalama.Framework.Code;
using Microsoft.UI.Xaml.Controls;

namespace ShadowViewer.Sdk.Aspects
{
    internal class PageFabric : TransitiveProjectFabric
    {
        public override void AmendProject(IProjectAmender amender)
        {
            amender.SelectMany(p => p.Types).Where(t =>!t.IsStatic)
                .Where(t => t.DerivesFrom(typeof(Page)))
                .AddAspectIfEligible<TriggerEventAttribute>();
            
        }
    }
}
