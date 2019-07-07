using GitHubUsers.Exceptions;
using GitHubUsers.Services;
using GitHubUsers.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GitHubUsers.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGitHubService _gitHubService;

        public HomeController(IGitHubService gitHubService)
        {
            _gitHubService = gitHubService;
        }

        public async Task<ActionResult> Index()
        {
            return View(new UserResponseVM());
        }

        [HttpPost]
        public async Task<ActionResult> Index(UserRequestVM model)
        {
            if (!ModelState.IsValid)
                return View(new UserResponseVM { Username = model.Username });

            try
            {
                var user = await _gitHubService.GetUser(model.Username);

                var vm = new UserResponseVM
                {
                    Username = user.Username,
                    Name = user.Name,
                    Location = user.Location,
                    AvatarUrl = user.AvatarUrl,
                    Repositories = user.Repositories.OrderByDescending(r => r.StargazerCount)
                                                    .Take(5)
                                                    .Select(r => new UserResponseVM.Repository
                                                    {
                                                        Name = r.Name,
                                                        Url = r.Url
                                                    })
                };

                return View(vm);
            }
            catch (NotFoundException)
            {
                ModelState.AddModelError("Username", "The specified username was not found.");
                return View(new UserResponseVM { Username = model.Username });
            }
        }
    }
}