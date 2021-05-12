using System;
using System.Collections.Generic;
using System.Text;

namespace Evaluation.Services
{
    public interface IUserClaims
    {
        bool HaseClaim(string Claim);
        bool HaseClaimAndThrow(string Claim);
        int GetUserId();

    }
}
