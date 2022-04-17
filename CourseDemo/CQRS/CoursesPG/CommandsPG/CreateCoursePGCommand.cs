using AutoMapper;
using CourseDemo.Domain.Common;
using CourseDemo.DTO.Request;
using CourseDemo.Repositories.Postgres;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using static CourseDemo.Enums.ResponseMessageEnum;

namespace CourseDemo.Services.CoursesPG.CommandsPG
{
    public class CreateCoursePGCommand : IRequestWrapper<object>
    {
        public string Name { get; set; }
        public string Content { get; set; }
    }
    public class CreateCoursePGCommandHandler : IHandlerWrapper<CreateCoursePGCommand, object>
    {
        private readonly ICoursesPGRepository _coursesPGRepository;
        private readonly ILogger<CreateCoursePGCommandHandler> _logger;
        private readonly IMapper _mapper;
        public CreateCoursePGCommandHandler(ICoursesPGRepository coursePGRepository, ILogger<CreateCoursePGCommandHandler> logger, IMapper mapper)
        {
            _coursesPGRepository = coursePGRepository;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<JsonResponse<object>> Handle(CreateCoursePGCommand request, CancellationToken cancellationToken)
        {
            try
            {
                CreateCourseRequestDto createCourseDto = _mapper.Map<CreateCourseRequestDto>(request);
                bool success = await _coursesPGRepository.CreateAsync(createCourseDto);
                return new JsonResponse<object>(success, new { }, ResponseMessage.CreateSuccess.ToString());
            }
            catch (Exception e)
            {
                _logger.LogError("[Error] " + e);
                return new JsonResponse<object>(false, new { }, e.Message.ToString());
            }
        }
    }
    public class CreateCoursePGCommandValidator : AbstractValidator<CreateCoursePGCommand>
    {
        public CreateCoursePGCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Content).NotEmpty();
        }
    }
    public class CreateCoursePGCommandMapper : Profile
    {
        public CreateCoursePGCommandMapper()
        {
            CreateMap<CreateCoursePGCommand, CreateCourseRequestDto>().ReverseMap();
        }
    }
}
