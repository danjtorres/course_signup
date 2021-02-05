using CoursesStatistics.API.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesStatistics.API.Queries
{
    public interface ICoursesStatisticsQueries
    {
        Task<List<CoursesAges>> CoursesAges();
    }
}
