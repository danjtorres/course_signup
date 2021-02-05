using Courses.Domain.Course;
using Courses.Domain.SignUp;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesSignUp.Consumer.API.Queries
{
    public class SignUpQueries : ISignUpQueries
    {
        private readonly ISignUpRepository _signUpRepository;

        public SignUpQueries(ISignUpRepository signUpRepository)
        {
            _signUpRepository = signUpRepository;
        }

        public async Task<int> NumberOfStudents(Guid courseId)
        {

            const string sql = @"SELECT
                                Count(S.Id) as NumberOfStudents
                                from [dbo].[SignUpCourse] S
                                WHERE S.CourseId = @courseId";

            var conection = _signUpRepository.ObterConexao();
            var result = (await conection.QueryAsync<dynamic>(sql, new { courseId })).SingleOrDefault();

            return result != null ? (Int32)result.NumberOfStudents : 0;

        }

        public async Task<bool> VerifyStudentNotAppliedYet(Guid courseId, string email)
        {
            const string sql = @"SELECT distinct
                                Count(S.Id) as StudentApplyed
                                from [dbo].[SignUpCourse] S
                                WHERE S.Email = @email";

            var conection = _signUpRepository.ObterConexao();
            var result = (await conection.QueryAsync<dynamic>(sql, new { email })).SingleOrDefault();

            return (result != null && (Int32)result.StudentApplyed == 0) ? true : false;
        }
    }
}
