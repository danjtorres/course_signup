using Courses.Domain.Course;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using CoursesStatistics.API.DTO;

namespace CoursesStatistics.API.Queries
{
    public class CoursesStatisticsQueries : ICoursesStatisticsQueries
    {

        private readonly ICourseRepository _courseRepository;

        public CoursesStatisticsQueries(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<List<CoursesAges>> CoursesAges()
        {
            const string sql = @"select 
	                                SC.CourseId as CourseId,
	                                C.[Name] as CourseName,
	                                Min(Cast(FLOOR(DATEDIFF(DAY, SC.DateOfBirth, GETDATE()) / 365.25)as int)) as MinimumAge,
	                                Max(Cast(FLOOR(DATEDIFF(DAY, SC.DateOfBirth, GETDATE()) / 365.25)as int)) as MaximumAge,
	                                Avg(Cast(FLOOR(DATEDIFF(DAY, SC.DateOfBirth, GETDATE()) / 365.25)as int)) as AverageAge
                                from [dbo].[SignUpCourse] SC 
	                                inner join [dbo].[Course] C on SC.CourseId = C.Id
                                group by 
	                                SC.CourseId, C.Name";

            var conection = _courseRepository.ObterConexao();

            var result = (await conection.QueryAsync<dynamic>(sql)).ToList();

            var courses = MapCoursesAges(result);

            return courses;

        }

        private List<CoursesAges> MapCoursesAges(List<dynamic> result)
        {
            List<CoursesAges> courses = new List<CoursesAges>();
            
            foreach (var item in result)
            {
                CoursesAges itemDto = new CoursesAges();

                itemDto.CourseId = item.CourseId;
                itemDto.CourseName = item.CourseName;
                itemDto.MinimumAge = (Int32)item.MinimumAge;
                itemDto.MaximumAge = (Int32)item.MaximumAge;
                itemDto.AverageAge = (Int32)item.AverageAge;

                courses.Add(itemDto);
            }

            return courses;
        }
    }
}
