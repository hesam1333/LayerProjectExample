
using Evaluation.Domain;
using Evaluation.Brokers.Context;
using Evaluation.Brokers.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.Brokers.Repositories
{

    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly EvaluationContext context;

        public UserRepository(EvaluationContext context) : base(context)
        {
            this.context = context ;
        }

        public async Task<bool> IsEmailUnicAsync(string email , int Id)
        {

            var result = await context.Users.AnyAsync(i => i.Email == email && !string.IsNullOrEmpty(i.Email) && i.Id != Id);

            return !result;
        }

        
    }
}