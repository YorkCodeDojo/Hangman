using System.Threading.Tasks;

namespace HangmanTest.Services
{
    interface DefinitionQuery
    {
        Task<string> Evaluate(string word);
    }
}
