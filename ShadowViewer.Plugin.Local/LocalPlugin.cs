using System;
using System.Collections.Generic;
using System.Linq;
using DryIoc;
using Microsoft.UI.Xaml.Controls;
using ShadowViewer.Extensions;
using ShadowViewer.Interfaces;
using ShadowViewer.Models;
using ShadowViewer.Plugin.Local.Enums;
using ShadowViewer.Plugin.Local.Models;
using ShadowViewer.Plugin.Local.Pages;
using ShadowViewer.Plugin.Local.ViewModels;
using ShadowViewer.Plugins;
using ShadowViewer.Services;
using SqlSugar;

namespace ShadowViewer.Plugin.Local;

[PluginMetaData("Local",
    "阅读器应用核心",
    "阅读器核心功能插件",
    "kitUIN", "0.1.0",
    "https://github.com/kitUIN/ShadowViewer/",
    "fluent://\uEA4E",
    20230808,
    null,
    new []{"zh-CN"})]
public class LocalPlugin : PluginBase
{
    public LocalPlugin(ICallableService callableService, ISqlSugarClient sqlSugarClient,
        CompressService compressServices, PluginService pluginService) : base(callableService, sqlSugarClient,
        compressServices, pluginService)
    {
        Db.CodeFirst.InitTables<LocalHistory>();
        DiFactory.Services.Register<AttributesViewModel>(Reuse.Transient);
        DiFactory.Services.Register<PicViewModel>(Reuse.Transient);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override PluginMetaData MetaData { get; } = typeof(LocalPlugin).GetPluginMetaData();

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override LocalTag AffiliationTag { get; }

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
                new LocalSearchItem(item.Name, MetaData.Id, item.Id, LocalSearchMode.SearchComic)));
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