using Courses.Domain.Course;
using Courses.Domain.Data;
using Courses.Domain.Messages;
using Courses.Domain.SignUp;
using Courses.MessageBus;
using CoursesSignUp.Consumer.API.Queries;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using System;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using Newtonsoft.Json;

namespace CoursesSignUp.Consumer.API.Handlers
{
    public class CourseSignUpHandler : BackgroundService
    {
        private readonly IMessageBus _bus;
        private readonly IServiceProvider _serviceProvider;
        private static Logger _log = LogManager.GetCurrentClassLogger();

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

        public async Task SignUpCourse(SignUpIntegrationEvent request)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var courseRepository = scope.ServiceProvider.GetRequiredService<ICourseRepository>();
                    var signUpRepository = scope.ServiceProvider.GetRequiredService<ISignUpRepository>();

                    //verify
                    bool isAvailable = await VerifyAvailableRoomAsync(courseRepository, signUpRepository, request.CourseId);
                    bool notAppliedYet = await VerifyStudentNotAppliedYet(signUpRepository, request.CourseId, request.Email);

                    // Map signUpCourse
                    var signUpCourse = MapSignUpCourse(request);

                    if (isAvailable && notAppliedYet)
                    {

                        //add course to context repository
                        signUpRepository.Create(signUpCourse);

                        //add sign up course to context repository 
                        var validationResult = await PersistData(signUpRepository.UnitOfWork);

                        //send a confirmation email
                        SendEmail(signUpCourse, true);
                    }
                    else
                    {
                        //send a decline email if not applyed yet
                        if (notAppliedYet)
                            SendEmail(signUpCourse, false);
                    }

                    await Task.FromResult(true);
                }

            }
            catch (Exception ex)
            {
                _log.Error($"", JsonConvert.SerializeObject(ex));
                //NextStep
                //In erro, retry X times.
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

        private void SendEmail(SignUpCourse signUpCourse, bool confirmation)
        {
            if (confirmation)
            {
                _log.Info($"Congratulation {signUpCourse.Name}, you're registerd to the course.");
                //NextStep
                //create the email sender;
            }
            else
            {
                _log.Info($"Hello {signUpCourse.Name}, unfortunately our course has no more vacancies, if you wish, subscribe to another available.");
                //NextStep
                //log email decline;
            }
        }

    }
}
