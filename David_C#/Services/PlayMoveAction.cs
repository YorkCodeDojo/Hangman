using HangmanTest.Services.DTOs;
using System.Threading.Tasks;

namespace HangmanTest.Services
{
    interface PlayMoveAction
    {
        Task<Game> Execute(char letter);
    }
}
