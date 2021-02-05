using CoursesSignUp.API.Commands.Responses;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesSignUp.API.Commands.Requests
{
    public class CreateCourseRequest : IRequest<CreateCourseResponse>
    {
        //Course
        public string Name { get; set; }
        public int Capacity { get; set; }
        public int NumberOfStudents { get; set; }
        public Guid LecturerId { get; set; }

        //validation
        public ValidationResult ValidationResult { get; set; }

        public bool IsValid()
        {
            ValidationResult = new CreateCourseValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class CreateCourseValidation : AbstractValidator<CreateCourseRequest>
        {
            public CreateCourseValidation()
            {
                RuleFor(c => c.Name)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("Course name is necessary.");

                RuleFor(c => c.Capacity)
                    .GreaterThan(0)
                    .WithMessage("Capacity must be greater than zero.");

                RuleFor(c => c.NumberOfStudents)
                    .GreaterThanOrEqualTo(0)
                    .WithMessage("Number of students must be zero or greater.");

                RuleFor(c => c.LecturerId)
                    .NotEmpty()
                    .WithMessage("Course Lecturer is necessary.");

                //NextStep
                //TODO: Course Id des not exists.
                //TODO: Validate Lecturer Id exists.
                //TODO: Max width of fields.

            }
        }

    }
}
