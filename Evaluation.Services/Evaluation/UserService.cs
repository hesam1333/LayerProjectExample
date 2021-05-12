using Common.Utilities;
using Evaluation.Brokers.Context;
using Evaluation.Brokers.Logging;
using Evaluation.Brokers.Repositories;
using Evaluation.Brokers.Repositories.Interfaces;
using Evaluation.Domain;
using Evaluation.Domain.Exceptions;
using Evaluation.Services.DTO;
using Evaluation.Services.DTO.Evaluation;
using Evaluation.Services.Validations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.Services
{
    public class UserService : AplicationService
    {
        private readonly ILoggingBroker loggingBroker;

        private readonly IUserRepository userRepository;

        private readonly UserValidator userValidator;
        private readonly SignUpSIngInDtoValidator signUpSIngInDtoValidator;

        public UserService(ILoggingBroker loggingBroker,
                           IUserRepository userRepository)
                           : base(loggingBroker)
        {
            this.loggingBroker = loggingBroker;

            this.userRepository = userRepository;

            this.userValidator = new UserValidator(userRepository);
            this.signUpSIngInDtoValidator = new SignUpSIngInDtoValidator();
        }


        public async ValueTask<List<User>> RetrieveAllUsersAsync(string nameFilter) => await
        TryCatchAsync(async () =>
        {
            List<User> Users = await this.userRepository.FindAsync(i => i.SureName.Contains(nameFilter));
            return Users;
        });


        public async ValueTask<UserRolesDto> SignUpInvitedEvaluatorAsync(SignUpSingInDto input) => await
        TryCatchAsync(async () =>
        {

            await signUpSIngInDtoValidator.ValidateEntityAsync(input);

            User invitedUser = (await userRepository.FindAsync(i => i.Email == input.Email)).FirstOrDefault();

            if (invitedUser == null)
            {
                throw new InvalidException(Message: "There are no invitations send to this email address " +
                    "for start evaluation . contact system admin to resive invitation");
            }

            if (!string.IsNullOrEmpty(invitedUser.Password) || !invitedUser.IsFirstTime)
            {
                throw new InvalidException(Message: "This email address alredy signed up . " +
                    "login to start your evaluation");
            }


            invitedUser.IsFirstTime = false;
            invitedUser.Password = Crypto.EncryptStringAES(input.Password, ConstantVaribels.HashKey);

            User user = await this.userRepository.EditAsync(invitedUser);

            var userRoles = GetUserRoles(user);

            return userRoles;
        });


        public async ValueTask<UserRolesDto> SignInUserAsync(SignUpSingInDto input) => await
        TryCatchAsync(async () =>
        {

            User maybeLoginUser = (await userRepository.FindAsync(i => i.Email == input.Email)).FirstOrDefault();

            if(maybeLoginUser == null)
            {
                throw new InvalidException(Message: "Could not find a user with this Email Address");
            }

            var DecryptPass = Crypto.DecryptStringAES(maybeLoginUser.Password, ConstantVaribels.HashKey);

            if (maybeLoginUser.Email != input.Email || DecryptPass != input.Password)
            {
                throw new InvalidException(Message: "Email or password is wrong . try again ");
            }

            var userRoles = GetUserRoles(maybeLoginUser);

            return userRoles;
        });


        public UserRolesDto GetUserRoles(User user)
        {
            var userClaims = new UserRolesDto() { UserId = user.Id };
            if (!string.IsNullOrEmpty(user.Password) || !string.IsNullOrEmpty(user.Email))
            {
                userClaims.Roles.Add("evaluator");
            }
            if (user.IsAdmin)
            {
                userClaims.Roles.Add("admin");
            }

            return userClaims;

        }

    }
}
