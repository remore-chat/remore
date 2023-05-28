using Microsoft.UI.Xaml.Controls;
using Remore.WinUI.Contracts.Services;
using Remore.WinUI.Services;
using Remore.WinUI.ViewModels;

namespace Remore.WinUI.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel
    {
        get;
    }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();

        InitializeComponent();
        Loaded += MainPage_Loaded;
    }

    private void MainPage_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        if (!App.GetService<AuthService>().IsAuthorized) App.GetService<INavigationService>().NavigateTo(typeof(UnauthorizedViewModel).FullName);
    }
}
