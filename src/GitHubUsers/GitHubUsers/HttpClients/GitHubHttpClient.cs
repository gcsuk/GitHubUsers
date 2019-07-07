using GitHubUsers.Exceptions;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace GitHubUsers.HttpClients
{
    public class GitHubHttpClient : IHttpClient
    {
        // Historically I would have had this local to the method and wrapped it in a using statement, but it has been
        // shown to be error prone and less performant so using a static variable instead
        // REF: https://aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/
        // REF: https://ankitvijay.net/2016/09/25/dispose-httpclient-or-have-a-static-instance/
        private static readonly HttpClient _httpClient = new HttpClient();

        public GitHubHttpClient()
        {
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "GitHub-Users");
        }

        public async Task<T> GetResult<T>(string url)
        {
            var response = await _httpClient.GetAsync(new Uri(url));

            var responseData = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<T>(responseData);
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                throw new NotFoundException();
            else
                throw new Exception(responseData);
        }
    }
}