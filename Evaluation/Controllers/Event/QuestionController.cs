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
    public class QuestionController : ApplicationController
    {

        private readonly QuestionService questionService;

        public QuestionController(QuestionService questionService) : base()
        {
            this.questionService = questionService;
        }


        [HttpPost("Add")]
        public async ValueTask<ActionResult> Add([FromBody] QuestiontDto parameters) => await
        TryCatchAsync(async () =>
        {
            var question = await this.questionService.CreatQuestionAsync(parameters);

            return Ok(question);
        });


        public class EditParas
        {
            public int guestionId { get; set; }
            public string  newTitle { get; set; }
        }

        [HttpPut("edit")]
        public async ValueTask<ActionResult> edit([FromBody] EditParas editParas) => await
        TryCatchAsync(async () =>
        {
            var isDone = await this.questionService.EditQuestionAsync(editParas.guestionId, editParas.newTitle);

            return Ok(isDone);
        });



        [HttpDelete("delete/{questionId}")]
        public async ValueTask<ActionResult> delete( int questionId) => await
        TryCatchAsync(async () =>
        {
            var isDone = await this.questionService.RemoveQuestionAsync(questionId);

            return Ok(isDone);
        });

    }
}
