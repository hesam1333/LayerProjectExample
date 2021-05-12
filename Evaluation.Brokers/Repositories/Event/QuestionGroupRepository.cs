
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

    public class QuestionGroupRepository : GenericRepository<QuestionGroup>, IQuestionGroupRepository
    {
        private readonly EvaluationContext context;

        public QuestionGroupRepository(EvaluationContext context) : base(context)
        {
            this.context = context;
        }


        public async ValueTask<List<QuestionGroupViewTable>> GetQuestionGroupViewTablesForEvent(int eventId)
        {

            var eventQuestions = await context.EventQuestions.Where(i => i.EventId == eventId)
                .Select(i => i.QuestionId).ToListAsync();

            var model = await context.QuestionGroups.Where(i => !i.IsDelete)
                .Select(i => new QuestionGroupViewTable()
                {
                    QuestionGroupId = i.Id,
                    QuestionGroupTitle = i.GroupTitle,
                    Questions = i.Questions.Where(i => !i.IsDelete).Select(j => new QuestiontDto()
                    {
                        IsDescriptive = j.IsDescriptive ,
                        QuestionGroupId = i.Id ,
                        QuestionId = j.Id ,
                        QuestionTitle = j.QuestionTitle,
                        IsInEvent = eventQuestions.Contains(j.Id)
                    })
                }).ToListAsync();

            return model;
        }


    }
}