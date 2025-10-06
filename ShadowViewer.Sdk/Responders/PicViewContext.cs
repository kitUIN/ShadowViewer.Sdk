using System.Collections.Generic;

namespace ShadowViewer.Sdk.Responders;

/// <summary>
/// 
/// </summary>
/// <param name="Affiliation"></param>
/// <param name="Parameter"></param>
/// <param name="States"></param>
public record PicViewContext(string Affiliation, object Parameter, Dictionary<string, object> States);