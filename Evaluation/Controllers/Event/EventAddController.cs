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
    [Authorize(Roles ="admin")]
    public class EventAddController : ApplicationController
    {

        private readonly EventAddService eventAddService;

        public EventAddController(EventAddService eventAddService) : base()
        {
            this.eventAddService = eventAddService;
        }


        [HttpPost("Stage1")]
        public async ValueTask<ActionResult> EventAddStage1([FromBody] EventAddStage1Dto parameters) => await
        TryCatchAsync(async () =>
        {
            var @event = await this.eventAddService.EventStage1AddAsync(parameters);

            return Ok(@event);
        });


        [HttpPost("Stage2")]
        public async ValueTask<ActionResult> EventAddStage2([FromBody] EventAddStage2Dto parameters) => await
        TryCatchAsync(async () =>
        {
            var newEventQuestionIdis = await this.eventAddService.EventStage2AddAsync(parameters);

            return Ok(newEventQuestionIdis);
        });


        [HttpPost("Stage3")]
        public async ValueTask<ActionResult> EventAddStage3([FromBody] EventAddStage3Dto parameters) => await
        TryCatchAsync(async () =>
        {
            var @event = await this.eventAddService.EventStage3AddAsync(parameters);

            return Ok(@event);
        });
    }
}
