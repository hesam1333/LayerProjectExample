using Evaluation.Brokers.Context;
using Evaluation.Brokers.Repositories.Interfaces;
using Evaluation.Domain;
using Evaluation.Domain.Exceptions;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Evaluation.Services.Validations
{
    public class UserValidator : Genericvalidator<User>
    {

        private readonly IUserRepository userRepository;

        public UserValidator(IUserRepository UserRepository)
        {
            this.userRepository = UserRepository;

            RuleFor(item => item).NotNull();
            RuleFor(item => item).MustAsync(async (item, cancellation) => await IsEmailUnicAsync(item.Email, item.Id))
                .WithMessage("Email must be unic").WithName("Email");

            RuleFor(item => item.Email).EmailAddress().WithMessage("Email is not correct");
            RuleFor(item => item.Email).MaximumLength(80);

            RuleFor(item => item.Password).MaximumLength(80);


            RuleFor(item => item).Must((item, cancellation) => IsPassOk(item))
                .WithMessage("Minimum eight characters, at least one letter," +
                " one number and" +
                " one special '@$!%*#?' character").WithName("evaluatorDtos");

            RuleFor(item => item.Position).MaximumLength(80);
            RuleFor(item => item.Position).NotEmpty();



            RuleFor(item => item.SureName).MaximumLength(80);
            RuleFor(item => item.SureName).NotEmpty();



        }


        private bool IsPassOk(User user)
        {

            if (user.Id != 0 || String.IsNullOrEmpty(user.Password)  ) // password is hashed or not exist
            {
                return true;
            }
            else
            {
                return Regex.Match(user.Password, @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$").Success;
            }
        }

        private async Task<bool> IsEmailUnicAsync(string email, int id)
        {
            var result = await userRepository.IsEmailUnicAsync(email, id);
            return result;
        }
    }
}
