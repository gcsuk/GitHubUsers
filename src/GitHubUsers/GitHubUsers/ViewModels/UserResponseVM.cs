using System.Collections.Generic;

namespace GitHubUsers.ViewModels
{
    public class UserResponseVM
    {
        public string Username { get; set; } = "";
        public string AvatarUrl { get; set; } = "";
        public string Name { get; set; } = "";
        public string Location { get; set; } = "";
        public IEnumerable<Repository> Repositories { get; set; } = new List<Repository>();

        public class Repository
        {
            public string Name { get; set; }
            public string Url { get; set; }
        }
    }
}