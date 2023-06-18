namespace ShadowViewer.ToolKits
{
    public class CallableToolKit : ICallableToolKit
    {
        public static ILogger Logger { get; } = Log.ForContext<CallableToolKit>();
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event EventHandler<NavigateToEventArgs> NavigateToEvent;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event EventHandler RefreshBookEvent;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void NavigateTo(NavigateMode mode,Type page, string id, Uri url)
        {
            NavigateToEventArgs args = new NavigateToEventArgs(mode, page, id, url);
            if(NavigateToEvent is null)
            {
                Logger.Debug("事件NavigateTo不存在");
                return;
            }
            NavigateToEvent?.Invoke(this, args);
            Logger.Debug("触发事件NavigateTo{a}", args.ToString());
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void RefreshBook()
        {
            if (RefreshBookEvent is null)
            {
                Logger.Debug("事件RefreshBook不存在");
                return;
            }
            RefreshBookEvent?.Invoke(this, null);
            Logger.Debug("触发事件RefreshBook");
        }
    }
}
