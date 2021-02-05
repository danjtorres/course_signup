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
    public class CreateLecturerRequest : IRequest<CreateLecturerResponse>
    {
        public string Name { get; set; }

        //validation
        public ValidationResult ValidationResult { get; set; }

        public bool IsValid()
        {
            ValidationResult = new CreateLecturerValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class CreateLecturerValidation : AbstractValidator<CreateLecturerRequest>
        {
            public CreateLecturerValidation()
            {
                RuleFor(c => c.Name)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("Lecturer name is necessary.");

                //NextStep
                //TODO: Lecturer Id does not exists.
                //TODO: Max width of fields.
            }
        }
    }
}
