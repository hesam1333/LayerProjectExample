using System;
using System.Collections.Generic;
using System.Text;

namespace Evaluation.Services.DTO.Evaluation
{
    public class UserRolesDto
    {
        public int UserId { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
