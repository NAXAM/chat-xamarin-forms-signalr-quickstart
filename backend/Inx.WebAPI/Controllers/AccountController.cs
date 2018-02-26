using System;
using Inx.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Inx.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Options;
using Inx.WebAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Inx.WebAPI.Controllers
{
    [Route("api/account")]
    public class AccountController : Controller
    {
        readonly SignInManager<UserEntity> signInManager;
        readonly UserManager<UserEntity> userManager;
        readonly IOptions<JwtOptions> jwtOptions;

        public AccountController(
            SignInManager<UserEntity> signInManager,
            UserManager<UserEntity> userManager,
            IOptions<JwtOptions> jwtOptions
        )
        {
            this.jwtOptions = jwtOptions;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpPost("sign-in")]
        public async Task<ActionResult> SignIn([FromBody]SignInDto dto)
        {
            // TODO Validate DTO
            var result = await signInManager.PasswordSignInAsync(dto.Email, dto.Password, true, false);

            if (result.Succeeded)
            {
                var user = signInManager.UserManager.Users.SingleOrDefault(x => string.Equals(x.Email, dto.Email, StringComparison.OrdinalIgnoreCase));

                return GenerateToken(user);
            }

            return Unauthorized();
        }

        [HttpPost("sign-up")]
        public async Task<ActionResult> SignUp([FromBody]SignUpDto dto)
        {
            // TODO Validate DTO
            var user = new UserEntity
            {
                Email = dto.Email,
                UserName = dto.Email
            };

            var result = await userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
                var signInResult = signInManager.SignInAsync(user, true);

                user = userManager.Users.SingleOrDefault(x => string.Equals(x.Email, dto.Email, StringComparison.OrdinalIgnoreCase));

                return GenerateToken(user);
            }

            return BadRequest("Unable to sign up");
        }

        ActionResult GenerateToken(UserEntity user)
        {
            var accessToken = GenerateJwtToken(user);

            return Json(new AccessTokenDto
            {
                AccessToken = accessToken,
                TokenType = JwtBearerDefaults.AuthenticationScheme,
                ExpiresIn = jwtOptions.Value.ExpiresInSecond
            });
        }

        string GenerateJwtToken(UserEntity user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddSeconds(jwtOptions.Value.ExpiresInSecond);

            var token = new JwtSecurityToken(
                jwtOptions.Value.Issuer,
                jwtOptions.Value.Issuer,
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
