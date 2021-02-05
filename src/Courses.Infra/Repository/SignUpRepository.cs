using Courses.Domain.Data;
using Courses.Domain.SignUp;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Courses.Infra.Repository
{
    public class SignUpRepository : ISignUpRepository
    {
        private readonly SignUpContext _context;

        public SignUpRepository(SignUpContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public void Create(SignUpCourse signUpCourse)
        {
            _context.SignUpCourse.Add(signUpCourse);
        }

        public DbConnection ObterConexao() => _context.Database.GetDbConnection();
    }
}
