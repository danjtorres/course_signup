using System.Threading.Tasks;

namespace Courses.Domain.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
