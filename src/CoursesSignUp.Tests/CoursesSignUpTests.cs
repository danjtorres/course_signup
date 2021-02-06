using NUnit.Framework;
using Courses.Domain.Course;
using CoursesSignUp.API.Commands.Requests;
using Courses.Domain.Lecturer;
using Courses.MessageBus;
using Courses.Domain.Messages;
using System.Threading.Tasks;

namespace CoursesSignUp.Tests
{
    public class CoursesSignUpTests
    {

        private Moq.Mock<ICourseRepository> _courseRepository;
        private Moq.Mock<ILecturerRepository> _lecturerRepository;
        private Moq.Mock<IMessageBus> _messageBus;

        [SetUp]
        public void Setup()
        {
            _courseRepository = new Moq.Mock<ICourseRepository>();
            _lecturerRepository = new Moq.Mock<ILecturerRepository>();
            _messageBus = new Moq.Mock<IMessageBus>();
        }

        [Test, Timeout(5000)]
        public void SignUpThousandCoursesInFiveSecondsTest()
        {
            var message = new SignUpIntegrationEvent("RabbitMq Overview", "torres@chama.com", new System.DateTime(1990, 12, 16), new System.Guid("57D92BF1-7AD6-4ACC-901C-8A54ACB39E76"));
            _messageBus.Setup(p => p.PublishAsync(message)).Returns(Task.FromResult(true));

            var cancellationToken = new System.Threading.CancellationToken();
            var signUpCourseRequest = new SignUpCourseRequest()
            {
                Name = "Torres",
                Email = "torres@chama.com",
                DateOfBirth = new System.DateTime(1990, 12, 16),
                CourseId = new System.Guid("57D92BF1-7AD6-4ACC-901C-8A54ACB39E76")
            };

            var handler = new API.Commands.Handlers.SignUpCourseHandler(_messageBus.Object);

            var lastResponse = new API.Commands.Responses.SignUpCourseResponse();

            for (int i = 0; i < 1000; i++)
            {
                lastResponse = handler.Handle(signUpCourseRequest, cancellationToken).Result;
            }

            Assert.IsTrue(lastResponse.ValidationResult.IsValid == true);
        }

        [Test]
        public void SignUpCourseTest()
        {
            var message = new SignUpIntegrationEvent("RabbitMq Overview", "torres@chama.com", new System.DateTime(1990, 12, 16), new System.Guid("57D92BF1-7AD6-4ACC-901C-8A54ACB39E76"));
            _messageBus.Setup(p => p.PublishAsync(message)).Returns(Task.FromResult(true));

            var cancellationToken = new System.Threading.CancellationToken();
            var signUpCourseRequest = new SignUpCourseRequest()
            {
                Name = "Torres",
                Email = "torres@chama.com",
                DateOfBirth = new System.DateTime(1990, 12, 16),
                CourseId = new System.Guid("57D92BF1-7AD6-4ACC-901C-8A54ACB39E76")
            };


            var handler = new API.Commands.Handlers.SignUpCourseHandler(_messageBus.Object);
            var response = handler.Handle(signUpCourseRequest, cancellationToken);

            Assert.IsTrue(response.Result.ValidationResult.IsValid == true);
        }

        [Test]
        public void CreateCourseTest()
        {
            _courseRepository.Setup(p => p.UnitOfWork.Commit()).Returns(Task.FromResult(true));

            var cancellationToken = new System.Threading.CancellationToken();
            var createCourseRequest = new CreateCourseRequest()
            {
                Name = "RabbitMq Overview",
                Capacity = 20,
                NumberOfStudents = 0,
                LecturerId = new System.Guid("30A297B1-00F4-4634-AE70-988EAB8852C0")
            };

            var handler = new API.Commands.Handlers.CreateCourseHandler(_courseRepository.Object);
            var response = handler.Handle(createCourseRequest, cancellationToken);

            Assert.IsTrue(response.Result.ValidationResult.IsValid == true);
        }

        [Test]
        public void CreateLecturerTest()
        {
            _lecturerRepository.Setup(p => p.UnitOfWork.Commit()).Returns(System.Threading.Tasks.Task.FromResult(true));

            var cancellationToken = new System.Threading.CancellationToken();
            var createLecturerRequest = new CreateLecturerRequest()
            {
                Name = "RabbitMq Overview"
            };

            var handler = new API.Commands.Handlers.CreateLecturerHandler(_lecturerRepository.Object);
            var response = handler.Handle(createLecturerRequest, cancellationToken);

            Assert.IsTrue(response.Result.ValidationResult.IsValid == true);
        }
    }
}