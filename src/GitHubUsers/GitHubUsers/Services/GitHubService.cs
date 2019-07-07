using GitHubUsers.Exceptions;
using GitHubUsers.HttpClients;
using GitHubUsers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitHubUsers.Services
{
    public class GitHubService : IGitHubService
    {
        private readonly IHttpClient _httpClient;

        public GitHubService(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GitHubUser> GetUser(string username)
        {
            try
            {
                var userResponse = await _httpClient.GetResult<GitHubUserResponse>($"https://api.github.com/users/{username}");

                var repos = await _httpClient.GetResult<IEnumerable<GitHubUserRepoResponse>>(userResponse.ReposUrl);

                var user = new GitHubUser
                {
                    Username = userResponse.Username,
                    Name = userResponse.Name,
                    Location = userResponse.Location,
                    AvatarUrl = userResponse.AvatarUrl,
                    Repositories = repos.Select(r => new GitHubUser.Repository
                    {
                        Name = r.Name,
                        Url = r.Url,
                        StargazerCount = r.StargazerCount
                    })
                };

                return user;
            }
            catch (NotFoundException ex)
            {
                // Log as Warning or Info here, using something like Loggly, Elastic Search etc
                throw;
            }
            catch (Exception ex)
            {
                // Log as Error here, using something like Loggly, Elastic Search etc
                throw;
            }
        }
    }
}