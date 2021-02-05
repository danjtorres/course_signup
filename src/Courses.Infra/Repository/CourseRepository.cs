using Courses.Domain.Course;
using Courses.Domain.Data;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Courses.Infra.Repository
{
    public class CourseRepository : ICourseRepository
    {
        private readonly CourseContext _context;

        public CourseRepository(CourseContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public void Create(Course course)
        {
            _context.Course.Add(course);
        }

        public DbConnection ObterConexao() => _context.Database.GetDbConnection();
    }
}
