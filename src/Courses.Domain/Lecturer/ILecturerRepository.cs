using Courses.Domain.Data;
using System.Data.Common;

namespace Courses.Domain.Lecturer
{

    public interface ILecturerRepository : IRepository<Lecturer>
    {
        void Create(Lecturer lecturer);

        DbConnection ObterConexao();
    }
}
