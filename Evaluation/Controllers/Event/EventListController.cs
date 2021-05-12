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
    public class EventListController : ApplicationController
    {

        private readonly EventListService eventListService;

        public EventListController(EventListService eventListService) : base()
        {
            this.eventListService = eventListService;
        }


        [HttpGet("Get")]
        public async ValueTask<ActionResult> GetEventlistAsync([FromQuery] SortAndPagingParameters parameters) => await
        TryCatchAsync(async () =>
        {
            var chartInfo = await this.eventListService.EventlistAsync(parameters);

            return Ok(chartInfo);
        });


        [HttpGet("OnGoingEvents")]
        public async ValueTask<ActionResult> GetOnGoingEventListAsync() => await
        TryCatchAsync(async () =>
        {
            var userNotEvaluatedDtos = await this.eventListService.OnGoingEventListAsync();

            return Ok(userNotEvaluatedDtos);
        });


    }
}
