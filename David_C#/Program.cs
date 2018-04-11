using HangmanTest.Services;
using HangmanTest.Services.DTOs;
using HangmanTest.WordList;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HangmanTest
{
    class Program
    {
        static async Task Main(string[] args)
        {

            try
            {
                var hangmanServer = HangmanServer();
                var filter = new FilterList();
                var makeMoveAction = new PlayMoveOnServerAction(hangmanServer);
                var newGameAction = new StartNewGameOnServerAction(hangmanServer);

                //var registerUserAction = new RegisterUserOnServerAction(hangmanServer);
                //var x_access_token = await registerUserAction.Execute(username: "MrDavidBetteridge", password: "NotTelling");
                var x_access_token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjVhY2U1NjliNWU3OTM4MDAxNDBhMjYyNCIsImlhdCI6MTUyMzQ3MjAyNywiZXhwIjoxNTIzNTU4NDI3fQ.UuEm6IZFrlGfYxixKzddus-STgV_9NTSKO12pU8xCvU";
                Login(hangmanServer, x_access_token);

                while (true)
                {
                    await PlayGame(makeMoveAction, filter, newGameAction);
                    Console.WriteLine("");
                    Console.WriteLine("------------------------------------");
                    Console.WriteLine("");

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return;
            }

        }

        private static async Task PlayGame(PlayMoveOnServerAction makeMoveAction, FilterList filter, StartNewGameOnServerAction newGameAction)
        {
            var words = AllWords();
            var game = await newGameAction.Execute();
            var wordLength = game.progress.Length;

            Console.WriteLine($"The word has {wordLength} letters.");
            words = filter.FilterByLength(words, wordLength);

            while (!game.complete)
            {
                var suggestion = filter.SuggestLetterIgnoringDuplicates(words, game.lettersGuessed.Select(d => d[0]).ToList());
                Console.Write($"Guessing {suggestion} - Possible words {words.Count()}");

                var previousMissesCount = game.misses;
                game = await makeMoveAction.Execute(suggestion);
                var newMissedCounts = game.misses;

                if (newMissedCounts > previousMissesCount)
                {
                    words = filter.RemoveWordsWhichDoNotContain(words, suggestion);
                    Console.WriteLine($" - Wrong");
                }
                else
                {
                    Console.WriteLine($" - Correct -  {ConvertToWord(game.progress)}");

                    for (int i = 0; i < game.progress.Length; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(game.progress[i]) && game.progress[i][0] == suggestion)
                        {
                            words = filter.RemoveWordsWithOutALetterInKnownPosition(words, i, suggestion);
                        }
                    }
                }
            }

            if (DidWeWin(game))
            {
                var finalWord = ConvertToWord(game.progress);
                Console.WriteLine($"Well done - we won in {game.misses + game.progress.Count()} guesses.  The word was {finalWord}");

                var definitionQuery = new DefinitionFromOxfordDictionaryQuery();
                var meaning = await definitionQuery.Evaluate(finalWord);

                if (!string.IsNullOrWhiteSpace(meaning))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Word means " + meaning);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            else
            {
                Console.WriteLine($"Sorry,  but we lost :-(.  {ConvertToWord(game.progress)}");
            }
        }

        private static IEnumerable<Word> AllWords()
        {
            var wordsJSON = File.ReadAllText(@"SOWPODS.json");
            var words = JsonConvert.DeserializeObject<List<Word>>(wordsJSON) as IEnumerable<Word>;
            return words;
        }

        private static string ConvertToWord(string[] letters) => string.Join("", letters.Select(s => s ?? "_"));
        private static bool DidWeWin(Game game) => game.progress.All(w => w != null);

        private static void Login(HttpClient hangmanServer, string x_access_token)
        {
            hangmanServer.DefaultRequestHeaders.Add("x-access-token", x_access_token);
        }

        private static HttpClient HangmanServer()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("http://dojo-hangman-server.herokuapp.com")
            };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
    }



}
