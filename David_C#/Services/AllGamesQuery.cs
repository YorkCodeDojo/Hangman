using HangmanTest.Services.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HangmanTest.Services
{
    interface AllGamesQuery
    {
        Task<List<Game>> Evalulate();
    }
}
