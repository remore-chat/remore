namespace Remore.Backend.Models.Dto
{
    public class SigninResponseDto
    {
        public string AccessToken { get; set; }
        public PrivateUser User { get; set; }
    }
}
