using Common.Utilities;
using Evaluation.Brokers.Context;
using Evaluation.Brokers.Logging;
using Evaluation.Brokers.Repositories;
using Evaluation.Brokers.Repositories.Interfaces;
using Evaluation.Domain;
using Evaluation.Domain.Exceptions;
using Evaluation.Services.DTO;
using Evaluation.Services.Validations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.Services
{
    public class QuestionService : AplicationService
    {
        private readonly ILoggingBroker loggingBroker;

        private readonly IQuestionRepository questionRepository;

        private readonly IUserClaims userClaims;
        private readonly QuestionValidator quesionGroupValidator;
        public QuestionService(ILoggingBroker loggingBroker,
                                IQuestionRepository questionRepository,
                                IUserClaims userClaims)
                                : base(loggingBroker)
        {

            this.loggingBroker = loggingBroker;
            this.userClaims = userClaims;

            this.questionRepository = questionRepository;

            this.quesionGroupValidator = new QuestionValidator();
        }



        public async ValueTask<Question> CreatQuestionAsync(QuestiontDto questiontDto)
        => await TryCatchAsync(async () =>
        {

            userClaims.HaseClaimAndThrow(ConstantVaribels.ServiceUserRoles.admin.ToString());

            var question = new Question()
            {
                QuestionTitle = questiontDto.QuestionTitle,
                IsDescriptive = questiontDto.IsDescriptive ,
                QuestionGroupId = questiontDto.QuestionGroupId,
                IsDelete = false,
            };

            quesionGroupValidator.ValidateEntity(question);

            Question result = await questionRepository.AddAsync(question);

            return result;
        });


        public async ValueTask<bool> RemoveQuestionAsync(int guestionId)
        => await TryCatchAsync(async () =>
        {

            userClaims.HaseClaimAndThrow(ConstantVaribels.ServiceUserRoles.admin.ToString());

            var question = await questionRepository.GetAsync(guestionId);

            question.IsDelete = true;

            await questionRepository.EditAsync(question);


            return true;
        });

        public async ValueTask<bool> EditQuestionAsync(int guestionId , string newTitle)
        => await TryCatchAsync(async () =>
        {

            userClaims.HaseClaimAndThrow(ConstantVaribels.ServiceUserRoles.admin.ToString());

            var question = await questionRepository.GetAsync(guestionId);

            question.QuestionTitle = newTitle;

            await questionRepository.EditAsync(question);

            quesionGroupValidator.ValidateEntity(question);

            return true;
        });

    }
}
