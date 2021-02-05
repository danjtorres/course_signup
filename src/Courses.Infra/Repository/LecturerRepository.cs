using Courses.Domain.Data;
using Courses.Domain.Lecturer;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Courses.Infra.Repository
{

    public class LecturerRepository : ILecturerRepository
    {
        private readonly LecturerContext _context;

        public LecturerRepository(LecturerContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public void Create(Lecturer lecturer)
        {
            _context.Lecturer.Add(lecturer);
        }

        public DbConnection ObterConexao() => _context.Database.GetDbConnection();
    }
}
