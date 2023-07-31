using Microsoft.UI.Xaml.Markup;
using ShadowViewer.Extensions;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ShadowViewer.Controls
{
    [ContentProperty(Name = "Content")]
    public class ShadowSettingHeader : Control
    {
        public ShadowSettingHeader()
        {
            this.DefaultStyleKey = typeof(ShadowSettingHeader);
        }
        /// <summary>
        /// 获取或设置Content的值
        /// </summary>  
        public object Content
        {
            get { return (object)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        /// <summary>
        /// 标识 Content 依赖属性。
        /// </summary>
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(object), typeof(ShadowSettingHeader), new PropertyMetadata(null, OnContentChanged));

        private static void OnContentChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            ShadowSettingHeader target = obj as ShadowSettingHeader;
            object oldValue = (object)args.OldValue;
            object newValue = (object)args.NewValue;
            if (oldValue != newValue)
                target.OnContentChanged(oldValue, newValue);
        }

        protected virtual void OnContentChanged(object oldValue, object newValue)
        {
        }



        /// <summary>
        /// 获取或设置ContentTemplate的值
        /// </summary>  
        public DataTemplate ContentTemplate
        {
            get { return (DataTemplate)GetValue(ContentTemplateProperty); }
            set { SetValue(ContentTemplateProperty, value); }
        }

        /// <summary>
        /// 标识 ContentTemplate 依赖属性。
        /// </summary>
        public static readonly DependencyProperty ContentTemplateProperty =
            DependencyProperty.Register("ContentTemplate", typeof(DataTemplate), typeof(ShadowSettingHeader), new PropertyMetadata(null, OnContentTemplateChanged));

        private static void OnContentTemplateChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            ShadowSettingHeader target = obj as ShadowSettingHeader;
            DataTemplate oldValue = (DataTemplate)args.OldValue;
            DataTemplate newValue = (DataTemplate)args.NewValue;
            if (oldValue != newValue)
                target.OnContentTemplateChanged(oldValue, newValue);
        }

        protected virtual void OnContentTemplateChanged(DataTemplate oldValue, DataTemplate newValue)
        {

        }
        public static readonly DependencyProperty DescriptionProperty =
           DependencyProperty.Register(nameof(Description),
               typeof(string),
               typeof(ShadowSettingHeader),
               new PropertyMetadata("", new PropertyChangedCallback(OnDescriptionChanged)));
        public static readonly DependencyProperty HeaderProperty =
           DependencyProperty.Register(nameof(Header),
               typeof(string),
               typeof(ShadowSettingHeader),
               new PropertyMetadata("", new PropertyChangedCallback(OnHeaderChanged)));
        public static readonly DependencyProperty IsShowDescriptionProperty =
           DependencyProperty.Register(nameof(IsShowDescription),
               typeof(Visibility),
               typeof(ShadowSettingHeader),
               new PropertyMetadata(Visibility.Collapsed, new PropertyChangedCallback(OnIsShowDescriptionChanged)));
 
        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }
        public Visibility IsShowDescription
        {
            get { return (Visibility)GetValue(IsShowDescriptionProperty); }
            set { SetValue(IsShowDescriptionProperty, value); }
        }
        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        
        private static void OnHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ShadowSettingHeader control = (ShadowSettingHeader)d;
            control.Header = (string)e.NewValue;
        }
        private static void OnDescriptionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ShadowSettingHeader control = (ShadowSettingHeader)d;
            control.Description = (string)e.NewValue;
            control.IsShowDescription = (!string.IsNullOrEmpty(control.Description)).ToVisibility();
        }
        private static void OnIsShowDescriptionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ShadowSettingHeader control = (ShadowSettingHeader)d;
            control.IsShowDescription = (Visibility)e.NewValue;
        }
    }
}
