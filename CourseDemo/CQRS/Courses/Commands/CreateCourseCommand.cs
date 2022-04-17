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
    public class CreateCourseCommand : IRequestWrapper<object>
    {
        public string Name { get; set; }
        public string Content { get; set; }
    }
    public class CreateCourseCommandHandler : IHandlerWrapper<CreateCourseCommand, object>
    {
        private readonly ICoursesRepository _coursesRepository;
        private readonly ILogger<CreateCourseCommandHandler> _logger;
        private readonly IMapper _mapper;
        public CreateCourseCommandHandler(ICoursesRepository courseRepository, ILogger<CreateCourseCommandHandler> logger, IMapper mapper)
        {
            _coursesRepository = courseRepository;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<JsonResponse<object>> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                CreateCourseRequestDto createCourseDto = _mapper.Map<CreateCourseRequestDto>(request);
                bool success = await _coursesRepository.CreateAsync(createCourseDto);
                return new JsonResponse<object>(success, new { }, ResponseMessage.CreateSuccess.ToString());
            }
            catch (Exception e)
            {
                _logger.LogError("[Error] " + e);
                return new JsonResponse<object>(false, new { }, e.Message.ToString());
            }
        }
    }
    public class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
    {
        public CreateCourseCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Content).NotEmpty();
        }
    }
    public class CreateCourseCommandMapper : Profile
    {
        public CreateCourseCommandMapper()
        {
            CreateMap<CreateCourseCommand, CreateCourseRequestDto>().ReverseMap();
        }
    }
}
