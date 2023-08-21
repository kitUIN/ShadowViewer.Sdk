using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DryIoc;
using ShadowViewer.Interfaces;
using SqlSugar;

namespace ShadowViewer.Plugin.Local.Models;

public partial class LocalHistory: ObservableObject,IHistory
{
    [ObservableProperty][property:SugarColumn(IsPrimaryKey = true)] private string id;
    [ObservableProperty] private string? title;
    [ObservableProperty] private string? icon;
    [ObservableProperty] private DateTime time;
    [SugarColumn(IsIgnore = true)]
    public string Plugin => "Local";
    
}