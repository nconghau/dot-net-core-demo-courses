using System;
using System.Collections.Generic;

namespace CourseDemo.DTO.Response
{
    public class BasePaginationDto
    {
        public int TotalPage { get; }
        public int CurrentPage { get; }
        public int TotalRecord { get; }
        public BasePaginationDto()
        {
        }
        public BasePaginationDto(int totalPagel, int currentPagel, int totalRecord)
        {
            TotalPage = totalPagel;
            CurrentPage = currentPagel;
            TotalRecord = totalRecord;
        }
    }
    public class BaseGetAllResponseDto<TEntity>
    {
        public List<TEntity> Datas { get; }
        public PaginationDto Pagination { get; }
        public BaseGetAllResponseDto(List<TEntity> datas, PaginationDto pagination)
        {
            Datas = datas;
            Pagination = pagination;
        }
    }
}
