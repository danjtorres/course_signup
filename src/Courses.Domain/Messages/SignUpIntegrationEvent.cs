using System;

namespace Courses.Domain.Messages
{
    public class SignUpIntegrationEvent : IntegrationEvent
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Guid CourseId { get; set; }

        public SignUpIntegrationEvent(string name, string email, DateTime dateOfBirth, Guid courseId)
        {
            Name = name;
            Email = email;
            DateOfBirth = dateOfBirth;
            CourseId = courseId;
        }

    }
}
