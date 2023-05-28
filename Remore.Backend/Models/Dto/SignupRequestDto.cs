using System.ComponentModel.DataAnnotations;

namespace Remore.Backend.Models.Dto
{
    public class SignupRequestDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Username { get; set; }
    }
}
