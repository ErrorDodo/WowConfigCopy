using System.Diagnostics;
using System.Windows;
using ICSharpCode.AvalonEdit;

namespace WowConfigCopy.UI.Behaviours
{
    public static class TextEditorBehaviour
    {
        public static readonly DependencyProperty TextProperty = 
            DependencyProperty.RegisterAttached(
                "Text",
                typeof(string),
                typeof(TextEditorBehaviour),
                new FrameworkPropertyMetadata(
                    default(string), 
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
                    TextChanged,
                    CoerceText));

        public static string GetText(TextEditor textEditor)
        {
            return (string)textEditor.GetValue(TextProperty);
        }

        public static void SetText(TextEditor textEditor, string value)
        {
            textEditor.SetValue(TextProperty, value);
        }

        private static void TextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textEditor = (TextEditor)d;
            if (textEditor.Document == null) return;
            var newText = (string)e.NewValue ?? string.Empty;
            textEditor.Document.Text = newText;
        }


        private static object CoerceText(DependencyObject d, object baseValue)
        {
            // This is a debugging aid. Set a breakpoint here if text is not being set.
            return baseValue;
        }
    }
}