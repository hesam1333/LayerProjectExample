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
    [Authorize(Roles = "admin")]
    public class QuestionGroupController : ApplicationController
    {

        private readonly QuestionGroupService questionGroupService;

        public QuestionGroupController(QuestionGroupService questionGroupService) : base()
        {
            this.questionGroupService = questionGroupService;
        }


        [HttpGet("Get/{eventId}")]
        public async ValueTask<ActionResult> Add( int eventId) => await
        TryCatchAsync(async () =>
        {
            var questionGroups = await this.questionGroupService.GetQuestionGroupsForEditEvent(eventId);

            return Ok(questionGroups);
        });

        public class AddParas
        {
            public string Title { get; set; }
        }

        [HttpPost("Add")]
        public async ValueTask<ActionResult> Add([FromBody] AddParas paras) => await
        TryCatchAsync(async () =>
        {
            var questionGroup = await this.questionGroupService.CreatQuestionGroupAsync(paras.Title);

            return Ok(questionGroup);
        });


        public class EditParas
        {
            public int guestionGroupId { get; set; }
            public string newTitle { get; set; }
        }

        [HttpPut("edit")]
        public async ValueTask<ActionResult> edit([FromBody] EditParas paras) => await
        TryCatchAsync(async () =>
        {
            var isDone = await this.questionGroupService.EditQuestionGroupAsync(paras.guestionGroupId, paras.newTitle);

            return Ok(isDone);
        });



        [HttpDelete("delete/{questionGroupId}")]
        public async ValueTask<ActionResult> edit(int questionGroupId) => await
        TryCatchAsync(async () =>
        {
            var isDone = await this.questionGroupService.RemoveQuestionGroupAsync(questionGroupId);

            return Ok(isDone);
        });

    }
}
