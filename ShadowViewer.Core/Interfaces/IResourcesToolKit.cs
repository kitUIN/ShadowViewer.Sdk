namespace ShadowViewer.Interfaces
{
    public interface IResourcesToolKit
    {
        /// <summary>
        /// 返回.resw文件中的本地化字符串
        /// </summary>
        public string GetString(string key);
    }
}
