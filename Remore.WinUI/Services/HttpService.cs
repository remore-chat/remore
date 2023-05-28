using Remore.WinUI.Contracts.Services;
using Remore.WinUI.ViewModels;

namespace Remore.WinUI.Services;

public class HttpService
{
    private readonly ILocalSettingsService _localSettings;
    private readonly INavigationService _navigation;
    private string _token;

    public string Token
    {
        get
        {
            return _token;
        }
        set
        {
            _token = value;
            _ = UpdateTokenAsync();
        }
    }

    public bool IsAuthorized
    {
        get;set;
    }

    public string BaseUrl => "http://localhost:5000/api/";
    public HttpClient Client => CreateClient();

    private bool _isInitialized = false;

    public HttpService(ILocalSettingsService localSettings, INavigationService navigation)
    {
        _localSettings = localSettings;
        _navigation = navigation;
    }


    public async Task InitializeAsync()
    {

        if (_isInitialized)
            return;
        var token = await _localSettings.ReadSettingAsync<string>("AccessToken");
        if (token != null)
        {
            _token = token;
            IsAuthorized = true;
            using var client = Client;
            var resp = await client.GetAsync("auth/check");
            if (!resp.IsSuccessStatusCode)
            {
                _token = null;
                IsAuthorized = false;
            }
        }
        _isInitialized = true;
    }

    private async Task UpdateTokenAsync()
    {
        await _localSettings.SaveSettingAsync("AccessToken", _token);
    }


    private HttpClient CreateClient()
    {
        var handler = new InterceptorMessageHandler();
        handler.TokensDied += (s, e) =>
        {
            if (!IsAuthorized) return;
            IsAuthorized = false;
            _navigation.NavigateTo(typeof(UnauthorizedViewModel).FullName);
        };
        var client = new HttpClient();
        client.BaseAddress = new Uri(BaseUrl);
        if (Token != null)
        {
            client.DefaultRequestHeaders.Authorization = new("Bearer", Token);
        }

        return client;
    }

}
