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
        public event EventHandler<ImportComicEventArgs> ImportComicEvent;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event EventHandler<ImportComicErrorEventArgs> ImportComicErrorEvent;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event EventHandler<ImportComicThumbEventArgs> ImportComicThumbEvent;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event EventHandler<ImportComicProgressEventArgs> ImportComicProgressEvent;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event EventHandler ImportComicCompletedEvent;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event EventHandler DebugEvent;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event EventHandler<PluginEventArg> PluginEnabledEvent;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event EventHandler<PluginEventArg> PluginDisabledEvent;
        public event EventHandler<TopGridEventArg> TopGridEvent;
        public event EventHandler<ImportPluginEventArgs> ImportPluginEvent;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void ImportComic(IReadOnlyList<IStorageItem> items, string[] passwords, int index)
        {
            ImportComicEvent?.Invoke(this, new ImportComicEventArgs(items, passwords, index));
            Logger.Information("触发事件ImportComicEvent");
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void ImportComicThumb(MemoryStream stream)
        {
            ImportComicThumbEvent?.Invoke(this, new ImportComicThumbEventArgs(stream));
            Logger.Information("触发事件ImportComicThumbEvent");
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void ImportComicError(ImportComicError error, string message, IReadOnlyList<IStorageItem> items, int index, string[] password)
        {
            ImportComicErrorEvent?.Invoke(this, new ImportComicErrorEventArgs(error,message,items,index, password));
            Logger.Information("触发事件ImportComicErrorEvent");
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void ImportComicProgress(double progress)
        {
            ImportComicProgressEvent?.Invoke(this, new ImportComicProgressEventArgs(progress));
            Logger.Information("触发事件ImportComicProgressEvent");
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void NavigateTo(NavigateMode mode,Type page, string id, Uri url)
        {
            var args = new NavigateToEventArgs(mode, page, id, url);
            NavigateToEvent?.Invoke(this, args);
            Logger.Information("触发事件NavigateTo{A}", args.ToString());
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void RefreshBook()
        {
            RefreshBookEvent?.Invoke(this, EventArgs.Empty);
            Logger.Information("触发事件RefreshBook");
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void ImportComicCompleted()
        {
            ImportComicCompletedEvent?.Invoke(this, EventArgs.Empty);
            Logger.Information("触发事件ImportComicCompletedEvent");
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Debug()
        {
            DebugEvent?.Invoke(this, EventArgs.Empty);
            Logger.Information("触发事件DebugEvent");
        }

        public void PluginEnabled(object sender, string id, bool enabled)
        {
            Logger.Information("{Sender}触发事件{Event}", sender.GetType().FullName,
                nameof(PluginEnabledEvent));
            PluginEnabledEvent?.Invoke(sender, new PluginEventArg(id, enabled));
        }

        public void PluginDisabled(object sender, string id, bool enabled)
        {
            Logger.Information("{Sender}触发事件{Event}", sender.GetType().FullName,
                nameof(PluginDisabledEvent));
            PluginDisabledEvent?.Invoke(sender, new PluginEventArg(id, enabled));
        }

        public void TopGrid(object sender, UIElement element, TopGridMode mode)
        {
            Logger.Information("{Sender}触发事件{Event}", sender.GetType().FullName,
                nameof(TopGridEvent));
            TopGridEvent?.Invoke(sender, new TopGridEventArg(element, mode));
        }

        public void ImportPlugin(object sender, IReadOnlyList<IStorageItem> items)
        {
            ImportPluginEvent?.Invoke(sender, new ImportPluginEventArgs(items));
            Logger.Information("触发事件{Event}",nameof(ImportPluginEvent));
        }
    }
}
