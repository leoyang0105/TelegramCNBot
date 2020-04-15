using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CNBot.Core.Paging
{
    public class PagedResult<T> : IPagedResult<T>
    {
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPages { get; private set; }
        public IEnumerable<T> Data { get; private set; }

        public PagedResult(IQueryable<T> source, int pageIndex, int pageSize)
        {
            if (pageIndex > 0)
                pageIndex--;
            int totalCount = source.Count();
            TotalCount = totalCount;
            TotalPages = totalCount / pageSize;

            if (totalCount % pageSize > 0)
                TotalPages++;
            PageSize = pageSize;
            PageIndex = pageIndex + 1;
            Data = source.Skip(pageIndex * pageSize).Take(pageSize);
        }

        public PagedResult(IEnumerable<T> source, int pageIndex, int pageSize)
        {
            if (pageIndex > 0)
                pageIndex--;
            int totalCount = source.Count();
            TotalCount = totalCount;
            TotalPages = totalCount / pageSize;

            if (totalCount % pageSize > 0)
                TotalPages++;
            PageSize = pageSize;
            PageIndex = pageIndex + 1;
            Data = source.Skip(pageIndex * pageSize).Take(pageSize);
        }
        public PagedResult(IEnumerable<T> source, int pageIndex, int pageSize, int totalCount)
        {
            this.Data = source;
            TotalCount = totalCount;
            TotalPages = totalCount / pageSize;
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.TotalCount = totalCount;
        }
    }
}
