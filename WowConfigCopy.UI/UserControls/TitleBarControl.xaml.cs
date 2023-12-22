using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WowConfigCopy.UI.UserControls;

public partial class TitleBarControl : UserControl
{
    public TitleBarControl()
    {
        InitializeComponent();
    }
    
    private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            Window.GetWindow(this)?.DragMove();
        }
    }
}