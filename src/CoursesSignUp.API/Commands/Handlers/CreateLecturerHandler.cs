using Courses.Domain.Course;
using Courses.Domain.Data;
using Courses.Domain.Lecturer;
using CoursesSignUp.API.Commands.Requests;
using CoursesSignUp.API.Commands.Responses;
using FluentValidation.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using Newtonsoft.Json;

namespace CoursesSignUp.API.Commands.Handlers
{
    public class CreateLecturerHandler : IRequestHandler<CreateLecturerRequest, CreateLecturerResponse>
    {

        private readonly ILecturerRepository _lecturerRepository;
        private static Logger _log = LogManager.GetCurrentClassLogger();

        public CreateLecturerHandler(ILecturerRepository lecturerRepository)
        {
            _lecturerRepository = lecturerRepository;
        }

        public async Task<CreateLecturerResponse> Handle(CreateLecturerRequest request, CancellationToken cancellationToken)
        {
            var result = new CreateLecturerResponse();

            try
            {
                // Validation
                if (!request.IsValid())
                {
                    result.ValidationResult = request.ValidationResult;
                    return await Task.FromResult(result);
                }

                // Map Lecturer
                var lecturer = MapLecturer(request);

                //add lecturer to context repository
                _lecturerRepository.Create(lecturer);

                // persist data
                result.ValidationResult = await PersistData(_lecturerRepository.UnitOfWork);

                //Map return
                result.LecturerId = lecturer.Id;
            }
            catch (Exception ex)
            {
                _log.Error($"", JsonConvert.SerializeObject(ex));
                result.ValidationResult.Errors.Add(new ValidationFailure(string.Empty, "An error has occurred please try again."));
            }

            return await Task.FromResult(result);

        }

        private Lecturer MapLecturer(CreateLecturerRequest request)
        {
            var mapped = new Lecturer();

            mapped.Id = Guid.NewGuid();
            mapped.Name = request.Name;

            return mapped;
        }

        protected async Task<ValidationResult> PersistData(IUnitOfWork unitOfWork)
        {
            ValidationResult validationResult = new ValidationResult();

            if (!await unitOfWork.Commit())
                validationResult.Errors.Add(new ValidationFailure(string.Empty, "Houve um erro ao persistir os dados"));

            return validationResult;
        }

    }
}
