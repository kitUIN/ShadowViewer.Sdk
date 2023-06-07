using Microsoft.UI.Xaml.Controls;
using static System.Net.Mime.MediaTypeNames;

namespace ShadowViewer.Helpers
{
    public static class XamlHelper
    {
        public static FontIcon CreateFontIcon(string glyph)
        {
            return new FontIcon()
            {
                Glyph = glyph,
            };
        }
        public static ImageIcon CreateImageIcon(Uri uri)
        {
            return new ImageIcon()
            {
                Source = new BitmapImage(uri)
            };
        }
        public static ImageIcon CreateImageIcon(string uriString)
        {
            return CreateImageIcon(new Uri(uriString));
        }
        public static BitmapIcon CreateBitmapIcon(Uri uri)
        {
            return new BitmapIcon()
            {
                UriSource = uri,
            };
        }
        public static BitmapIcon CreateBitmapIcon(string uriString)
        {
            return CreateBitmapIcon(new Uri(uriString));
        }
        
        /// <summary>
        /// 创建一个横着的带Header的TextBox
        /// </summary>
        /// <param name="header">The header.</param>
        /// <param name="placeholder">The placeholder.</param>
        /// <param name="width">The width.</param>
        /// <returns></returns>
        public static StackPanel CreateOneLineTextBox(string header, string placeholder,string text, int width)
        {
            StackPanel grid = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
            };
            TextBlock headerBlock = new TextBlock
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 15, 0),
                Text = header,
                FontSize = 16,
            };
            TextBox txt = new TextBox()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Width = width,
                Text = text,
                PlaceholderText = placeholder,
            };
            grid.Children.Add(headerBlock);
            grid.Children.Add(txt);
            return grid;
        }

        /// <summary>
        /// 创建一个基础的ContentDialog
        /// </summary>
        public static ContentDialog CreateContentDialog(XamlRoot xamlRoot)
        {
            ContentDialog dialog = new ContentDialog();
            dialog.XamlRoot = xamlRoot;
            dialog.Style = Microsoft.UI.Xaml.Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.DefaultButton = ContentDialogButton.Primary;
            return dialog;
        }
        public static ContentDialog CreateOneTextBoxDialog(XamlRoot xamlRoot,
            string title = "", string header = "", string placeholder = "", string text = "",
            Action<ContentDialog, ContentDialogButtonClickEventArgs, string> primaryAction = null,
            Action<ContentDialog, ContentDialogButtonClickEventArgs, string> closeAction = null)
        {
            TextBox textBox = new TextBox()
            {
                Header = header,
                Text = text,
                PlaceholderText = placeholder,
            };
            ContentDialog dialog = new ContentDialog()
            {
                DefaultButton = ContentDialogButton.Primary,
                Title = title,
                PrimaryButtonText = I18nHelper.GetString("Shadow.String.Confirm"),
                CloseButtonText = I18nHelper.GetString("Shadow.String.Canel"),
                XamlRoot = xamlRoot,
                IsPrimaryButtonEnabled = true,
                Content = new Border()
                {
                    Child = textBox,
                }
            };
            dialog.PrimaryButtonClick += (ContentDialog sender, ContentDialogButtonClickEventArgs args) =>
            {
                primaryAction?.Invoke(sender, args, textBox.Text);
            };
            dialog.CloseButtonClick += (ContentDialog sender, ContentDialogButtonClickEventArgs args) =>
            {
                closeAction?.Invoke(sender, args, textBox.Text);
            };
            // dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            return dialog;
        }
        /// <summary>
        /// 通知ContentDialog
        /// </summary>
        /// <param name="xamlRoot"></param>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ContentDialog CreateMessageDialog(XamlRoot xamlRoot,string title,string message)
        {
            ContentDialog dialog = CreateContentDialog(xamlRoot);
            dialog.Title = title;
            dialog.Content = message;
            dialog.IsPrimaryButtonEnabled = false;
            dialog.CloseButtonText = I18nHelper.GetString("Shadow.String.Canel");
            return dialog;
        }
        public static StackPanel CreateOneLineTextBlock(string title, string value)
        {
            StackPanel panel = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
            };
            TextBlock headerBlock = new TextBlock
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Width = 100,
                Text = title,
            };
            TextBlock text = new TextBlock
            {
                IsTextSelectionEnabled = true,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(10, 0, 0, 0),
                Text = value,
            };
            panel.Children.Add(headerBlock);
            panel.Children.Add(text);
            return panel;
        }
        /// <summary>
        /// 创建一个原始的对话框
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="xamlRoot">The xaml root.</param>
        /// <param name="oldName">The old name.</param>
        /// <returns></returns>
        public static ContentDialog CreateOneLineTextBoxDialog(string title, XamlRoot xamlRoot, string oldName="",string header="",string placeholder = "")
        {
            ContentDialog dialog = CreateContentDialog(xamlRoot);
            dialog.Title = title;
            dialog.PrimaryButtonText = I18nHelper.GetString("Shadow.String.Confirm");
            dialog.CloseButtonText = I18nHelper.GetString("Shadow.String.Canel");
            StackPanel grid = new StackPanel()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Orientation = Orientation.Vertical,
            };
            StackPanel nameBox = CreateOneLineTextBox( header, placeholder, oldName, 222);
            grid.Children.Add(nameBox);
            dialog.Content = grid;
            dialog.IsPrimaryButtonEnabled = true;
            return dialog;
        }
        
    }
}
