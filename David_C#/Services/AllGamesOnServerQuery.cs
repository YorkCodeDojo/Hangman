using HangmanTest.Services.DTOs;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace HangmanTest.Services
{
    public class AllGamesOnServerQuery : AllGamesQuery
    {
        private readonly HttpClient _httpClient;
        public AllGamesOnServerQuery(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Game>> Evalulate()
        {
            var response = await _httpClient.GetAsync("/api/games");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<Game>>(json);
        }
    }
}
