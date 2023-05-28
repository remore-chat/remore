using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Remore.WinUI.Contracts.Services;
using Remore.WinUI.Models;
using Remore.WinUI.Services;

namespace Remore.WinUI.ViewModels;

public partial class MainViewModel : ObservableRecipient
{
    private readonly AuthService _authService;

    public User User
    {
        get;set;
    }

    [RelayCommand]
    public void Logout()
    {
        _authService.Logout();
    }

    public MainViewModel(AuthService authService)
    {
        _authService = authService;
        User = _authService.User;
    }
}
