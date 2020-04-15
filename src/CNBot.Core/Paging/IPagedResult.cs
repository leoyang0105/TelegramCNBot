using System;
using System.Collections.Generic;
using System.Text;

namespace CNBot.Core.Paging
{
    public interface IPagedResult<T>
    {
        int PageIndex { get; }
        int PageSize { get; }
        int TotalCount { get; }
        int TotalPages { get; }
        IEnumerable<T> Data { get; }
    }
}
