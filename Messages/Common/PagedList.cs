using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.Common
{

    public class PaginatedList<T>
    {
        public List<T> Items { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }

        public PaginatedList()
        {

        }

        public PaginatedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
            Items = items;
        }

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < TotalPages;

        public static async Task<PaginatedList<T>> CreateAsync(List<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source;

            return new PaginatedList<T>(items, count, pageNumber, pageSize);
        }
    }

}
