namespace ShadowViewer.Args
{
    public class PicViewArg
    {
        public string Affiliation { get; set; }
        public object Parameter { get; set; }
        public PicViewArg(string affiliation,object parameter)
        { 
            Affiliation = affiliation;
            Parameter = parameter;
        
        }
    }
}
