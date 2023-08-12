using Microsoft.UI.Xaml.Data;

namespace ShadowViewer.Converters
{
    /// <summary>
    /// 文件大小转换器: long to string(B/KB/MB/GB)
    /// </summary>
    public class SizeToFormatConverter : IValueConverter
    {
        public static string SizeFormat(long size)
        {
            long KB = 1024;
            long MB = KB * 1024;
            long GB = MB * 1024;
            if (size / GB >= 1)
            {
                return $"{Math.Round(size / (float)GB, 2)} GB";
            }
            else if (size / MB >= 1)
            {
                return $"{Math.Round(size / (float)MB, 2)} MB";
            }
            else if (size / KB >= 1)
            {
                return $"{Math.Round(size / (float)KB, 2)} KB";
            }
            return $"{size} B";
        }
        public static long SizeToLong(string sizeString)
        {
            var sizes = sizeString.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            long b = 0;
            if (sizes.Length == 2) {
                switch (sizes[1].ToLower())
                {
                    case "b":
                        b = 1;
                        break;
                    case "kb":
                        b = 1024;
                        break;
                    case "mb":
                        b = 1024 * 1024;
                        break;
                    case "g":
                        b = 1024 * 1024 * 1024;
                        break;
                }

            }
            return (long)(double.Parse(sizes[0]) * b);
        }
        #region IValueConverter Members

        public object Convert(object value, Type targetType,
            object parameter, string language)
        {
            return SizeFormat((long)value);
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, string language)
        {
            return SizeToLong(value as string ?? "");
        }

        #endregion
    }
}
