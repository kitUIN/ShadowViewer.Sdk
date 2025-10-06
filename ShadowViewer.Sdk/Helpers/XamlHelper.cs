using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using ShadowViewer.Sdk.I18n;

namespace ShadowViewer.Sdk.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public static class XamlHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="glyph"></param>
        /// <returns></returns>
        public static FontIcon CreateFontIcon(string glyph)
        {
            return new FontIcon()
            {
                Glyph = glyph,
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static ImageIcon CreateImageIcon(Uri uri)
        {
            return new ImageIcon()
            {
                Source = new BitmapImage(uri)
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uriString"></param>
        /// <returns></returns>
        public static ImageIcon CreateImageIcon(string uriString)
        {
            return CreateImageIcon(new Uri(uriString));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static BitmapIcon CreateBitmapIcon(Uri uri)
        {
            return new BitmapIcon()
            {
                UriSource = uri,
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uriString"></param>
        /// <returns></returns>
        public static BitmapIcon CreateBitmapIcon(string uriString)
        {
            return CreateBitmapIcon(new Uri(uriString));
        }

        /// <summary>
        /// 创建一个横着的带Header的TextBox
        /// </summary>
        /// <param name="header">The header.</param>
        /// <param name="placeholder">The placeholder.</param>
        /// <param name="text"></param>
        /// <param name="width">The width.</param>
        /// <returns></returns>
        public static StackPanel CreateOneLineTextBox(string header, string placeholder, string text, int width)
        {
            var grid = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
            };
            var headerBlock = new TextBlock
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 15, 0),
                Text = header,
                FontSize = 16,
            };
            var txt = new TextBox()
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
        public static ContentDialog CreateContentDialog()
        {
            var dialog = new ContentDialog
            {
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                DefaultButton = ContentDialogButton.Primary
            };
            return dialog;
        }
        /// <summary>
        /// 创建一个带TextBox的ContentDialog
        /// </summary>
        public static ContentDialog CreateOneTextBoxDialog(
            string title = "", string header = "", string placeholder = "", string text = "",
            Action<ContentDialog, ContentDialogButtonClickEventArgs, string>? primaryAction = null,
            Action<ContentDialog, ContentDialogButtonClickEventArgs, string>? closeAction = null)
        {
            var textBox = new TextBox()
            {
                Header = header,
                Text = text,
                PlaceholderText = placeholder,
            };
            var dialog = new ContentDialog()
            {
                Title = title,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                PrimaryButtonText = ResourcesHelper.GetString(ResourceKey.Confirm),
                CloseButtonText = ResourcesHelper.GetString(ResourceKey.Cancel),
                IsPrimaryButtonEnabled = true,
                DefaultButton = ContentDialogButton.Primary,
                Content = new Border()
                {
                    Child = textBox,
                }
            };
            dialog.PrimaryButtonClick += (sender, args) => primaryAction?.Invoke(sender, args, textBox.Text);
            dialog.CloseButtonClick += (sender, args) => closeAction?.Invoke(sender, args, textBox.Text);
            return dialog;
        }
        /// <summary>
        /// 通知ContentDialog
        /// </summary>
        /// <param name="xamlRoot"></param>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ContentDialog CreateMessageDialog(XamlRoot xamlRoot, string title, string message)
        {
            var dialog = CreateContentDialog();
            dialog.Title = title;
            dialog.Content = message;
            dialog.IsPrimaryButtonEnabled = false;
            dialog.CloseButtonText = ResourcesHelper.GetString(ResourceKey.Cancel);
            return dialog;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="value"></param>
        /// <returns></returns>
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
        /// <param name="header"></param>
        /// <param name="placeholder"></param>
        /// <returns></returns>
        public static ContentDialog CreateOneLineTextBoxDialog(string title,
            XamlRoot xamlRoot, string oldName = "", string header = "", string placeholder = "")
        {
            var dialog = CreateContentDialog();
            dialog.Title = title;
            dialog.PrimaryButtonText = I18N.Confirm;
            dialog.CloseButtonText = I18N.Cancel;
            var grid = new StackPanel()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Orientation = Orientation.Vertical,
            };
            var nameBox = CreateOneLineTextBox(header, placeholder, oldName, 222);
            grid.Children.Add(nameBox);
            dialog.Content = grid;
            dialog.IsPrimaryButtonEnabled = true;
            return dialog;
        }

    }
}
