using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Remore.WinUI.Contracts.Services;
using Remore.WinUI.Services;
using Remore.WinUI.Views;

namespace Remore.WinUI.ViewModels;

public partial class UnauthorizedViewModel : ObservableRecipient
{
    public UnauthorizedViewModel(AuthService authService, INavigationService navigation)
    {
        _authService = authService;
        _navigation = navigation;
    }
    private readonly AuthService _authService;
    private readonly INavigationService _navigation;
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsLoaded))]
    private bool isLoading = false;

    public bool IsLoaded => !isLoading;

    public bool HasErrors => _errors != null;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasErrors))]
    private string _errors = null;

    [ObservableProperty]
    private string _email;

    [ObservableProperty]
    private string _password;

    [RelayCommand]
    public async Task SignInAsync()
    {
        App.DispatcherQueue.TryEnqueue(() => { IsLoading = true; Errors = null; });
        await Task.Delay(1500);
        var errors = await _authService.SignInAsync(_email, _password);
        if (errors == null)
        {
            _navigation.NavigateTo(typeof(MainViewModel).FullName);
        }
        else
        {
            App.DispatcherQueue.TryEnqueue(async () =>
            {
                Errors = string.Join("\n", errors);
            });
        }
        App.DispatcherQueue.TryEnqueue(() => IsLoading = false);

    }
}
