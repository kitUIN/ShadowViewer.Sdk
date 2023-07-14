using Microsoft.UI.Xaml.Controls;
using ShadowViewer.Utils.Args;
using System.IO;
using System.Security.Policy;

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
        public void ImportComic(IReadOnlyList<IStorageItem> items, string[] passwords, int index)
        {
            ImportComicEventArgs args = new ImportComicEventArgs(items, passwords, index);
            if (ImportComicEvent is null)
            {
                Logger.Debug("事件ImportComicEvent不存在");
                return;
            }
            ImportComicEvent?.Invoke(this, args);
            Logger.Debug("触发事件ImportComicEvent");
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void ImportComicThumb(MemoryStream stream)
        {
            ImportComicThumbEventArgs args = new ImportComicThumbEventArgs(stream);
            if (ImportComicThumbEvent is null)
            {
                Logger.Debug("事件ImportComicThumbEvent不存在");
                return;
            }
            ImportComicThumbEvent?.Invoke(this, args);
            Logger.Debug("触发事件ImportComicThumbEvent");
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void ImportComicError(ImportComicError error, string message, IReadOnlyList<IStorageItem> items, int index, string[] password)
        {
            ImportComicErrorEventArgs args = new ImportComicErrorEventArgs(error,message,items,index, password);
            if (ImportComicErrorEvent is null)
            {
                Logger.Debug("事件ImportComicErrorEvent不存在");
                return;
            }
            ImportComicErrorEvent?.Invoke(this, args);
            Logger.Debug("触发事件ImportComicErrorEvent");
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void ImportComicProgress(double progress)
        {
            ImportComicProgressEventArgs args = new ImportComicProgressEventArgs(progress);
            if (ImportComicProgressEvent is null)
            {
                Logger.Debug("事件ImportComicProgressEvent不存在");
                return;
            }
            ImportComicProgressEvent?.Invoke(this, args);
            Logger.Debug("触发事件ImportComicProgressEvent");
        }

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
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void ImportComicCompleted()
        {
            if (ImportComicCompletedEvent is null)
            {
                Logger.Debug("事件ImportComicCompletedEvent不存在");
                return;
            }
            ImportComicCompletedEvent?.Invoke(this, null);
            Logger.Debug("触发事件ImportComicCompletedEvent");
        }
    }
}
