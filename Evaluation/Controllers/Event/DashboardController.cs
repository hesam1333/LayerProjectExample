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
    public class DashboardController : ApplicationController
    {

        private readonly DashboardService dashboardService;

        public DashboardController(DashboardService dashboardService) : base()
        {
            this.dashboardService = dashboardService;
        }


        [HttpGet("GetChart")]
        public async ValueTask<ActionResult> GetChartAsync() => await
        TryCatchAsync(async () =>
        {
            var chartInfo = await this.dashboardService.DashBoardChartValuesAsync();

            return Ok(chartInfo);
        });


        [HttpGet("searchEvents/{searchString}")]
        public async ValueTask<ActionResult> SearchEvents(string searchString) => await
        TryCatchAsync(async () =>
        {
            var Events = await this.dashboardService.SearchEventsAsync(searchString);

            return Ok(Events);
        });


        [HttpGet("GetNotEvaluated")]
        public async ValueTask<ActionResult> GetUsersNotEvaluated() => await
        TryCatchAsync(async () =>
        {
            var userNotEvaluatedDtos = await this.dashboardService.GetUsersNotEvaluatedAsync();

            return Ok(userNotEvaluatedDtos);
        });


        [HttpGet("GetTopStars")]
        public async ValueTask<ActionResult> GetTopStars() => await
        TryCatchAsync(async () =>
        {
            var topStarUsersDtos = await this.dashboardService.GetTopStarAsyncs();

            return Ok(topStarUsersDtos);
        });

    }
}
