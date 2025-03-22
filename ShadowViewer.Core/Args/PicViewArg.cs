namespace ShadowViewer.Core.Args
{
    /// <summary>
    /// 
    /// </summary>
    public class PicViewArg
    {
        /// <summary>
        /// 
        /// </summary>
        public string Affiliation { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object Parameter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="affiliation"></param>
        /// <param name="parameter"></param>
        public PicViewArg(string affiliation, object parameter)
        {
            Affiliation = affiliation;
            Parameter = parameter;
        }
    }
}