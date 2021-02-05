using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Domain.Course
{
    public class Course : BaseEntity
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
        public int NumberOfStudents { get; set; }
        public Guid LecturerId { get; set; }

    }
}
