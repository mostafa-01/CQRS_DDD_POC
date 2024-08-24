

namespace CQRSAndDDD.App.Queries
{
    public class GetTicketsQuery
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetTicketsQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
