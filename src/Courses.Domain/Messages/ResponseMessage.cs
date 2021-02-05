using System;
using FluentValidation.Results;

namespace Courses.Domain.Messages
{
    public class ResponseMessage
    {
        public ValidationResult ValidationResult { get; set; }

        public ResponseMessage(ValidationResult validationResult)
        {
            ValidationResult = validationResult;
        }
    }
}
