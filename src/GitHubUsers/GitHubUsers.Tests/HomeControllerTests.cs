using GitHubUsers.Controllers;
using GitHubUsers.Exceptions;
using GitHubUsers.Models;
using GitHubUsers.Services;
using GitHubUsers.ViewModels;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GitHubUsers.Tests
{
    public class HomeControllerTests
    {
        private const string TestUsername = "gcsuk";

        [Test]
        public async Task Index_Post_When_valid_username_requested_return_user_with_details_and_repositories()
        {
            var user = GetUser();

            var gitHubServiceMock = new Mock<IGitHubService>();

            gitHubServiceMock.Setup(g => g.GetUser(TestUsername)).ReturnsAsync(user);

            var sut = new HomeController(gitHubServiceMock.Object);

            var request = new UserRequestVM { Username = TestUsername };

            var result = await sut.Index(request) as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName));
            Assert.IsNotNull(result.Model);
            Assert.AreEqual(TestUsername, ((UserResponseVM)result.Model).Username);
            Assert.IsFalse(string.IsNullOrEmpty(((UserResponseVM)result.Model).Name));
            Assert.IsFalse(string.IsNullOrEmpty(((UserResponseVM)result.Model).Location));
            Assert.IsFalse(string.IsNullOrEmpty(((UserResponseVM)result.Model).AvatarUrl));
        }

        [Test]
        public async Task Index_Post_When_valid_username_requested_return_only_top_five_starred_repositories()
        {
            // Arrange
            var user = GetUser();

            var gitHubServiceMock = new Mock<IGitHubService>();

            gitHubServiceMock.Setup(g => g.GetUser(TestUsername)).ReturnsAsync(user);

            var sut = new HomeController(gitHubServiceMock.Object);

            var request = new UserRequestVM { Username = TestUsername };
            
            // Act
            var result = await sut.Index(request) as ViewResult;

            // Assert
            
            // There should be 5 repos returned
            Assert.AreEqual(5, ((UserResponseVM)result.Model).Repositories.Count());
            // Repos 0-4 have less stars than 5-9 so they should not be in the response.
            Assert.IsNull(((UserResponseVM)result.Model).Repositories.SingleOrDefault(r => r.Name == "Repo0"));
            Assert.IsNull(((UserResponseVM)result.Model).Repositories.SingleOrDefault(r => r.Name == "Repo1"));
            Assert.IsNull(((UserResponseVM)result.Model).Repositories.SingleOrDefault(r => r.Name == "Repo2"));
            Assert.IsNull(((UserResponseVM)result.Model).Repositories.SingleOrDefault(r => r.Name == "Repo3"));
            Assert.IsNull(((UserResponseVM)result.Model).Repositories.SingleOrDefault(r => r.Name == "Repo4"));
            // Repos 5-9 have more stars than 0-4 so they should be in the response.
            Assert.IsNotNull(((UserResponseVM)result.Model).Repositories.SingleOrDefault(r => r.Name == "Repo5"));
            Assert.IsNotNull(((UserResponseVM)result.Model).Repositories.SingleOrDefault(r => r.Name == "Repo6"));
            Assert.IsNotNull(((UserResponseVM)result.Model).Repositories.SingleOrDefault(r => r.Name == "Repo7"));
            Assert.IsNotNull(((UserResponseVM)result.Model).Repositories.SingleOrDefault(r => r.Name == "Repo8"));
            Assert.IsNotNull(((UserResponseVM)result.Model).Repositories.SingleOrDefault(r => r.Name == "Repo9"));
        }

        [Test]
        public async Task Index_Post_When_invalid_username_requested_return_empty_response_with_username()
        {
            // Arrange
            var user = GetUser();

            var gitHubServiceMock = new Mock<IGitHubService>();

            gitHubServiceMock.Setup(g => g.GetUser(TestUsername)).ThrowsAsync(new NotFoundException());

            var sut = new HomeController(gitHubServiceMock.Object);

            var request = new UserRequestVM { Username = TestUsername };

            // Act
            var result = await sut.Index(request) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName));
            Assert.IsNotNull(result.Model);
            Assert.IsFalse(string.IsNullOrEmpty(((UserResponseVM)result.Model).Username));
            Assert.IsTrue(string.IsNullOrEmpty(((UserResponseVM)result.Model).Name));
            Assert.IsTrue(string.IsNullOrEmpty(((UserResponseVM)result.Model).Location));
            Assert.IsTrue(string.IsNullOrEmpty(((UserResponseVM)result.Model).AvatarUrl));
        }

        private GitHubUser GetUser()
        {
            var user = new GitHubUser
            {
                Username = TestUsername,
                Name = "Rob King",
                Location = "Newcastle",
                AvatarUrl = "https://avatars3.githubusercontent.com/u/11536864?v=4"
            };

            var repositories = new List<GitHubUser.Repository>();

            for (int i = 0; i < 10; i++)
            {
                repositories.Add(new GitHubUser.Repository
                {
                    Name = $"Repo{i}",
                    Url = $"https://github.com/gcsuk/Repo{i}",
                    StargazerCount = (i + (i * 10))
                });
            }

            user.Repositories = repositories;

            return user;
        }
    }
}
