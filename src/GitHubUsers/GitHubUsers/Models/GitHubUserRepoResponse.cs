using Newtonsoft.Json;

namespace GitHubUsers.Models
{
    public class GitHubUserRepoResponse
    {
        public string Name { get; set; }
        [JsonProperty("stargazers_count")]
        public int StargazerCount { get; set; }
        [JsonProperty("html_url")]
        public string Url { get; set; }
    }
}