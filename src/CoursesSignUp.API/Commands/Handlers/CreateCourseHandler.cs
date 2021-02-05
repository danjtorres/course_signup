using Courses.Domain.Course;
using Courses.Domain.Data;
using CoursesSignUp.API.Commands.Requests;
using CoursesSignUp.API.Commands.Responses;
using FluentValidation.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoursesSignUp.API.Commands.Handlers
{
    public class CreateCourseHandler : IRequestHandler<CreateCourseRequest, CreateCourseResponse>
    {
        private readonly ICourseRepository _courseRepository;

        public CreateCourseHandler(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<CreateCourseResponse> Handle(CreateCourseRequest request, CancellationToken cancellationToken)
        {
            var result = new CreateCourseResponse();

            // Validation
            if (!request.IsValid())
            {
                result.ValidationResult = request.ValidationResult;
                return await Task.FromResult(result);
            }

            // Map Course
            var course = MapCourse(request);

            //add course to context repository
            _courseRepository.Create(course);

            // persist data
            result.ValidationResult = await PersistData(_courseRepository.UnitOfWork);
            
            //Map return
            result.CourseId = course.Id;

            return await Task.FromResult(result);

        }

        private Course MapCourse(CreateCourseRequest request)
        {
            var mapped = new Course();

            mapped.Id = Guid.NewGuid();
            mapped.Name = request.Name;
            mapped.Capacity = request.Capacity;
            mapped.NumberOfStudents = request.NumberOfStudents;
            mapped.LecturerId = request.LecturerId;

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
