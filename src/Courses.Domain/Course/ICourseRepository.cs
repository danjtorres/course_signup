using Courses.Domain.Data;
using System.Data.Common;

namespace Courses.Domain.Course
{
    public interface ICourseRepository : IRepository<Course>
    {
        void Create(Course course);

        DbConnection ObterConexao();
    }
}
