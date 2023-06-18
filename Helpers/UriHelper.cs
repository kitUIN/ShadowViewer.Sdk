namespace ShadowViewer.Helpers
{
    public static class UriHelper
    {
        /// <summary>
        /// 从浏览器打开
        /// </summary>
        public static async void LaunchUriAsync(this Uri uri)
        {
            if (uri != null)
            {
                await Launcher.LaunchUriAsync(uri);
            }
        }
        /// <summary>
        /// 从资源管理器打开
        /// </summary>
        public static async void LaunchFolderAsync(this StorageFolder folder)
        {
            if (folder != null)
            {
                await Launcher.LaunchFolderAsync(folder);
            }
        }
        /// <summary>
        /// Join
        /// </summary> 
        public static string JoinToString(this ObservableCollection<string> tags,string separator = ",")
        {
            return string.Join(separator, tags);
        }
        /// <summary>
        /// 获取文件
        /// </summary>
        public static async Task<StorageFile> GetFile(this Uri uri)
        {
            return await StorageFile.GetFileFromPathAsync(uri.DecodePath());
        }
        public static string DecodePath(this StorageFile file)
        {
            return HttpUtility.UrlDecode(file.Path);
        }
        public static string DecodePath(this Uri uri)
        {
            return HttpUtility.UrlDecode(uri.AbsolutePath);
        }
        public static string DecodeUri(this Uri uri)
        {
            return HttpUtility.UrlDecode(uri.AbsoluteUri);
        }
    }
}
