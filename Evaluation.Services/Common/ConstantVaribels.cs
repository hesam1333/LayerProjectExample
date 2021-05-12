using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Evaluation.Domain;
using Evaluation.Domain.Exceptions;
using Evaluation.Brokers.Context;
using Evaluation.Brokers.Logging;

namespace Evaluation.Services
{
    public partial class ConstantVaribels 
    {
        public static string HashKey = "123@fsf@!%!@%!@#!@#fiohwebrwuerwpoer hrbgkawjeoi haweoiu234234";

        public enum ServiceUserRoles
        {
            admin ,
            evaluator ,
            evaluatee
            
        }

        public enum EventStatus
        {
            notStarted,
            finished,
            onGoing

        }
    }
}
