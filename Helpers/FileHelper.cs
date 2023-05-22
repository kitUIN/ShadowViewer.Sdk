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
        }
        public static async Task<StorageFile> ToStorageFile(this string path)
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
                }
            }
            if (!File.Exists(path))
            {
                File.Create(path);
            }
            return await StorageFile.GetFileFromPathAsync(path);
        } 
        public static async Task<StorageFolder> SelectFolderAsync(UIElement element, string accessToken = "")
        {
            var window = WindowHelper.GetWindowForElement(element);
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
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
                Log.ForContext<FolderPicker>().Debug("选择了文件夹:{Path}", folder.Path);
                return folder;
            }
            return null;
        }
        public static async Task<StorageFile> SelectFileAsync(UIElement element, params string[] filter)
        {
            var window = WindowHelper.GetWindowForElement(element);
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            FileOpenPicker openPicker = new FileOpenPicker();
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);
            openPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            foreach ( var filterItem in filter) { openPicker.FileTypeFilter.Add(filterItem); }
            var file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                Log.ForContext<FileOpenPicker>().Debug("选择了文件:{Path}", file.Path);
                return file;
            }
            return null;
        }
        /// <summary>
        /// 计算大小
        /// </summary>
        /// <param name="files">The files.</param>
        /// <returns></returns>
        public static async Task<ulong> GetSizeInFiles(IReadOnlyList<StorageFile> files)
        {
            ulong res = 0;
            foreach (var item in files.Where(x => pngs.Contains(x.FileType)))
            {
                res += (await item.GetBasicPropertiesAsync()).Size;
            }
            return res;
        }
        /// <summary>
        /// 从文件中获取封面
        /// </summary>
        /// <param name="files">The files.</param>
        /// <returns></returns>
        public static string GetImgInFiles(IReadOnlyList<StorageFile> files)
        {
            
            var imgFile = files.OrderBy(x => x.Name).FirstOrDefault(x => pngs.Contains(x.FileType));
            return imgFile is null ? "" : imgFile.Path;
        }
    }
}
