using CourseDemo.DTO.Request;
using CourseDemo.DTO.Response;
using CourseDemo.Models;
using CourseDemo.Mongo.Context;
using CourseDemo.Mongo.UnitOfWork;
using CourseDemo.Repositories.Mongo.Base;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseDemo.Repositories.Mongo
{
    public class CoursesRepository : BaseRepository<CourseModel>, ICoursesRepository
    {
        private readonly IUnitOfWork _uow;
        public CoursesRepository(IMongoContext context, IUnitOfWork unitOfWork) : base(context)
        {
            _uow = unitOfWork;
        }
        public async Task<GetAllCourseResponseDto> GetAllAsync(int page, int pagesize, string keyword)
        {
            // Add Datas
            var datas = new List<CourseModel>();
            var totalItem = 0.0;
            if (keyword == "")
            {
                datas = await DbSet.Find(f => true).Skip((page - 1) * pagesize).Limit(pagesize).ToListAsync();
                totalItem = await DbSet.CountDocumentsAsync(f => true);
            }
            else
            {
                var datasSearch = DbSet.Find(f => f.Name.Contains(keyword));
                totalItem = datasSearch.ToList().Count();
                datas = await datasSearch.Skip((page - 1) * pagesize).Limit(pagesize).ToListAsync();
            }
            // Add Pagination
            int totalPage = (int)Math.Ceiling((double)totalItem / pagesize);
            int totalRecord = datas.Count();
            var pagination = new PaginationDto(totalPage, page, totalRecord);
            // Mapper
            return new GetAllCourseResponseDto(datas, pagination);
        }

        public async Task<CourseModel> GetByIdAsync(string id)
        {
            return await GetById(id);
        }

        public async Task<bool> CreateAsync(CreateCourseRequestDto createCourseDto)
        {
            Create(createCourseDto.ToModel());
            return await _uow.Commit();
        }

        public async Task<bool> UpdateAsync(UpdateCourseRequestDto updateCourseDto)
        {
            CourseModel courseModel = await GetById(updateCourseDto.Id);
            courseModel.Name = updateCourseDto.Name;
            courseModel.Content = updateCourseDto.Content;
            Update(courseModel);
            return await _uow.Commit();
        }

        public async Task<bool> DeleteAsync(string id)
        {
            Delete(id);
            return await _uow.Commit();
        }

        public bool Create(CreateCourseRequestDto createCourseDto)
        {
            DbSet.InsertOne(createCourseDto.ToModel());
            return true;
        }

        public List<CourseModel> GetAllByKeyWord(string keyword)
        {
            return DbSet.Find(f => f.Name.Contains(keyword)).ToList();
        }
    }
}
