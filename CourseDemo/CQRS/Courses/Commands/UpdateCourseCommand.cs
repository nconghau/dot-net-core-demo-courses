using AutoMapper;
using CourseDemo.Domain.Common;
using CourseDemo.DTO.Request;
using CourseDemo.Repositories.Mongo;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using static CourseDemo.Enums.ResponseMessageEnum;

namespace CourseDemo.Services.Courses.Commands
{
    public class UpdateCourseCommand : IRequestWrapper<object>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
    }
    public class UpdateCourseCommandHandler : IHandlerWrapper<UpdateCourseCommand, object>
    {
        private readonly ICoursesRepository _coursesRepository;
        private readonly ILogger<UpdateCourseCommandHandler> _logger;
        private readonly IMapper _mapper;
        public UpdateCourseCommandHandler(ICoursesRepository courseRepository, ILogger<UpdateCourseCommandHandler> logger, IMapper mapper)
        {
            _coursesRepository = courseRepository;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<JsonResponse<object>> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                UpdateCourseRequestDto updateCourse = _mapper.Map<UpdateCourseRequestDto>(request);
                bool success = await _coursesRepository.UpdateAsync(updateCourse);
                return new JsonResponse<object>(success, new { }, ResponseMessage.UpdateSuccess.ToString());
            }
            catch (Exception e)
            {
                _logger.LogError("[Error] " + e);
                return new JsonResponse<object>(false, new { }, e.Message.ToString());
            }
        }
    }
    public class UpdateCourseCommandValidator : AbstractValidator<UpdateCourseCommand>
    {
        public UpdateCourseCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Content).NotEmpty();
        }
    }
    public class UpdateCourseCommandMapper : Profile
    {
        public UpdateCourseCommandMapper()
        {
            CreateMap<UpdateCourseCommand, UpdateCourseRequestDto>().ReverseMap(); 
        }
    }
}
