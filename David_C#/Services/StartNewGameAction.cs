using HangmanTest.Services.DTOs;
using System.Threading.Tasks;

namespace HangmanTest.Services
{
    interface StartNewGameAction
    {
        Task<Game> Execute(); 
    }
}
