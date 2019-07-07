using System.Collections.Generic;

namespace GitHubUsers.Models
{
    public class GitHubUser
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string AvatarUrl { get; set; }
        public string Location { get; set; }
        public IEnumerable<Repository> Repositories { get; set; }

        public class Repository
        {
            public string Name { get; set; }
            public string Url { get; set; }
            public int StargazerCount { get; set; }
        }
    }
}