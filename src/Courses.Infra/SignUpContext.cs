using Courses.Domain.Course;
using Courses.Domain.Data;
using Courses.Domain.Lecturer;
using Courses.Domain.SignUp;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Courses.Infra
{
    public class SignUpContext : DbContext, IDisposable, IUnitOfWork
    {

        public SignUpContext(DbContextOptions<SignUpContext> options)
            : base(options)
        {
            
        }

        public DbSet<SignUpCourse> SignUpCourse { get; set; }

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

            //NextStep
            //todo: publish message events after saving.

            return sucesso;
        }
    }
}
