using System.Collections.Generic;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ShadowPluginLoader.Attributes;
using ShadowViewer.Sdk.Models.Interfaces;

namespace ShadowViewer.Sdk.Responders;

/// <summary>
/// <inheritdoc />
/// </summary>
public partial class AbstractSearchSuggestionResponder : ISearchSuggestionResponder
{
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    [Autowired] public string Id { get; }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public virtual IEnumerable<IShadowSearchItem> SearchTextChanged(AutoSuggestBox sender,
        AutoSuggestBoxTextChangedEventArgs args)
    {
        return [];
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public virtual void SearchSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public virtual void SearchQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public virtual void SearchGotFocus(object sender, RoutedEventArgs args)
    {

    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public virtual void SearchLostFocus(object sender, RoutedEventArgs args)
    {

    }
}