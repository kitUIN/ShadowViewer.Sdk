using System;
using System.Collections.Generic;
using System.Linq;
using DryIoc;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Windows.ApplicationModel.Resources;
using Serilog;
using ShadowViewer.Enums;
using ShadowViewer.Extensions;
using ShadowViewer.Interfaces;
using ShadowViewer.Models;
using ShadowViewer.Plugin.Local.Enums;
using ShadowViewer.Plugin.Local.Models;
using ShadowViewer.Plugin.Local.Pages;
using ShadowViewer.Plugin.Local.ViewModels;
using ShadowViewer.Plugins;
using ShadowViewer.Services;
using ShadowViewer.Services.Interfaces;
using ShadowViewer.ViewModels;
using SqlSugar;

namespace ShadowViewer.Plugin.Local;

[AutoPluginMeta]
public partial class LocalPlugin : PluginBase
{
    public LocalPlugin(ICallableService callableService, ISqlSugarClient sqlSugarClient, CompressService compressServices, IPluginService pluginService, ILogger logger) : base(callableService, sqlSugarClient, compressServices, pluginService, logger)
    {
        DiFactory.Services.Register<AttributesViewModel>(Reuse.Transient);
        DiFactory.Services.Register<PicViewModel>(Reuse.Transient);
        DiFactory.Services.Register<BookShelfViewModel>(Reuse.Transient);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override LocalTag AffiliationTag { get; } = new LocalTag("Local", "#000000", "#ffd657");

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override Type? SettingsPage => typeof(BookShelfSettingsPage);

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override bool CanSwitch => false;

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override bool CanDelete => false;

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override bool CanOpenFolder => false;

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override void PluginEnabled()
    {
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override void PluginDisabled()
    {
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override IEnumerable<IShadowSearchItem> SearchTextChanged(AutoSuggestBox sender,
        AutoSuggestBoxTextChangedEventArgs args)
    {
        var res = new List<IShadowSearchItem>();
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput && !string.IsNullOrEmpty(sender.Text))
            res.AddRange(Db.Queryable<LocalComic>().Where(x => x.Name.Contains(sender.Text)).ToList().Select(item =>
                new LocalSearchItem(item.Name, MetaData!.Id, item.Id, LocalSearchMode.SearchComic)));
        return res;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override void SearchSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        if (args.SelectedItem is LocalSearchItem item) sender.Text = item.Title;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override void SearchQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
    }

    public override void PluginDeleting()
    {
        
    }
}