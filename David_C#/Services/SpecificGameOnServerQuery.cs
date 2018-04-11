using HangmanTest.Services.DTOs;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace HangmanTest.Services
{
    public class SpecificGameOnServerQuery : SpecificGameQuery
    {
        private readonly HttpClient _httpClient;
        public SpecificGameOnServerQuery(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Game> Evalulate(string id)
        {
            var response = await _httpClient.GetAsync($"/api/games/{id}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Game>(json);
        }
    }
}
