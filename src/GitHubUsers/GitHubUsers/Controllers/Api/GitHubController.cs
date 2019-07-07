using GitHubUsers.Exceptions;
using GitHubUsers.Services;
using GitHubUsers.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace GitHubUsers.Controllers.Api
{
    public class GitHubController : ApiController
    {
        private readonly IGitHubService _gitHubService;

        public GitHubController(IGitHubService gitHubService)
        {
            _gitHubService = gitHubService;
        }

        /// <summary>
        /// Retrieves a specified user's details
        /// </summary>
        /// <param name="username">The username of the user to return</param>
        /// <returns>User object including list of popular repositories</returns>
        [Route("api/users/{username}")]
        public async Task<IHttpActionResult> GetUser(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return BadRequest("Invalid username");

            try
            {
                var user = await _gitHubService.GetUser(username);

                var vm = new UserResponseVM
                {
                    Username = user.Username,
                    Name = user.Name,
                    Location = user.Location,
                    AvatarUrl = user.AvatarUrl,
                    Repositories = user.Repositories.Select(r => new UserResponseVM.Repository
                    {
                        Name = r.Name,
                        Url = r.Url
                    })
                };

                return Ok(vm);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}