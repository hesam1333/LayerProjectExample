using Evaluation.Domain.Exceptions;
using Evaluation.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Evaluation.WebAPI.Models
{
    public class WebApiUserClaims : IUserClaims
    {
        public int UserId { get; set; }
        public List<string> Roles { get; set; } = new List<string>();

        public WebApiUserClaims(IHttpContextAccessor httpContextAccessor)
        {
            try
            {
                var httpContext = httpContextAccessor.HttpContext;
                var user = httpContext.User;

                var claimsIdentity = user.Identity as ClaimsIdentity;
                var IsAuthenticated = user.Identity.IsAuthenticated;

                if (claimsIdentity != null)
                {
                    Roles = claimsIdentity.Claims.Where(i => i.Type == ClaimTypes.Role).Select(i => i.Value).ToList();
                    UserId = Int32.Parse(claimsIdentity.FindFirst(i => i.Type == ClaimTypes.NameIdentifier)?.Value ?? "-1" );
                }
            }
            catch (Exception ex)
            {
                UserId = -1;
            }

        }

        public bool HaseClaim(string Claim)
        {
            return Roles.Contains(Claim);
        }

        public bool HaseClaimAndThrow(string Claim)
        {
            if (Roles.Contains(Claim))
            {
                return true;
            }
            else
            {
                throw new NotAuthorizedException();
            }
        }

        public int GetUserId()
        {
            return UserId;
        }
    }
}
