using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Domain.SignUp
{
    public class SignUpCourse : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Guid CourseId { get; set; }
    }
}
