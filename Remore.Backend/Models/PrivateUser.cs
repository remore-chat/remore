namespace Remore.Backend.Models
{
    public class PrivateUser : PublicUser
    {
        public string Email { get; set; }
        public bool EmailConfirmed {get; set; }
    }
}
