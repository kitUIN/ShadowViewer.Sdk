using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ShadowViewer.Controls
{
    public sealed partial class PluginMainSection : UserControl
    {
        #region 标题
        public static readonly DependencyProperty HeaderProperty =
           DependencyProperty.Register(nameof(Header),
               typeof(string),
               typeof(PluginMainSection),
               new PropertyMetadata(default, new PropertyChangedCallback(OnHeaderChanged)));
        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
        private static void OnHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PluginMainSection control = (PluginMainSection)d;
            control.Header = (string)e.NewValue;
        }
        #endregion
        #region 简介
        public static readonly DependencyProperty DescriptionProperty =
           DependencyProperty.Register(nameof(Description),
               typeof(string),
               typeof(PluginMainSection),
               new PropertyMetadata(default, new PropertyChangedCallback(OnDescriptionChanged)));
        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }
        private static void OnDescriptionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PluginMainSection control = (PluginMainSection)d;
            control.Description = (string)e.NewValue;
            control.IsShowDescription = !string.IsNullOrEmpty(control.Description);
        }
        #endregion
        #region 是否显示简介
        public static readonly DependencyProperty IsShowDescriptionProperty =
           DependencyProperty.Register(nameof(IsShowDescription),
               typeof(bool),
               typeof(PluginMainSection),
               new PropertyMetadata(default, new PropertyChangedCallback(OnIsShowDescriptionChanged)));
        public bool IsShowDescription
        {
            get { return (bool)GetValue(IsShowDescriptionProperty); }
            set { SetValue(IsShowDescriptionProperty, value); }
        }
        private static void OnIsShowDescriptionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PluginMainSection control = (PluginMainSection)d;
            control.IsShowDescription = (bool)e.NewValue;
        }
        #endregion
        #region 是否启动插件
        public static readonly DependencyProperty IsEnableProperty =
           DependencyProperty.Register(nameof(IsEnable),
               typeof(bool),
               typeof(PluginMainSection),
               new PropertyMetadata(default, new PropertyChangedCallback(OnIsShowDescriptionChanged)));
        public bool IsEnable
        {
            get { return (bool)GetValue(IsShowDescriptionProperty); }
            set { SetValue(IsShowDescriptionProperty, value); }
        }
        private static void OnIsEnableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PluginMainSection control = (PluginMainSection)d;
            control.IsEnable = (bool)e.NewValue;
        }
        #endregion
        public PluginMainSection()
        {
            this.InitializeComponent();
        }
    }
}
