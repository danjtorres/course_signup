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
    public class CoursesController : ControllerBase
    {

        [HttpPost("CreateCourse")]
        [Route("CreateCourse")]
        public async Task<CreateCourseResponse> CreateCourse(
            [FromServices] IMediator mediator, 
            [FromBody] CreateCourseRequest command)
        {
            return await mediator.Send(command);
        }

        [HttpPost("SignUpCourse")]
        [Route("SignUpCourse")]
        public async Task<SignUpCourseResponse> SignUpCourse(
            [FromServices] IMediator mediator,
            [FromBody] SignUpCourseRequest command)
        {
            return await mediator.Send(command);
        }

        //NextStep
        //TODO: Create other commands.

    }
}
