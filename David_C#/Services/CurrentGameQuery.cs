using HangmanTest.Services.DTOs;
using System.Threading.Tasks;

namespace HangmanTest.Services
{
    interface CurrentGameQuery
    {
        Task<Game> Evalulate();
    }
}
