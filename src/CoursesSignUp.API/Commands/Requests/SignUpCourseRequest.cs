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
    public class SignUpCourseRequest : IRequest<SignUpCourseResponse>
    {
        //Sign Up
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Guid CourseId { get; set; }

        //validation
        public ValidationResult ValidationResult { get; set; }

        public bool IsValid()
        {
            ValidationResult = new SignUpCourseValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class SignUpCourseValidation : AbstractValidator<SignUpCourseRequest>
        {
            public SignUpCourseValidation()
            {
                RuleFor(c => c.Name)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("Student name is necessary.");

                RuleFor(c => c.Email).EmailAddress();

                RuleFor(c => c.DateOfBirth)
                    .NotNull()
                    .NotEmpty()
                    .Must(BeAValidDate).WithMessage("Date of birth is necessary.");

                RuleFor(c => c.CourseId)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("Course Lecturer is necessary.");

                //NextStep
                //TODO: Student already signed up.
                //TODO: Max width of fields.
                //TODO: Course ID exists.

            }
            private bool BeAValidDate(DateTime date)
            {
                return !date.Equals(default(DateTime));
            }
        }

    }
}
