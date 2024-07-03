using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

namespace ShadowViewer.Controls
{
    /// <summary>
    /// 
    /// </summary>
    [ContentProperty(Name = "Content")]
    public class TitleBorder : Control
    {
        /// <summary>
        /// 
        /// </summary>
        public TitleBorder()
        {
            this.DefaultStyleKey = typeof(TitleBorder);
        }
        /// <summary>
        /// 获取或设置Content的值
        /// </summary>  
        public object Content
        {
            get => (object)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        /// <summary>
        /// 标识 Content 依赖属性。
        /// </summary>
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register(nameof(Content), typeof(object), typeof(TitleBorder), new PropertyMetadata(null, OnContentChanged));

        private static void OnContentChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            TitleBorder target = obj as TitleBorder;
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
            get => (DataTemplate)GetValue(ContentTemplateProperty);
            set => SetValue(ContentTemplateProperty, value);
        }

        /// <summary>
        /// 标识 ContentTemplate 依赖属性。
        /// </summary>
        public static readonly DependencyProperty ContentTemplateProperty =
            DependencyProperty.Register(nameof(ContentTemplate), typeof(DataTemplate), typeof(TitleBorder), new PropertyMetadata(null, OnContentTemplateChanged));

        private static void OnContentTemplateChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var target = obj as TitleBorder;
            var oldValue = (DataTemplate)args.OldValue;
            var newValue = (DataTemplate)args.NewValue;
            if (oldValue != newValue)
                target?.OnContentTemplateChanged(oldValue, newValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected virtual void OnContentTemplateChanged(DataTemplate oldValue, DataTemplate newValue)
        {

        }

        /// <summary>
        /// Header
        /// </summary>
        public object Header
        {
            get => (object)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        /// <summary>
        /// 标识 Header 依赖属性。
        /// </summary>
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(nameof(Header), typeof(object), typeof(TitleBorder), new PropertyMetadata(null, OnHeaderChanged));

        private static void OnHeaderChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var target = obj as TitleBorder;
            var oldValue = (object)args.OldValue;
            var newValue = (object)args.NewValue;
            if (oldValue != newValue)
                target?.OnHeaderChanged(oldValue, newValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected virtual void OnHeaderChanged(object oldValue, object newValue)
        {
        }

        public DataTemplate HeaderTemplate
        {
            get => (DataTemplate)GetValue(HeaderTemplateProperty);
            set => SetValue(HeaderTemplateProperty, value);
        }

        public static readonly DependencyProperty HeaderTemplateProperty =
            DependencyProperty.Register(nameof(HeaderTemplate), typeof(DataTemplate), typeof(TitleBorder), new PropertyMetadata(null, OnHeaderTemplateChanged));

        private static void OnHeaderTemplateChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var target = obj as TitleBorder;
            var oldValue = (DataTemplate)args.OldValue;
            var newValue = (DataTemplate)args.NewValue;
            if (oldValue != newValue)
                target?.OnHeaderTemplateChanged(oldValue, newValue);
        }

        protected virtual void OnHeaderTemplateChanged(DataTemplate oldValue, DataTemplate newValue)
        {

        }
    }
}
