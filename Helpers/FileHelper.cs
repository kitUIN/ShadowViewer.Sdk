namespace ShadowViewer.Helpers
{
    public class FileHelper
    {
        public static string[] pngs = { ".png", ".jpg", ".jpeg", ".bmp" };
        public static string[] zips = { ".zip", ".rar", ".7z" , ".tar"};
        
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
        public static async Task<StorageFile> SelectFileAsync(UIElement element, params string[] filter)
        {
            return await SelectFileAsync(WindowHelper.GetWindowForElement(element), PickerLocationId.Desktop, PickerViewMode.List, filter);
        }
        public static async Task<StorageFile> SelectPicFileAsync(XamlRoot xamlRoot)
        {
            return await SelectFileAsync(WindowHelper.GetWindowForXamlRoot(xamlRoot), PickerLocationId.PicturesLibrary, PickerViewMode.Thumbnail, ".png", ".jpg", ".jpeg", ".bmp",".gif");
        }
        public static async Task<StorageFile> SelectFileAsync(Window window, PickerLocationId id, PickerViewMode mode, params string[] filter)
        {
            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            FileOpenPicker openPicker = new FileOpenPicker();
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);
            openPicker.SuggestedStartLocation = id;
            openPicker.ViewMode = mode;
            foreach (string filterItem in filter) openPicker.FileTypeFilter.Add(filterItem);
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
