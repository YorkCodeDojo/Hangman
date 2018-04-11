using HangmanTest.Services.DTOs;
using System.Threading.Tasks;

namespace HangmanTest.Services
{
    interface SpecificGameQuery
    {
        Task<Game> Evalulate(string id);
    }
}
