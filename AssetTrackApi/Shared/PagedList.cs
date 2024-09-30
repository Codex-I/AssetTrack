namespace System.Collections.Generic
{
    public class PagedList<T> : List<T>
    {
        public int PageNumber { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalItems { get; private set; }
        public bool HasPrevious => PageNumber > 1;
        public bool HasNext => PageNumber < TotalPages;

        internal PagedList(IList<T> items, int total, int pageNumber, int pageSize)
        {
            TotalItems = total;
            PageSize = pageSize;
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling((decimal)total / pageSize);
            AddRange(items);
        }
    }
}