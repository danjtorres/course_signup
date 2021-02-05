using Courses.Domain.Course;
using Courses.Domain.Data;
using Courses.Domain.Messages;
using Courses.Domain.SignUp;
using Courses.MessageBus;
using CoursesSignUp.API.Commands.Requests;
using CoursesSignUp.API.Commands.Responses;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoursesSignUp.API.Commands.Handlers
{
    public class SignUpCourseHandler : IRequestHandler<SignUpCourseRequest, SignUpCourseResponse>
    {
        private readonly IMessageBus _bus;

        public SignUpCourseHandler(IMessageBus bus)
        {
            _bus = bus;
        }

        public async Task<SignUpCourseResponse> Handle(SignUpCourseRequest request, CancellationToken cancellationToken)
        {
            var result = new SignUpCourseResponse();

            // Validation
            if (!request.IsValid())
            {
                result.ValidationResult = request.ValidationResult;
                return await Task.FromResult(result);
            }

            //Send event to a service Bus
            var message = new SignUpIntegrationEvent(request.Name, request.Email, request.DateOfBirth, request.CourseId);
            await _bus.PublishAsync<SignUpIntegrationEvent>(message);

            return await Task.FromResult(result);

        }
        protected async Task<ValidationResult> PersistData(IUnitOfWork unitOfWork)
        {
            ValidationResult validationResult = new ValidationResult();

            if (!await unitOfWork.Commit())
                validationResult.Errors.Add(new ValidationFailure(string.Empty, "Houve um erro ao persistir os dados"));

            return validationResult;
        }

    }
}
