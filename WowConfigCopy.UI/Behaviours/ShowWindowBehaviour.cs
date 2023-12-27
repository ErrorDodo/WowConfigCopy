using System.Windows;
using Microsoft.Xaml.Behaviors;

namespace WowConfigCopy.UI.Behaviours
{
    public class ShowWindowBehaviour : Behavior<Window>
    {
        public static readonly DependencyProperty IsVisibleProperty =
            DependencyProperty.Register(
                nameof(IsVisible), 
                typeof(bool), 
                typeof(ShowWindowBehaviour), 
                new PropertyMetadata(false, OnIsVisibleChanged));

        public bool IsVisible
        {
            get => (bool)GetValue(IsVisibleProperty);
            set => SetValue(IsVisibleProperty, value);
        }

        private static void OnIsVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not ShowWindowBehaviour {AssociatedObject: { } window}) return;
            if ((bool)e.NewValue)
            {
                window.Show();
            }
            else
            {
                window.Hide();
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            if (IsVisible)
            {
                AssociatedObject.Show();
            }
        }
    }
}