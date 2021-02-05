using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesSignUp.Consumer.API.Queries
{
    public interface ICourseQueries
    {
        Task<int> CourseCapacity(Guid courseId);
    }
}
