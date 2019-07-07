using System.Threading.Tasks;

namespace GitHubUsers.HttpClients
{
    /// <summary>
    /// Abstracted HttpClient functionality. Abstracting this makes testing/mocking easier.
    /// </summary>
    public interface IHttpClient
    {
        Task<T> GetResult<T>(string url);
    }
}