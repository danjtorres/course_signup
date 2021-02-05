using Courses.Domain.Course;
using Courses.Domain.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Courses.Infra
{
    public class CourseContext : DbContext, IDisposable, IUnitOfWork
    {

        public CourseContext(DbContextOptions<CourseContext> options)
            : base(options)
        {
            
        }

        public DbSet<Course> Course { get; set; }

        public async Task<bool> Commit()
        {
            //manage createdAt and UdatedAt
            var DateTimeNow = DateTime.Now;
            foreach (var entry in ChangeTracker.Entries()
                .Where(entry => entry.Entity.GetType().GetProperty("CreatedAt") != null))
            {
                if (entry.State == EntityState.Added)
                {
                    
                    entry.Property("CreatedAt").CurrentValue = DateTimeNow;
                    entry.Property("UpdatedAt").CurrentValue = DateTimeNow;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("CreatedAt").IsModified = false;
                    entry.Property("UpdatedAt").CurrentValue = DateTimeNow;
                }
            }

            var sucesso = await base.SaveChangesAsync() > 0;

            //todo: publish message events after saving using Mediatr

            return sucesso;
        }
    }
}
