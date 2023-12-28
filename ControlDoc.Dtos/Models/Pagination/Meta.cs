namespace ControlDoc.Models.Models.Pagination
{
    public class Meta
    {
        public int totalCount { get; set; }
        public int pageSize { get; set; }
        public int currentPage { get; set; }
        public int totalPages { get; set; }
        public bool hasNextPage { get; set; }
        public bool hasPreviousPage { get; set; }
        public string firstPageUrl { get; set; }
        public string lastPageUrl { get; set; }
        public string nextPageUrl { get; set; }
        public string previousPageUrl { get; set; }
    }
}
