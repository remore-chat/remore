using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using FluentHttpClient;
using Newtonsoft.Json.Linq;
using Remore.WinUI.Contracts.Services;
using Remore.WinUI.Models;
using Remore.WinUI.ViewModels;
using Windows.Media.Protection.PlayReady;

namespace Remore.WinUI.Services;
public class AuthService
{
    private HttpService _httpService;
    private readonly INavigationService _navigation;
    private HttpClient _client;
    private bool _isInitialized;

    public AuthService(HttpService http, INavigationService navigation)
    {
        _httpService = http;
        _navigation = navigation;
        _client = http.Client;

    }
    public bool IsAuthorized => _httpService.IsAuthorized;
    public User User
    {
        get;
        private set;
    }

    /// <summary>
    ///  
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns>null if auth succeeded; list of errors if failed</returns>
    public async Task<List<string>?> SignInAsync(string email, string password)
    {
        var resp = await _client.
            UsingRoute("auth/signin")
            .WithJsonContent(new SigninRequestDto()
            {
                Email = email,
                Password = password
            })
            .PostAsync()
            .GetResponseStreamAsync()
            .DeserializeJsonAsync<ResponseEntity<SigninResponse>>();
        if (resp.Ok)
        {
            _httpService.Token = resp.Data.AccessToken;
            _httpService.IsAuthorized = true;
            User = resp.Data.User;
        }
        return resp!.Ok ? null : resp.Errors;
    }


    public void Logout()
    {
        _httpService.Token = null;
        _navigation.NavigateTo(typeof(UnauthorizedViewModel).FullName);
    }
    public async Task InitializeAsync()
    {

        if (_isInitialized)
            return;
        if (!IsAuthorized)
            return;
        var client = _client;
        var resp = await client.GetFromJsonAsync<ResponseEntity<User>>("auth/check");
        User = resp.Data;
        
        _isInitialized = true;
    }

}
