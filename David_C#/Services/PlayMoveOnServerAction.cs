using HangmanTest.Services.DTOs;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HangmanTest.Services
{
    public class PlayMoveOnServerAction : PlayMoveAction
    {

        private readonly HttpClient _httpClient;
        public PlayMoveOnServerAction(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<Game> Execute(char letter)
        {
            var play = new Play()
            {
                letter = letter,
            };

            var json = JsonConvert.SerializeObject(play);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, "/api/games/current")
            {
                Content = content
            };

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var resultJSON = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Game>(resultJSON);

            return result;
        }
    }
}
