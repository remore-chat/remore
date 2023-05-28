using Microsoft.UI.Xaml.Controls;

using Remore.WinUI.ViewModels;

namespace Remore.WinUI.Views;

public sealed partial class UnauthorizedPage : Page
{
    public UnauthorizedViewModel ViewModel
    {
        get;
    }

    public UnauthorizedPage()
    {
        ViewModel = App.GetService<UnauthorizedViewModel>();
        InitializeComponent();
    }
}
