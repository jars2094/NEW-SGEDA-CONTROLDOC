namespace ControlDoc.Models.Models.Administration.Response
{
    public class DocumentalVersionsDtoResponse
    {
        public int DocumentalVersionId { get; set; }
        public int CompanyId { get; set; }
        public string VersionType { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? ActiveState { get; set; }
    }
}