using HangmanTest.Services.DTOs;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace HangmanTest.Services
{
    class StartNewGameOnServerAction : StartNewGameAction
    {
        private readonly HttpClient _httpClient;
        public StartNewGameOnServerAction(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Game> Execute()
        {
            var response = await _httpClient.PostAsync("/api/games", null);
            response.EnsureSuccessStatusCode();

            var resultJSON = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Game>(resultJSON);

            return result;
        }
    }
}
