// Copyright (c) Richasy. All rights reserved.

namespace ShadowViewer.Controls
{
    /// <summary>
    /// 消息提醒.
    /// <remarks>请使用<see cref="NotificationHelper"/>,而非自己实例化</remarks>
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
        /// 显示文本.
        /// </summary>
        public InfoBarSeverity Severity
        {
            get { return (InfoBarSeverity)GetValue(SeverityProperty); }
            set { SetValue(SeverityProperty, value); }
        }
        /// <summary>
        /// <see cref="Text"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty SeverityProperty =
            DependencyProperty.Register(nameof(Severity), typeof(InfoBarSeverity), typeof(TipPopup), new PropertyMetadata(string.Empty));

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
            Severity = type;
        }

    }
}
