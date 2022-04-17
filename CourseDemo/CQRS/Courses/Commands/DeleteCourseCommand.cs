using CourseDemo.Domain.Common;
using CourseDemo.Repositories.Mongo;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using static CourseDemo.Enums.ResponseMessageEnum;

namespace CourseDemo.Services.Courses.Commands
{
    public class DeleteCourseCommand : IRequestWrapper<object>
    {
        public string Id { get; }
        public DeleteCourseCommand(string id)
        {
            Id = id;
        }
    }
    public class DeleteCourseCommandHandler : IHandlerWrapper<DeleteCourseCommand, object>
    {
        private readonly ICoursesRepository _coursesRepository;
        private readonly ILogger<DeleteCourseCommandHandler> _logger;
        public DeleteCourseCommandHandler(ICoursesRepository courseRepository, ILogger<DeleteCourseCommandHandler> logger)
        {
            _coursesRepository = courseRepository;
            _logger = logger;
        }
        public async Task<JsonResponse<object>> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                bool success = await _coursesRepository.DeleteAsync(request.Id);
                if (success)
                {
                    return new JsonResponse<object>(success, new { }, ResponseMessage.DeleteSuccess.ToString());
                }
            }
            catch (Exception e)
            {
                _logger.LogError("[Error] " + e);
                return new JsonResponse<object>(false, new { }, e.Message.ToString());
            }
            return new JsonResponse<object>(false, new { }, ResponseMessage.DeleteFail.ToString());
        }
    }
    public class DeleteCourseCommandValidator : AbstractValidator<DeleteCourseCommand>
    {
        public DeleteCourseCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
