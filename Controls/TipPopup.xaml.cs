// Copyright (c) Richasy. All rights reserved.

 
namespace ShadowViewer.Controls
{
    /// <summary>
    /// 消息提醒.
    /// </summary>
    public sealed partial class TipPopup : UserControl
    {
        /// <summary>
        /// 显示文本.
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        /// <summary>
        /// <see cref="Text"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(TipPopup), new PropertyMetadata(string.Empty));

        /// <summary>
        /// Initializes a new instance of the <see cref="TipPopup"/> class.
        /// </summary>
        public TipPopup() => InitializeComponent();

        public double DisplaySeconds { get; set; } = 2;
        /// <summary>
        /// Initializes a new instance of the <see cref="TipPopup"/> class.
        /// </summary>
        /// <param name="text">要显示的文本.</param>
        /// <param name="type">信息级别</param>
        /// <param name="displaySeconds">显示的时间</param>
        public TipPopup(string text, InfoBarSeverity type = InfoBarSeverity.Informational, double displaySeconds = 2)
            : this()
        {
            Text = text;
            DisplaySeconds = displaySeconds;
            switch (type)
            {
                case InfoBarSeverity.Informational:
                    InformationIcon.Visibility = Visibility.Visible;
                    break;
                case InfoBarSeverity.Success:
                    SuccessIcon.Visibility = Visibility.Visible;
                    break;
                case InfoBarSeverity.Warning:
                    WarningIcon.Visibility = Visibility.Visible;
                    break;
                case InfoBarSeverity.Error:
                    ErrorIcon.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        
        
    }
}
