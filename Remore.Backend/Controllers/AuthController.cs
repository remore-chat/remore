using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Remore.Backend.Models;
using Remore.Backend.Models.Dto;
using Remore.Backend.Services;

namespace Remore.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [Authorize]
        [HttpGet("check")]
        public async Task<IActionResult> CheckAuthenticationAsync()
        {
            return Ok((await _authService.GetMeAsync()).AsOkResponse());
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUpAsync(SignupRequestDto signupRequest)
        {
            var result = await _authService.SignUpAsync(signupRequest.Email, signupRequest.Username, signupRequest.Password);
            if (result)
                return Ok(result.Value.AsOkResponse());
            return BadRequest(result.Error.AsErrorResponse());
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignInAsync(SigninRequestDto signinRequest)
        {
            var result = await _authService.SignInAsync(signinRequest.Email, signinRequest.Password);
            if (result)
                return Ok(result.Value.AsOkResponse());
            return BadRequest(result.Error.AsErrorResponse());
        }
    }
}
