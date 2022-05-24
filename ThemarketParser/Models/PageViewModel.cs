namespace ThemarketParser.Models
{
    public class PageViewModel
    {
        public int PageNumber { get; private set; }
        public int TotalPages { get; private set; }

        public PageViewModel(int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageNumber > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageNumber < TotalPages);
            }
        }

        public List<int> Pages {
            get
            {
                if (PageNumber == 1) return new List<int>() { 1, 2, 3};
                if (PageNumber == TotalPages) return new List<int>() { TotalPages-2, TotalPages-1, TotalPages };
                return new List<int>() { PageNumber - 1, PageNumber, PageNumber + 1 };
            }
        }
    }
}
