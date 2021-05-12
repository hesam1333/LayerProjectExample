using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Evaluation.Brokers.Context;
using Evaluation.Brokers.Logging;
using Evaluation.Domain;
using Evaluation.Domain.Exceptions;
using Evaluation.Services;
using Evaluation.Services.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Evaluation.Brokers.Repositories;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Evaluation.WebAPI.Models;
using Evaluation.Services.DTO.Evaluation;

namespace Evaluation.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ApplicationController
    {

        private readonly UserService userService;
        private readonly IConfiguration config;

        public AuthController(UserService userService, IConfiguration config) : base()
        {
            this.config = config;
            this.userService = userService;
        }


        [HttpPost("register")]
        [AllowAnonymous()]
        public async ValueTask<ActionResult> registerAsync([FromBody] SignUpSingInDto data) => await
        TryCatchAsync(async () =>
        {
            UserRolesDto userClaims = await userService.SignUpInvitedEvaluatorAsync(data);

            var jwtClaims = ConvertToJwtClaim(userClaims);

            var token = GenerateToken(jwtClaims);

            var result = new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
                Roles = userClaims.Roles
            };

            return Ok(result);

        });


        [HttpPost("login")]
        [AllowAnonymous()]
        public async ValueTask<ActionResult> LoginAsync([FromBody] SignUpSingInDto data) => await
        TryCatchAsync(async () =>
        {
            UserRolesDto userClaims = await userService.SignInUserAsync(data);

            var jwtClaims = ConvertToJwtClaim(userClaims);

            var token = GenerateToken(jwtClaims);

            var result = new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
                Roles = userClaims.Roles
            };

            return Ok(result);

        });


        private JwtSecurityToken GenerateToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:SecurityKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                config["Jwt:Issuer"],
                config["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return token;
        }


        private  List<Claim> ConvertToJwtClaim(UserRolesDto claims)
        {
            List<Claim> JwtClaims = claims.Roles.Select(r => new Claim(ClaimTypes.Role, r)).ToList();

            JwtClaims.Add(new Claim(ClaimTypes.NameIdentifier, claims.UserId.ToString()));

            return JwtClaims;
        }


    }
}
