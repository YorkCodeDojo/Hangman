using HangmanTest.Services.DTOs;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HangmanTest.Services
{
    class DefinitionFromOxfordDictionaryQuery : DefinitionQuery
    {
        public async Task<string> Evaluate(string word)
        {

            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri("https://od-api.oxforddictionaries.com");

                client.DefaultRequestHeaders.Add("app_id", "c7cd133f");
                client.DefaultRequestHeaders.Add("app_key", "CHANGE_ME");

                var response = await client.GetAsync($"/api/v1/entries/en/{word}");
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {

                    var json = await response.Content.ReadAsStringAsync();
                    var obj = JsonConvert.DeserializeObject<Dictionary>(json);

                    return obj.results[0].lexicalEntries[0].entries[0].senses[0].definitions[0];
                }
                else
                {
                    return "";
                }
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
