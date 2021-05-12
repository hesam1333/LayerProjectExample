using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Evaluation.Brokers.Context;
using Evaluation.Brokers.Logging;
using Evaluation.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Evaluation.WebAPI.Controllers
{
    public class ApplicationController : ControllerBase
    {
        protected delegate ValueTask<ActionResult> ActionFunctionAsync();
        protected delegate ActionResult ActionFunction();

        protected async ValueTask<ActionResult>  TryCatchAsync(ActionFunctionAsync actionFunctionAsync)
        {
            try
            {
                return await actionFunctionAsync();
            }
            catch (ValidationException validationException)
            {
                return Conflict(validationException.Message);
            }
            catch (ModelValidationException modelValidationException)
            {
                return BadRequest(modelValidationException.validationError);
            }
            catch (DependencyException dependencyException)
            {
                return Problem(dependencyException.Message);
            }
            catch (ServiceException serviceException)
            {
                return Problem(title:serviceException.Message , detail: serviceException.ToString());
            }
            catch (NotAuthorizedException notAuthorizedException)
            {
                return BadRequest(notAuthorizedException.Message);
            }
            catch (Exception e)
            {
                return Problem(title:"Fatal error! try again later" , detail:e.Message);
            }
        }

        protected  ActionResult TryCatch(ActionFunction actionFunction)
        {
            try
            {
                return actionFunction();
            }
            catch (ValidationException validationException)
            {
                return Conflict(validationException.Message);
            }
            catch (ModelValidationException modelValidationException)
            {
                return BadRequest(modelValidationException.validationError);
            }
            catch (DependencyException dependencyException)
            {
                return Problem(dependencyException.Message);
            }
            catch (ServiceException serviceException)
            {
                return Problem(title: serviceException.Message, detail: serviceException.ToString());
            }
            catch (NotAuthorizedException notAuthorizedException)
            {
                return BadRequest(notAuthorizedException.Message);
            }
            catch (Exception e)
            {
                return Problem(title: "Fatal error! try again later", detail:e.Message);
            }
        }
    }
}
