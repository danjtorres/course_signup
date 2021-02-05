using CoursesSignUp.API.Commands.Handlers;
using CoursesSignUp.API.Commands.Requests;
using CoursesSignUp.API.Commands.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesSignUp.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class LecturerController : ControllerBase
    {

        [HttpPost("CreateLecturer")]
        [Route("CreateLecturer")]
        public async Task<CreateLecturerResponse> CreateLecturer(
            [FromServices] IMediator mediator, 
            [FromBody] CreateLecturerRequest command)
        {
            return await mediator.Send(command);
        }

        //NextStep
        //TODO: Create other commands.

    }
}
