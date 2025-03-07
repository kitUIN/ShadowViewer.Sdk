using System.Windows.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.Xaml.Interactivity;

namespace ShadowViewer.Core.Utils;

/// <summary>
///     Executes a specified <see cref="T:System.Windows.Input.ICommand" /> when invoked.
/// </summary>
public class InvokeCommandWithArgsAction : DependencyObject, IAction
{
    /// <summary>
    ///     Identifies the <seealso cref="P:Microsoft.Xaml.Interactions.Core.InvokeCommandWithArgsAction.Command" /> dependency
    ///     property.
    /// </summary>
    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof(Command),
        typeof(ICommand), typeof(InvokeCommandWithArgsAction), new PropertyMetadata(null));

    /// <summary>
    ///     Identifies the <seealso cref="P:Microsoft.Xaml.Interactions.Core.InvokeCommandWithArgsAction.CommandParameter" />
    ///     dependency property.
    /// </summary>
    public static readonly DependencyProperty CommandParameterProperty =
        DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(InvokeCommandWithArgsAction),
            new PropertyMetadata(null));

    /// <summary>
    ///     Identifies the <seealso cref="P:Microsoft.Xaml.Interactions.Core.InvokeCommandWithArgsAction.InputConverter" />
    ///     dependency property.
    /// </summary>
    public static readonly DependencyProperty InputConverterProperty =
        DependencyProperty.Register(nameof(InputConverter), typeof(IValueConverter),
            typeof(InvokeCommandWithArgsAction),
            new PropertyMetadata(null));

    /// <summary>
    ///     Identifies the
    ///     <seealso cref="P:Microsoft.Xaml.Interactions.Core.InvokeCommandWithArgsAction.InputConverterParameter" />
    ///     dependency property.
    /// </summary>
    public static readonly DependencyProperty InputConverterParameterProperty =
        DependencyProperty.Register(nameof(InputConverterParameter), typeof(object),
            typeof(InvokeCommandWithArgsAction),
            new PropertyMetadata(null));

    /// <summary>
    ///     Identifies the
    ///     <seealso cref="P:Microsoft.Xaml.Interactions.Core.InvokeCommandWithArgsAction.InputConverterLanguage" /> dependency
    ///     property.
    /// </summary>
    public static readonly DependencyProperty InputConverterLanguageProperty =
        DependencyProperty.Register(nameof(InputConverterLanguage), typeof(string), typeof(InvokeCommandWithArgsAction),
            new PropertyMetadata(string.Empty));

    /// <summary>
    ///     Gets or sets the command this action should invoke. This is a dependency property.
    /// </summary>
    public ICommand? Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    /// <summary>
    ///     Gets or sets the parameter that is passed to <see cref="M:System.Windows.Input.ICommand.Execute(System.Object)" />.
    ///     If this is not set, the parameter from the
    ///     <seealso cref="M:Microsoft.Xaml.Interactions.Core.InvokeCommandWithArgsAction.Execute(System.Object,System.Object)" />
    ///     method will be used.
    ///     This is an optional dependency property.
    /// </summary>
    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    /// <summary>
    ///     Gets or sets the converter that is run on the parameter from the
    ///     <seealso cref="M:Microsoft.Xaml.Interactions.Core.InvokeCommandWithArgsAction.Execute(System.Object,System.Object)" />
    ///     method.
    ///     This is an optional dependency property.
    /// </summary>
    public IValueConverter? InputConverter
    {
        get => (IValueConverter)GetValue(InputConverterProperty);
        set => SetValue(InputConverterProperty, value);
    }

    /// <summary>
    ///     Gets or sets the parameter that is passed to the
    ///     <see cref="M:Microsoft.UI.Xaml.Data.IValueConverter.Convert(System.Object,System.Type,System.Object,System.String)" />
    ///     method of <see cref="P:Microsoft.Xaml.Interactions.Core.InvokeCommandWithArgsAction.InputConverter" />.
    ///     This is an optional dependency property.
    /// </summary>
    public object InputConverterParameter
    {
        get => GetValue(InputConverterParameterProperty);
        set => SetValue(InputConverterParameterProperty, value);
    }

    /// <summary>
    ///     Gets or sets the language that is passed to the
    ///     <see cref="M:Microsoft.UI.Xaml.Data.IValueConverter.Convert(System.Object,System.Type,System.Object,System.String)" />
    ///     method of <see cref="P:Microsoft.Xaml.Interactions.Core.InvokeCommandWithArgsAction.InputConverter" />.
    ///     This is an optional dependency property.
    /// </summary>
    public string InputConverterLanguage
    {
        get => (string)GetValue(InputConverterLanguageProperty);
        set => SetValue(InputConverterLanguageProperty, value);
    }

    /// <summary>Executes the action.</summary>
    /// <param name="sender">
    ///     The <see cref="T:System.Object" /> that is passed to the action by the behavior. Generally this is
    ///     <seealso cref="P:Microsoft.Xaml.Interactivity.IBehavior.AssociatedObject" /> or a target object.
    /// </param>
    /// <param name="parameter">The value of this parameter is determined by the caller.</param>
    /// <returns>True if the command is successfully executed; else false.</returns>
    public object Execute(object sender, object parameter)
    {
        if (Command == null) return false;
        var param = new CommandWithArgs(
            InputConverter == null
                ? parameter
                : InputConverter.Convert(parameter, typeof(object), InputConverterParameter,
                    InputConverterLanguage),
            CommandParameter
        );
        if (!Command.CanExecute(param)) return false;
        Command.Execute(param);
        return true;
    }
}