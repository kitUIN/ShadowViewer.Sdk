using System;
using System.Collections.Generic;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ShadowViewer.Core.Models;
using ShadowViewer.Core.Models.Interfaces;
using ShadowViewer.Core.Utils;

namespace ShadowViewer.Core.Responders;

/// <summary>
/// 搜索触发器基类
/// </summary>
public interface ISearchSuggestionResponder : IResponder
{
    /// <summary>
    /// 搜索文本变化
    /// </summary>
    IEnumerable<IShadowSearchItem> SearchTextChanged(AutoSuggestBox sender,
        AutoSuggestBoxTextChangedEventArgs args);

    /// <summary>
    /// 选中
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    void SearchSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args);

    /// <summary>
    /// 提交
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    void SearchQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args);

    /// <summary>
    /// 获取焦点
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    void SearchGotFocus(object sender, RoutedEventArgs args);

    /// <summary>
    /// 失去焦点
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    void SearchLostFocus(object sender, RoutedEventArgs args);
}