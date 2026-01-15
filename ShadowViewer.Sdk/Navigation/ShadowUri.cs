using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ShadowViewer.Sdk.Navigation;

/*
 Format: scheme://host/seg1/seg2?k=v (query pairs)
*/
/// <summary>
/// 表示一个解析后的 Shadow URI，支持方案、主机、路径段、查询参数与片段（fragment）。
/// </summary>
public class ShadowUri
{
    /// <summary>
    /// URI 的方案部分（例如 "http", "shadow" 等）。
    /// </summary>
    public string Scheme { get; }

    /// <summary>
    /// URI 的主机部分（host）。
    /// </summary>
    public string Host { get; }

    /// <summary>
    /// 路径段数组，已经对每个段进行了 URL 解码，不包含前导或后缀的斜线。
    /// 如果原始路径为空或为根路径，数组长度为 0。
    /// </summary>
    public string[] Segments { get; }

    /// <summary>
    /// 查询参数的只读字典，键为参数名（不区分大小写），值为该参数的多个值数组（按出现顺序）。
    /// </summary>
    public IReadOnlyDictionary<string, string[]> Query { get; }

    /// <summary>
    /// URI 的片段（fragment），已解码；如果没有片段则为 <c>null</c>。
    /// </summary>
    public string? Fragment { get; }
    

    /// <summary>
    /// 创建一个 <see cref="ShadowUri"/> 的内部构造函数。
    /// </summary>
    /// <param name="scheme">方案字符串。</param>
    /// <param name="host">主机字符串。</param>
    /// <param name="segments">路径段数组（已解码）。</param>
    /// <param name="query">查询参数映射（键 -> 值数组）。</param>
    /// <param name="fragment">片段字符串（已解码），或 <c>null</c>。</param>
    private ShadowUri(string scheme, string host, string[] segments, Dictionary<string, string[]> query, string? fragment)
    {
        Scheme = scheme;
        Host = host;
        Segments = segments;
        Query = query;
        Fragment = fragment;
    }

    /// <summary>
    /// 从字符串解析并返回 <see cref="ShadowUri"/> 实例。
    /// </summary>
    /// <param name="uriString">要解析的 URI 字符串。</param>
    /// <returns>解析后的 <see cref="ShadowUri"/> 对象。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="uriString"/> 为 <c>null</c>、空或仅空白时抛出。</exception>
    /// <exception cref="UriFormatException">当 <paramref name="uriString"/> 不是有效的 URI 时由 <see cref="Uri"/> 的构造函数抛出。</exception>
    public static ShadowUri Parse(string uriString)
    {
        if (string.IsNullOrWhiteSpace(uriString))
            throw new ArgumentNullException(nameof(uriString));
        var uri = new Uri(uriString);
        return Parse(uri);
    }

    /// <summary>
    /// 从已有的 <see cref="Uri"/> 实例解析并返回 <see cref="ShadowUri"/> 对象。
    /// </summary>
    /// <param name="uri">要解析的 <see cref="Uri"/> 实例，不能为空。</param>
    /// <returns>解析后的 <see cref="ShadowUri"/> 对象。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="uri"/> 为 <c>null</c> 时抛出。</exception>
    public static ShadowUri Parse(Uri uri)
    {
        ArgumentNullException.ThrowIfNull(uri);

        var scheme = uri.Scheme;
        var host = uri.Host;

        // Path segments: Uri.Segments contains parts with leading '/'. We'll trim '/'
        var rawSegments = uri.AbsolutePath.Split(['/'], StringSplitOptions.RemoveEmptyEntries);
        var segments = rawSegments.Select(Uri.UnescapeDataString).ToArray();

        // Query parsing
        var queryMap = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);
        var query = uri.Query;
        if (!string.IsNullOrEmpty(query))
        {
            var q = query.TrimStart('?');
            var parts = q.Split(['&'], StringSplitOptions.RemoveEmptyEntries);
            foreach (var part in parts)
            {
                var idx = part.IndexOf('=');
                string key, value;
                if (idx >= 0)
                {
                    key = WebUtility.UrlDecode(part[..idx]);
                    value = WebUtility.UrlDecode(part[(idx + 1)..]);
                }
                else
                {
                    continue;
                }

                if (!queryMap.TryGetValue(key, out var arr))
                {
                    queryMap[key] = [value];
                }
                else
                {
                    var list = arr.ToList();
                    list.Add(value);
                    queryMap[key] = list.ToArray();
                }
            }
        }

        var fragment = string.IsNullOrEmpty(uri.Fragment) ? null : Uri.UnescapeDataString(uri.Fragment.TrimStart('#'));

        return new ShadowUri(scheme, host, segments, queryMap, fragment);
    }

    /// <summary>
    /// 将当前 <see cref="ShadowUri"/> 实例重新格式化为 URI 字符串。结果字符串对路径段、查询键值和片段进行 URL 编码。
    /// </summary>
    /// <returns>表示此对象的 URI 字符串。</returns>
    public string ToUriString()
    {
        var sb = new System.Text.StringBuilder();
        sb.Append(Scheme);
        sb.Append("://");
        sb.Append(Host);

        if (Segments.Length > 0)
        {
            foreach (var seg in Segments)
            {
                sb.Append('/');
                sb.Append(Uri.EscapeDataString(seg));
            }
        }
        else
        {
            sb.Append('/');
        }

        if (Query.Count > 0)
        {
            var first = true;
            foreach (var kv in Query)
            {
                foreach (var v in kv.Value)
                {
                    sb.Append(first ? '?' : '&');
                    first = false;
                    sb.Append(Uri.EscapeDataString(kv.Key));
                    sb.Append('=');
                    sb.Append(Uri.EscapeDataString(v));
                }
            }
        }

        if (string.IsNullOrEmpty(Fragment)) return sb.ToString();
        sb.Append('#');
        sb.Append(Uri.EscapeDataString(Fragment));

        return sb.ToString();
    }

    /// <summary>
    /// 获取指定键的第一个查询值。
    /// </summary>
    /// <param name="key">查询参数的键。</param>
    /// <returns>对应的第一个值，如果不存在则为 <c>null</c>。</returns>
    public string? GetQueryFirst(string key)
    {
        if (string.IsNullOrEmpty(key)) return null;
        if (Query.TryGetValue(key, out var arr) && arr.Length > 0)
            return arr[0];
        return null;
    }

    /// <summary>
    /// 返回当前 <see cref="ShadowUri"/> 实例的字符串表示形式，即 URI 字符串。
    /// </summary>
    /// <returns>表示此对象的 URI 字符串。</returns>
    public override string ToString() => ToUriString();
}