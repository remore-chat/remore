using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OperationResult;
using Remore.Backend.DAL.Models;
using Remore.Backend.Models;
using Remore.Backend.Models.Dto;
using static OperationResult.Helpers;

namespace Remore.Backend.Services
{
    public class AuthService
    {
        private TokenService _tokenService;
        private UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly HttpContext? _context;

        public AuthService(UserManager<AppUser> userManager, IMapper mapper, TokenService tokenService, IHttpContextAccessor httpContextAccessor)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _mapper = mapper;
            _context = httpContextAccessor.HttpContext;
        }

        public async Task<PrivateUser> GetMeAsync()
        {
            var user = await _userManager.FindByEmailAsync(_context.User.Claims.First(x=>x.Type == ClaimTypes.Email).Value);
            return _mapper.Map<PrivateUser>(user);
        }
            
        public async Task<Result<SigninResponseDto, string>> SignInAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Error(ErrorCodes.InvalidCredentials.ToString());
            }
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
            if (!isPasswordValid)
            {
                return Error(ErrorCodes.InvalidCredentials.ToString());
            }
            var accessToken = _tokenService.CreateToken(user);
            return Ok(new SigninResponseDto()
            {
                User = _mapper.Map<PrivateUser>(user),
                AccessToken = accessToken
            });
        }
        public async Task<Result<object, List<string>>> SignUpAsync(string email, string username, string password)
        {
            var result = await _userManager.CreateAsync(new AppUser()
            {
                UserName = username,
                Email = email,
                DisplayName = username,
            }, password);
            if (result.Succeeded)
            {
                return null;
            }
            else
            {
                return result.Errors.Select(x => x.Code).ToList();
            }
        }
    }
}
