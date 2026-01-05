using System;
using System.Collections.Concurrent;
using ShadowViewer.Sdk.Utils;

namespace ShadowViewer.Sdk.Navigation;

/// <summary>
/// Registry for mapping navigation path segments to page <see cref="Type"/> instances.
/// The registry is a single-tree (host is assumed fixed); each segment is a node and the
/// final node may hold the Page Type to navigate to.
/// </summary>
public static class ShadowRouteRegistry
{
    // Rework registry to be a single-root tree. Each node can hold a PageType; children are indexed by segment.
    private sealed class Node
    {
        public readonly ConcurrentDictionary<string, Node> Children = new(StringComparer.OrdinalIgnoreCase);
        public ShadowNavigation? Navigation;
    }

    private static readonly Node Root = new();

    /// <summary>
    /// Register a page <paramref name="navigation"/> for the provided path segments.
    /// If no segments are provided, the page is registered at the root.
    /// </summary>
    /// <param name="navigation">The page type to associate with the path.</param>
    /// <param name="segments">An ordered list of path segments (each segment corresponds to a tree level).</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="navigation"/> is null.</exception>
    public static void RegisterPage(ShadowNavigation navigation, params string[] segments)
    {
        ArgumentNullException.ThrowIfNull(navigation);

        var node = Root;

        foreach (var raw in segments)
        {
            var part = raw;
            if (string.IsNullOrEmpty(part)) continue;
            var key = part.ToLowerInvariant();
            node = node.Children.GetOrAdd(key, _ => new Node());
        }

        // Set/override the page type at the final node
        node.Navigation = navigation;
    }

    /// <summary>
    /// Resolve the most specific registered page <see cref="Type"/> for the given <paramref name="uri"/>.
    /// The registry ignores host and matches against the URI's segments, returning the most specific
    /// matching node's Navigation (or null if none found).
    /// </summary>
    /// <param name="uri">The ShadowUri to resolve. If null, returns null.</param>
    /// <returns>The matched Page Type, or null if no match exists.</returns>
    public static ShadowNavigation? ResolvePage(ShadowUri? uri)
    {
        if (uri is not { Scheme: "shadow" }) return null;

        Root.Children.TryGetValue(uri.Host.ToLowerInvariant(), out var node);

        if (node == null) return null;

        var best = node.Navigation;

        foreach (var seg in uri.Segments)
        {
            var key = seg.ToLowerInvariant();
            if (!node.Children.TryGetValue(key, out node)) break;
            if (node.Navigation != null) best = node.Navigation;
        }

        return best;
    }
}