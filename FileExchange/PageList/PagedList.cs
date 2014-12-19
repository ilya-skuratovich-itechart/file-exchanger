using System.Collections;
using System.Collections.Generic;
using PagedList;

namespace FileExchange.PageList
{
    public class PagedList<T>:BasePagedList<T>
    {
        public PagedList(IEnumerable<T> pagedRecord, int pageNumber, int pageSize, int totalItemsCount)
            : base(pageNumber, pageSize, totalItemsCount)
        {
            base.Subset.AddRange(pagedRecord);
        }
    }
}