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
    public class EvaluatorController : ApplicationController
    {

        private readonly EvaluatorService evaluatorService;

        public EvaluatorController(EvaluatorService evaluatorService) : base()
        {
            this.evaluatorService = evaluatorService;
        }


        [HttpGet("OngoingEvents")]
        public async ValueTask<ActionResult> OngoingEvents() => await
        TryCatchAsync(async () =>
        {
            var onGoingEvents = await this.evaluatorService.RetrieveCurrentEvaluatorOngoingEvents();

            return Ok(onGoingEvents);
        });


        [HttpGet("evaluatees/{eventId}")]
        public async ValueTask<ActionResult> evaluatees(int eventId) => await
        TryCatchAsync(async () =>
        {
            var evaluateeProgresses = await this.evaluatorService.GetCurrentEvaluatorEvaluateesWhitProgres(eventId);

            return Ok(evaluateeProgresses);
        });

    }
}
