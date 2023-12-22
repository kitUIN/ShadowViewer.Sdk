using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml.Controls;
using ShadowViewer.Interfaces;
using ShadowViewer.Models;
using ShadowViewer.Plugin.Local.Enums;
using ShadowViewer.Plugin.Local.Helpers;
using ShadowViewer.Plugin.Local.Models;
using ShadowViewer.Plugin.Local.Pages;
using ShadowViewer.Responders;
using ShadowViewer.Services;
using ShadowViewer.Services.Interfaces;
using SqlSugar;

namespace ShadowViewer.Plugin.Local.Responders;

public class LocalNavigationResponder : NavigationResponderBase
{
    public override IEnumerable<IShadowNavigationItem> NavigationViewMenuItems { get; } =
        new List<IShadowNavigationItem>
        {
            new LocalNavigationItem
            {
                Icon = new SymbolIcon(Symbol.Home),
                Id = "BookShelf",
                Content = LocalResourcesHelper.GetString(LocalResourceKey.BookShelf)
            }
        };

    public override IEnumerable<IShadowNavigationItem> NavigationViewFooterItems { get; } =
        new List<IShadowNavigationItem>
        {
            new LocalNavigationItem
            {
                Icon = new FontIcon { Glyph = "\uE74C" },
                Id = "PluginManager",
                Content = LocalResourcesHelper.GetString(LocalResourceKey.PluginManager)
            }
        };

    public override void NavigationViewItemInvokedHandler(IShadowNavigationItem item, ref Type? page,
        ref object? parameter)
    {
        switch (item.Id)
        {
            case "BookShelf":
                page = typeof(BookShelfPage);
                parameter = new Uri("shadow://local/");
                break;
            case "PluginManager":
                page = typeof(PluginPage);
                break;
        }
    }

    public override void Navigate(Uri uri, string[] urls)
    {
        for (var i = 0; i < urls.Length; i++)
            if (!Db.Queryable<LocalComic>().Any(x => x.Id == urls[i] && x.IsFolder))
            {
                if (!Db.Queryable<LocalComic>().Any(x => x.Id == urls[i] && !x.IsFolder))
                {
                    Caller.NavigateTo(typeof(AttributesPage),  urls[i]);
                    return;
                }
                var s = "shadow://local/" + string.Join("/", urls.Take(i + 1));
                Caller.NavigateTo(typeof(BookShelfPage), new Uri(s));
                return;
            }
        Caller.NavigateTo(typeof(BookShelfPage), new Uri("shadow://local/"));
    }

    public LocalNavigationResponder(ICallableService callableService, ISqlSugarClient sqlSugarClient,
        CompressService compressServices, IPluginService pluginService, string id) : base(callableService,
        sqlSugarClient, compressServices, pluginService, id)
    {
    }
}