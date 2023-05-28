using System.ComponentModel.DataAnnotations;

namespace Remore.Backend.Models.Dto
{
    public class SigninRequestDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
