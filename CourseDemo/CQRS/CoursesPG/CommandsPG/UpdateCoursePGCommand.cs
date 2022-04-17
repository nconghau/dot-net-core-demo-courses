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
    public class UpdateCoursePGCommand : IRequestWrapper<object>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
    }
    public class UpdateCoursePGCommandHandler : IHandlerWrapper<UpdateCoursePGCommand, object>
    {
        private readonly ICoursesPGRepository _coursesPGRepository;
        private readonly ILogger<UpdateCoursePGCommandHandler> _logger;
        private readonly IMapper _mapper;
        public UpdateCoursePGCommandHandler(ICoursesPGRepository coursePGRepository, ILogger<UpdateCoursePGCommandHandler> logger, IMapper mapper)
        {
            _coursesPGRepository = coursePGRepository;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<JsonResponse<object>> Handle(UpdateCoursePGCommand request, CancellationToken cancellationToken)
        {
            try
            {
                UpdateCourseRequestDto updateCourse = _mapper.Map<UpdateCourseRequestDto>(request);
                bool success = await _coursesPGRepository.UpdateAsync(updateCourse);
                return new JsonResponse<object>(success, new { }, ResponseMessage.UpdateSuccess.ToString());
            }
            catch (Exception e)
            {
                _logger.LogError("[Error] " + e);
                return new JsonResponse<object>(false, new { }, e.Message.ToString());
            }
        }
    }
    public class UpdateCoursePGCommandValidator : AbstractValidator<UpdateCoursePGCommand>
    {
        public UpdateCoursePGCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Content).NotEmpty();
        }
    }
    public class UpdateCoursePGCommandMapper : Profile
    {
        public UpdateCoursePGCommandMapper()
        {
            CreateMap<UpdateCoursePGCommand, UpdateCourseRequestDto>().ReverseMap();
        }
    }
}
