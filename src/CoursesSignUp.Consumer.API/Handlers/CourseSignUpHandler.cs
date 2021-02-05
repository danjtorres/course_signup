using Courses.Domain.Course;
using Courses.Domain.Data;
using Courses.Domain.Messages;
using Courses.Domain.SignUp;
using Courses.MessageBus;
using CoursesSignUp.Consumer.API.Queries;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoursesSignUp.Consumer.API.Handlers
{
    public class CourseSignUpHandler : BackgroundService
    {
        private readonly IMessageBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public CourseSignUpHandler(IServiceProvider serviceProvider, IMessageBus bus)
        {
            _serviceProvider = serviceProvider;
            _bus = bus;
        }

        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            SetConsumers();

            return Task.CompletedTask;
        }

        private void SetConsumers()
        {
            _bus.SubscribeAsync<SignUpIntegrationEvent>("SignUpCourse",
                async request => await SignUpCourse(request));
        }

        private async Task SignUpCourse(SignUpIntegrationEvent request)
        {

            using (var scope = _serviceProvider.CreateScope())
            {
                var courseRepository = scope.ServiceProvider.GetRequiredService<ICourseRepository>();
                var signUpRepository = scope.ServiceProvider.GetRequiredService<ISignUpRepository>();

                //verify
                bool isAvailable = await VerifyAvailableRoomAsync(courseRepository, signUpRepository, request.CourseId);
                bool notAppliedYet = await VerifyStudentNotAppliedYet(signUpRepository, request.CourseId, request.Email);

                if (isAvailable && notAppliedYet)
                {
                    // Map signUpCourse
                    var signUpCourse = MapSignUpCourse(request);

                    //add course to context repository
                    signUpRepository.Create(signUpCourse);

                    //NextStep
                    //validationResult false need to register the error, retry X times. 
                    //add sign up course to context repository 
                    var validationResult = await PersistData(signUpRepository.UnitOfWork);

                    //send a confirmation email
                    SendEmail(true);
                }
                else
                {
                    //send a decline email if not applyed yet
                    if (notAppliedYet)
                        SendEmail(false);
                }

                await Task.FromResult(true);
            }

        }

        private async Task<bool> VerifyAvailableRoomAsync(ICourseRepository courseRepository, ISignUpRepository signUpRepository, Guid courseId)
        {

            var capacity = await new CourseQueries(courseRepository).CourseCapacity(courseId);
            var students = await new SignUpQueries(signUpRepository).NumberOfStudents(courseId);

            var available = capacity > students ? true : false;

            return await Task.FromResult(available);
        }

        private async Task<bool> VerifyStudentNotAppliedYet(ISignUpRepository signUpRepository, Guid courseId, string email)
        {

            bool notApplyedYet = await new SignUpQueries(signUpRepository).VerifyStudentNotAppliedYet(courseId, email);

            return await Task.FromResult(notApplyedYet);
        }

        private SignUpCourse MapSignUpCourse(SignUpIntegrationEvent request)
        {
            var mapped = new SignUpCourse();

            mapped.Id = Guid.NewGuid();
            mapped.Name = request.Name;
            mapped.Email = request.Email;
            mapped.DateOfBirth = request.DateOfBirth;
            mapped.CourseId = request.CourseId;

            return mapped;
        }

        private async Task<ValidationResult> PersistData(IUnitOfWork unitOfWork)
        {
            ValidationResult validationResult = new ValidationResult();

            if (!await unitOfWork.Commit())
                validationResult.Errors.Add(new ValidationFailure(string.Empty, "Houve um erro ao persistir os dados"));

            return validationResult;
        }

        private void SendEmail(bool confirmation)
        {
            if (confirmation)
            {
                //NextStep
                //log email confirmation;
            }
            else
            {
                //NextStep
                //log email decline;
            }
        }

    }
}
