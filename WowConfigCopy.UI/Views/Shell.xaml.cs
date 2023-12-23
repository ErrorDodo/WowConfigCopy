using System;
using System.Windows;
using System.Windows.Input;
using WowConfigCopy.UI.ViewModels;

namespace WowConfigCopy.UI.Views;

public partial class Shell : Window
{
    public Shell(ShellViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        viewModel.Initialize();
    }
}