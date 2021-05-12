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
    public class UserController : ApplicationController
    {

        private readonly UserService userService;

        public UserController(UserService userService) : base()
        {
            this.userService = userService;
        }


        [HttpGet("Get/{nameFilter}")]
        public async ValueTask<ActionResult> Get(string nameFilter = "") => await
        TryCatchAsync(async () =>
        {
            var users = await this.userService.RetrieveAllUsersAsync(nameFilter);
            return Ok(users);
        });



    }
}
