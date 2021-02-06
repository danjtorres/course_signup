using System;
using Courses.Domain.Course;
using Courses.Domain.Lecturer;
using Courses.Domain.Messages;
using Courses.MessageBus;
using NUnit.Framework;
using System.Threading.Tasks;

namespace CoursesSignUp.Consumer.Tests
{
    public class CoursesSignUpConsumerTests
    {

        private Moq.Mock<IMessageBus> _messageBus;
        private Moq.Mock<IServiceProvider> _serviceProvider;
        private Moq.Mock<ICourseRepository> _courseRepository;
        private Moq.Mock<Courses.Infra.CourseContext> _courseContext;
        private Moq.Mock<Courses.Domain.Data.IUnitOfWork> _unitOfWork;

        [SetUp]
        public void Setup()
        {
            _messageBus = new Moq.Mock<IMessageBus>();
            _serviceProvider = new Moq.Mock<IServiceProvider>();
            _courseRepository = new Moq.Mock<ICourseRepository>();
        }

        //[Test, Timeout(5000)]

        [Test]
        public void SubscribeCourseTest()
        {

            var message = new SignUpIntegrationEvent("RabbitMq Overview", "torres@chama.com", new System.DateTime(1990, 12, 16), new System.Guid("57D92BF1-7AD6-4ACC-901C-8A54ACB39E76"));

            _serviceProvider
                .Setup(x => x.GetService(typeof(ICourseRepository)))
                .Returns(_courseRepository);
            
            var serviceScope = new Moq.Mock<Microsoft.Extensions.DependencyInjection.IServiceScope>();
            serviceScope.Setup(x => x.ServiceProvider).Returns(_serviceProvider.Object);

            var serviceScopeFactory = new Moq.Mock<Microsoft.Extensions.DependencyInjection.IServiceScopeFactory>();
            serviceScopeFactory
                .Setup(x => x.CreateScope())
                .Returns(serviceScope.Object);

            _serviceProvider
                .Setup(x => x.GetService(typeof(Microsoft.Extensions.DependencyInjection.IServiceScopeFactory)))
                .Returns(serviceScopeFactory.Object);

            var handler = new API.Handlers.CourseSignUpHandler(_serviceProvider.Object, _messageBus.Object);
            var response = handler.SignUpCourse(message);

            Assert.IsTrue(response.IsCompletedSuccessfully == true);
        }

    }
}