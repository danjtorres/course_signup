using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Domain.Data
{
    public interface IRepository<T>
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
