using System.ComponentModel.DataAnnotations;

namespace GitHubUsers.ViewModels
{
    public class UserRequestVM
    {
        [Required(ErrorMessage = "Please enter a username.")]
        public string Username { get; set; }
    }
}