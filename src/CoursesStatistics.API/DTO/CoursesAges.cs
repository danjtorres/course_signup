using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesStatistics.API.DTO
{
    public class CoursesAges
    {
        public Guid CourseId { get; set; }
        public string CourseName { get; set; }
        public int MinimumAge { get; set; }
        public int MaximumAge { get; set; }
        public int AverageAge { get; set; }

    }
}
