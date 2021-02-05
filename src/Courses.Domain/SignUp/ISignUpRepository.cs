using Courses.Domain.Data;
using System.Data.Common;

namespace Courses.Domain.SignUp
{
    public interface ISignUpRepository : IRepository<SignUpCourse>
    {
        void Create(SignUpCourse signUpCourse);
        DbConnection ObterConexao();
    }

}
