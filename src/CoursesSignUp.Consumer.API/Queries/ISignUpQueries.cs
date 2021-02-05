using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesSignUp.Consumer.API.Queries
{
    public interface ISignUpQueries
    {
        Task<int> NumberOfStudents(Guid courseId);
        Task<bool> VerifyStudentNotAppliedYet(Guid courseId, string email);
    }
}
