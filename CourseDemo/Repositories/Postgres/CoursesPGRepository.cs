using CourseDemo.Context.Postgres;
using CourseDemo.DTO.Request;
using CourseDemo.DTO.Response;
using CourseDemo.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseDemo.Repositories.Postgres
{
    public class CoursesPGRepository : ICoursesPGRepository
    {
        private readonly PGContext _coursesPGContext;
        public CoursesPGRepository(PGContext coursesPGContext)
        {
            _coursesPGContext = coursesPGContext;
        }
        public async Task<GetAllCoursePGResponseDto> GetAllAsync(int page, int pagesize, string keyword)
        {
            // Add Datas
            var datas = new List<CoursePGModel>();
            var totalItem = 0.0;
            if (keyword == "")
            {
                datas = await _coursesPGContext.CoursePGModel.Where(f => true).Skip((page - 1) * pagesize).Take(pagesize).ToListAsync();
                totalItem = await _coursesPGContext.CoursePGModel.CountAsync();
            }
            else
            {
                var datasSearch = _coursesPGContext.CoursePGModel.Where(f => f.Name.Contains(keyword));
                totalItem = datasSearch.ToList().Count();
                datas = await datasSearch.Skip((page - 1) * pagesize).Take(pagesize).ToListAsync();
            }
            // Add Pagination
            int totalPage = (int)Math.Ceiling((double)totalItem / pagesize);
            int totalRecord = datas.Count();
            var pagination = new PaginationDto(totalPage, page, totalRecord);
            // Mapper
            return new GetAllCoursePGResponseDto(datas, pagination);
        }

        public async Task<CoursePGModel> GetByIdAsync(string id)
        {
            var result = await _coursesPGContext.CoursePGModel.Where(x => x.Id == id).Take(1).ToListAsync();
            return result.FirstOrDefault();
        }

        public async Task<bool> CreateAsync(CreateCourseRequestDto createCourseDto)
        {
            await _coursesPGContext.CoursePGModel.AddAsync(createCourseDto.ToModelPG());
            int changes = await _coursesPGContext.SaveChangesAsync();
            return changes >= 0;
        }

        public async Task<bool> UpdateAsync(UpdateCourseRequestDto updateCourseDto)
        {
            var course = await _coursesPGContext.CoursePGModel.Where(x => x.Id == updateCourseDto.Id).FirstOrDefaultAsync();
            course.Content = updateCourseDto.Content;
            course.Name = updateCourseDto.Name;
            int changes = await _coursesPGContext.SaveChangesAsync();
            return changes >= 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            _coursesPGContext.CoursePGModel.Remove(new CoursePGModel { Id = id });
            int changes = await _coursesPGContext.SaveChangesAsync();
            return changes >= 0;
        }

        public bool Create(CreateCourseRequestDto createCourseDto)
        {
            _coursesPGContext.CoursePGModel.Add(createCourseDto.ToModelPG());
            int changes = _coursesPGContext.SaveChanges();
            return changes >= 0;
        }

        public List<CoursePGModel> GetAllByKeyWord(string keyword)
        {
            return _coursesPGContext.CoursePGModel.Where(f => f.Name.Contains(keyword)).ToList();
        }
    }
    // For Test
    //public class UpdateCourseCommandMapper : Profile
    //{
    //    public UpdateCourseCommandMapper()
    //    {
    //        CreateMap<UpdateCourseRequestDto, CoursePGModel>().ReverseMap();
    //    }
    //}
}
