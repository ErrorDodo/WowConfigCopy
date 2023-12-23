using System.Windows.Controls;
using WowConfigCopy.UI.ViewModels;

namespace WowConfigCopy.UI.Views;

public partial class Accounts : UserControl
{
    public Accounts(AccountsViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}