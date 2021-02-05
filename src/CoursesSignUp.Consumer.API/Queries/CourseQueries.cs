using Courses.Domain.Course;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesSignUp.Consumer.API.Queries
{
    public class CourseQueries : ICourseQueries
    {
        private readonly ICourseRepository _courseRepository;

        public CourseQueries(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<int> CourseCapacity(Guid courseId)
        {

            const string sql = @"SELECT distinct top 1
                                C.Capacity as Capacity
                                from [dbo].[Course] C
                                WHERE C.Id = @courseId";

            var conection = _courseRepository.ObterConexao();

            var result = (await conection.QueryAsync<dynamic>(sql, new { courseId })).SingleOrDefault();

            return result != null ? (Int32)result.Capacity : 0;

        }
    }
}
