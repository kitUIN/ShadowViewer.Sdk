namespace ShadowViewer.ToolKits
{
    public class CallableToolKit : ICallableToolKit
    {
        public static ILogger Logger { get; } = Log.ForContext<CallableToolKit>();
        public event EventHandler<NavigateToEventArgs> NavigateTo;

        public void NavigateToPage(NavigateMode mode,Type page, string id, Uri url)
        {
            NavigateToEventArgs args = new NavigateToEventArgs(mode, page, id, url);
            if(NavigateTo is null)
            {
                Logger.Debug("事件NavigateTo不存在");
                return;
            }
            NavigateTo?.Invoke(this, args);
            Logger.Debug("触发事件NavigateTo{a}", args.ToString());
        }

    }
}
