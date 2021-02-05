using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesSignUp.API.Commands.Responses
{
    public class CreateCourseResponse
    {
        public Guid CourseId { get; set; }

        //validation
        public ValidationResult ValidationResult { get; set; }
    }
}
