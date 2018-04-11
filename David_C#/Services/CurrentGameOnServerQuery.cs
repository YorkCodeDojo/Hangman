using HangmanTest.Services.DTOs;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace HangmanTest.Services
{
    public class CurrentGameOnServerQuery : CurrentGameQuery
    {
        private readonly HttpClient _httpClient;
        public CurrentGameOnServerQuery(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Game> Evalulate()
        {
            var response = await _httpClient.GetAsync("/api/games/current");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Game>(json);
        }
    }
}
