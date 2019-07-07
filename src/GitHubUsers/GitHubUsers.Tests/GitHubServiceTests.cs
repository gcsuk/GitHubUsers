using GitHubUsers.Exceptions;
using GitHubUsers.HttpClients;
using GitHubUsers.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitHubUsers.Tests
{
    public class GitHubServiceTests
    {
        private const string TestUsername = "gcsuk";

        [Test]
        public async Task GetUser_When_valid_username_requested_return_user_with_details_and_repositories()
        {
            // Arrange
            var httpClientMock = new Mock<IHttpClient>();

            httpClientMock.Setup(h => h.GetResult<GitHubUserResponse>(It.Is<string>(s => s.Contains(TestUsername)))).ReturnsAsync(UserResponse);

            httpClientMock.Setup(h => h.GetResult<IEnumerable<GitHubUserRepoResponse>>(It.IsAny<string>())).ReturnsAsync(GetRepoResponse());

            var sut = new Services.GitHubService(httpClientMock.Object);

            // Act
            var result = await sut.GetUser(TestUsername);

            // Assert
            Assert.AreEqual(TestUsername, result.Username);
            Assert.AreEqual(10, result.Repositories.Count());
            // As long as only one subject is tested, there's no harm testing more properties here
        }

        [Test]
        public void GetUser_When_invalid_username_requested_throw_not_found_exception()
        {
            // Arrange
            var httpClientMock = new Mock<IHttpClient>();

            httpClientMock.Setup(h => h.GetResult<GitHubUserResponse>(It.Is<string>(s => s.Contains(TestUsername)))).ThrowsAsync(new NotFoundException());

            var sut = new Services.GitHubService(httpClientMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(async() => await sut.GetUser(TestUsername));
        }

        private GitHubUserResponse UserResponse => new GitHubUserResponse
        {
            Username = TestUsername,
            Name = "Rob King",
            Location = "Newcastle",
            AvatarUrl = "https://avatars3.githubusercontent.com/u/11536864?v=4",
            ReposUrl = "https://api.github.com/users/gcsuk/repos"
        };

        private IEnumerable<GitHubUserRepoResponse> GetRepoResponse()
        {
            var repos = new List<GitHubUserRepoResponse>();

            for (int i = 0; i < 10; i++)
            {
                repos.Add(new GitHubUserRepoResponse
                {
                    Name = $"Repo{i}",
                    Url = $"https://github.com/gcsuk/Repo{i}",
                    StargazerCount = (i + (i * 10))
                });
            }

            return repos;
        }
    }
}
