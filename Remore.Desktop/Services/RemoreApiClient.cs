using Remore.WinUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Remore.Desktop.Services
{
    public class RemoreApiClient
    {
        public User User { get; set; }
        private string _token;

        public string Token
        {
            get { return _token; }
            set
            {
                _token = value;
                _client.DefaultRequestHeaders.Authorization = new("Bearer", value);
            }
        }

        public bool IsAuthenticated => User != null;
        private HttpClient _client;
        public RemoreApiClient()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:5000/api");
        }

        public async Task SignInAsync(string email, string password)
        {
            var result = await _client.GetFromJsonAsync<ResponseEntity<SigninResponse>>($"auth?email={email}&password={password}");
            if (result.Ok)
            {

            }
        }
    }
}
