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
    public class QuestionGroupService : AplicationService
    {
        private readonly ILoggingBroker loggingBroker;

        private readonly IQuestionGroupRepository questionGroupRepository;

        private readonly IUserClaims userClaims;
        private readonly QuestionGroupValidator quesionGroupValidator;
        public QuestionGroupService(ILoggingBroker loggingBroker,
                                IQuestionGroupRepository questionGroupRepository,
                                IUserClaims userClaims)
                                : base(loggingBroker)
        {

            this.loggingBroker = loggingBroker;
            this.userClaims = userClaims;
            this.questionGroupRepository = questionGroupRepository;

            this.quesionGroupValidator = new QuestionGroupValidator();
        }

        public async ValueTask<List<QuestionGroupViewTable>> GetQuestionGroupsForEditEvent(int eventId)
        => await TryCatchAsync(async () =>
        {

            userClaims.HaseClaimAndThrow(ConstantVaribels.ServiceUserRoles.admin.ToString());


            var questionGroupViewTables = 
                await questionGroupRepository.GetQuestionGroupViewTablesForEvent(eventId);

            return questionGroupViewTables;
        });


        public async ValueTask<QuestionGroup> CreatQuestionGroupAsync(string title)
        => await TryCatchAsync(async () =>
        {

            userClaims.HaseClaimAndThrow(ConstantVaribels.ServiceUserRoles.admin.ToString());

            var questionGroup = new QuestionGroup()
            {
                GroupTitle = title,
                IsDelete = false,
            };


            quesionGroupValidator.ValidateEntity(questionGroup);

            QuestionGroup result = await questionGroupRepository.AddAsync(questionGroup);

            return result;
        });


        public async ValueTask<bool> RemoveQuestionGroupAsync(int guestionGroupId)
        => await TryCatchAsync(async () =>
        {

            userClaims.HaseClaimAndThrow(ConstantVaribels.ServiceUserRoles.admin.ToString());

            var questionGroup = await questionGroupRepository.GetAsync(guestionGroupId);

            questionGroup.IsDelete = true;

            await questionGroupRepository.EditAsync(questionGroup);


            return true;
        });

        public async ValueTask<bool> EditQuestionGroupAsync(int guestionGroupId , string newTitle)
        => await TryCatchAsync(async () =>
        {

            userClaims.HaseClaimAndThrow(ConstantVaribels.ServiceUserRoles.admin.ToString());

            var questionGroup = await questionGroupRepository.GetAsync(guestionGroupId);

            questionGroup.GroupTitle = newTitle;


            quesionGroupValidator.ValidateEntity(questionGroup);

            await questionGroupRepository.EditAsync(questionGroup);


            return true;
        });

    }
}
