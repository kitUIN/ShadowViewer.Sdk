namespace ShadowViewer.Helpers
{
    public static class FileHelper
    {
        public static string[] pngs = { ".png", ".jpg", ".jpeg", ".bmp" };
        public static string[] zips = { ".zip", ".rar", ".7z" , ".tar"}; 
        public static bool IsPic(this StorageFile file)
        {
            return pngs.Contains(file.FileType);
        }
        public static bool IsPic(string file)
        {
            return pngs.Any(x => file.EndsWith(x));
        }
        /// <summary>
        /// 是否是压缩文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool IsZip(this StorageFile file)
        {
            return zips.Contains(file.FileType);
        }
        /// <summary>
        /// 获取StorageFolder,若没有则创建文件夹
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static async Task<StorageFolder> ToStorageFolder(this string path)
        {
            path.CreateDirectory();
            return await StorageFolder.GetFolderFromPathAsync(path);
        }
        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="path"></param>
        public static void CreateDirectory(this string path)
        {
            string[] substrings = path.Split(new char[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
            string[] result = new string[substrings.Length];
            for (int i = 0; i < substrings.Length; i++)
            {
                result[i] = string.Join("/", substrings.Take(i + 1));
            }
            for (int i = 0; i < result.Length; i++)
            {
                if (!Directory.Exists(result[i]))
                {
                    Directory.CreateDirectory(result[i]);
                    Log.Information("文件夹{Dir}不存在,新建", result[i]);
                }
            } 
        }
        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="targetDir"></param>
        public static void DeleteDirectory(this string targetDir)
        {
            if (!Directory.Exists(targetDir))
            {
                return;
            }
            File.SetAttributes(targetDir, System.IO.FileAttributes.Normal);
            foreach (string file in Directory.GetFiles(targetDir))
            {
                File.SetAttributes(file, System.IO.FileAttributes.Normal);
                File.Delete(file);
            }
            foreach (string subDir in Directory.GetDirectories(targetDir))
            {
                DeleteDirectory(subDir);
            }
            Directory.Delete(targetDir, false);
            Log.Information("删除文件夹{Dir}", targetDir);
        }
        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="path"></param>
        public static void CreateFile(this string path)
        {
            string[] substrings = path.Split(new char[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
            string[] result = new string[substrings.Length];
            for (int i = 0; i < substrings.Length; i++)
            {
                result[i] = string.Join("/", substrings.Take(i + 1));
            }
            for (int i = 0; i < result.Length - 1; i++)
            {
                if (!Directory.Exists(result[i]))
                {
                    Directory.CreateDirectory(result[i]);
                    Log.Information("文件夹{Dir}不存在,新建", result[i]);
                }
            }
            if (!File.Exists(path))
            {
                File.Create(path);
                Log.Information("文件{Dir}不存在,新建", path);
            }
        }
        public static async Task<StorageFile> ToStorageFile(this string path)
        {
            path.CreateFile();
            return await StorageFile.GetFileFromPathAsync(path);
        } 
        /// <summary>
        /// 选择文件夹
        /// </summary>
        /// <param name="element"></param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static async Task<StorageFolder> SelectFolderAsync(UIElement element, string accessToken = "")
        {
            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(WindowHelper.GetWindowForElement(element));
            FolderPicker openPicker = new FolderPicker(); 
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);
            openPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            openPicker.FileTypeFilter.Add("*");
            StorageFolder folder = await openPicker.PickSingleFolderAsync();
            if (folder != null)
            {
                if(accessToken != "")
                {
                    StorageApplicationPermissions.FutureAccessList.AddOrReplace(accessToken, folder);
                }
                Log.ForContext<FolderPicker>().Information("选择了文件夹:{Path}", folder.Path);
                return folder;
            }
            return null;
        }
        /// <summary>
        /// 选择文件
        /// </summary>
        /// <param name="element"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static async Task<StorageFile> SelectFileAsync(UIElement element, params string[] filter)
        {
            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(WindowHelper.GetWindowForElement(element));
            FileOpenPicker openPicker = new FileOpenPicker();
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);
            openPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            foreach ( string filterItem in filter) { openPicker.FileTypeFilter.Add(filterItem); }
            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                Log.ForContext<FileOpenPicker>().Information("选择了文件:{Path}", file.Path);
                return file;
            }
            return null;
        }
    }
}
