using CourseDemo.Domain.Common;
using CourseDemo.Repositories.Postgres;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using static CourseDemo.Enums.ResponseMessageEnum;

namespace CourseDemo.Services.CoursesPG.CommandsPG
{
    public class DeleteCoursePGCommand : IRequestWrapper<object>
    {
        public string Id { get; }
        public DeleteCoursePGCommand(string id)
        {
            Id = id;
        }
    }
    public class DeleteCoursePGCommandHandler : IHandlerWrapper<DeleteCoursePGCommand, object>
    {
        private readonly ICoursesPGRepository _coursesPGRepository;
        private readonly ILogger<DeleteCoursePGCommandHandler> _logger;
        public DeleteCoursePGCommandHandler(ICoursesPGRepository coursesPGRepository, ILogger<DeleteCoursePGCommandHandler> logger)
        {
            _coursesPGRepository = coursesPGRepository;
            _logger = logger;
        }
        public async Task<JsonResponse<object>> Handle(DeleteCoursePGCommand request, CancellationToken cancellationToken)
        {
            try
            {
                bool success = await _coursesPGRepository.DeleteAsync(request.Id);
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
    public class DeleteCoursePGCommandValidator : AbstractValidator<DeleteCoursePGCommand>
    {
        public DeleteCoursePGCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
