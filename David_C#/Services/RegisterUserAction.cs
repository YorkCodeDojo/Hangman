using System.Threading.Tasks;

namespace HangmanTest.Services
{
    interface RegisterUserAction
    {
        Task<string> Execute(string username, string password); 
    }
}
