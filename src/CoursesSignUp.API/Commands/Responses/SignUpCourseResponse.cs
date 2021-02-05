using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesSignUp.API.Commands.Responses
{
    public class SignUpCourseResponse
    {
        //validation
        public ValidationResult ValidationResult { get; set; }
    }
}
