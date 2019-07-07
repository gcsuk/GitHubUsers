using Newtonsoft.Json;

namespace GitHubUsers.Models
{
    public class GitHubUserResponse
    {
        [JsonProperty("login")]
        public string Username { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; set; }
        [JsonProperty("repos_url")]
        public string ReposUrl { get; set; }
    }
}