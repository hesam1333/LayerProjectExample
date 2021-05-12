using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Evaluation.Brokers.Context;
using Evaluation.Brokers.Logging;
using Evaluation.Domain;
using Evaluation.Domain.Exceptions;
using Evaluation.Services;
using Evaluation.Services.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Evaluation.Brokers.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace Evaluation.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "evaluator")]
    public class EvaluateeController : ApplicationController
    {

        private readonly EvaluateeService evaluateeService;

        public EvaluateeController(EvaluateeService evaluateeService) : base()
        {
            this.evaluateeService = evaluateeService;
        }


        [HttpGet("GetEventGroups/{evaluateeId}")]
        public async ValueTask<ActionResult> GetEventGroups(int evaluateeId) => await
        TryCatchAsync(async () =>
        {
            var eventGroupsForEvaluation = await this.evaluateeService.GetEventGroupsForEvaluation(evaluateeId);

            return Ok(eventGroupsForEvaluation);
        });


        [HttpGet("QuestionsForGroup/{questionGroupId}")]
        public async ValueTask<ActionResult> QuestionsForGroup(int questionGroupId, [FromQuery] int evaluateeId) => await
        TryCatchAsync(async () =>
        {
            var questionsWhitAnswers = await this.evaluateeService.getEvaluateeQuestionsForGroup(evaluateeId, questionGroupId);

            return Ok(questionsWhitAnswers);
        });

        [HttpPost("AddAnswer")]
        public async ValueTask<ActionResult> AddAnswer([FromBody] EvaluateeQuestionAnswersDto paras) => await
        TryCatchAsync(async () =>
        {
            var countAdded = await this.evaluateeService.AddQuestionAnswersForEvaluatee(paras);

            return Ok(countAdded);
        });

    }
}
