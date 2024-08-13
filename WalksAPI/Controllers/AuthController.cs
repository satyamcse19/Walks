using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WalksAPI.CustomActionFilters;
using WalksAPI.Interfaces.Repositories;
using WalksAPI.Models.DTO;
using WalksAPI.Repositories;

namespace WalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository _tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager,ITokenRepository tokenRepository)
        {
            this._userManager = userManager;
            this._tokenRepository = tokenRepository;
        }

        //post:api/Register
        [HttpPost]
        [Route("Register")]
        [ValidateModel]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Username

            };
            var identityResult = await _userManager.CreateAsync(identityUser, registerDto.Password);
            if (identityResult.Succeeded)
            {
                //add roles to thise user
                if(registerDto.Roles != null && registerDto.Roles.Any())
                {
                    identityResult = await _userManager.AddToRolesAsync(identityUser , registerDto.Roles);
                    if (identityResult.Succeeded)
                    {
                        return Ok("User was registered ! Please login");
                    }
                }

               
            }
            return BadRequest("Something went wrong");
        }
        [HttpPost]
        [Route("Login")]
        [ValidateModel]
        public async Task<IActionResult> Login(LoginDto loginRequestDto)
        {
            var user = await _userManager.FindByEmailAsync(loginRequestDto.Username);
            if (user != null) 
            {
                var checkPasswordResult =await _userManager.CheckPasswordAsync(user,loginRequestDto.Password);
                if (checkPasswordResult)
                {
                    //get Roles of user
                    var roles= await _userManager.GetRolesAsync(user);
                    if (roles !=null)
                    {
                        //create token
                        string JwtTokendata = _tokenRepository.CreateJWTToken(user, roles.ToList());
                        var Responce = new LoginResponceDto
                        {
                            JwtToken = JwtTokendata,
                        };
                        return Ok(Responce);
                    }
                    
                }
            }
            return BadRequest("Username and Password Incorrect");
        }
    }
}
