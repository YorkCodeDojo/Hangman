using HangmanTest.Services.DTOs;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HangmanTest.Services
{
    class RegisterUserOnServerAction : RegisterUserAction
    {
        private readonly HttpClient _httpClient;
        public RegisterUserOnServerAction(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> Execute(string username, string password)
        {
            var register = new Register()
            {
                username = username,
                password = password
            };

            var json = JsonConvert.SerializeObject(register);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/auth/register", content);
            response.EnsureSuccessStatusCode();

            var resultJSON = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<RegisterResult>(resultJSON);

            return result.token;
        }
    }
}
