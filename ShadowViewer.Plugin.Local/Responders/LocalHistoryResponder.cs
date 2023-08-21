using System;
using System.Collections.Generic;
using ShadowViewer.Enums;
using ShadowViewer.Interfaces;
using ShadowViewer.Plugin.Local.Models;
using ShadowViewer.Plugin.Local.Pages;
using ShadowViewer.Responders;
using ShadowViewer.Services;
using SqlSugar;

namespace ShadowViewer.Plugin.Local.Responders;

public class LocalHistoryResponder:HistoryResponderBase
{
     
    public override IEnumerable<IHistory> GetHistories(HistoryMode mode = HistoryMode.Day)
    {
        return mode switch
        {
            HistoryMode.Day => Db.Queryable<LocalHistory>()
                .Where(history => history.Time >= DateTime.Now - TimeSpan.FromDays(1))
                .ToList(),
            HistoryMode.Week => Db.Queryable<LocalHistory>()
                .Where(history => history.Time >= DateTime.Now - TimeSpan.FromDays(7))
                .ToList(),
            HistoryMode.Month => Db.Queryable<LocalHistory>()
                .Where(history => history.Time >= DateTime.Now - TimeSpan.FromDays(30))
                .ToList(),
            _ => Db.Queryable<LocalHistory>().ToList()
        };
    }

    public override void ClickHistoryHandler(IHistory history)
    {
        Caller.NavigateTo(typeof(AttributesPage),  history.Id);
    }

    public override void DeleteHistoryHandler(IHistory history)
    {
        Db.Deleteable(new LocalHistory { Id = history.Id }).ExecuteCommand();
    }

    public LocalHistoryResponder(ICallableService callableService, ISqlSugarClient sqlSugarClient, CompressService compressServices, PluginService pluginService, string id) : base(callableService, sqlSugarClient, compressServices, pluginService, id)
    {
    }
}