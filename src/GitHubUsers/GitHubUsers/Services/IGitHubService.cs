using GitHubUsers.Models;
using System.Threading.Tasks;

namespace GitHubUsers.Services
{
    public interface IGitHubService
    {
        Task<GitHubUser> GetUser(string username);
    }
}