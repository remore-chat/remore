using System.ComponentModel.DataAnnotations;

namespace Remore.WinUI.Models
{
    public class SigninRequestDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
