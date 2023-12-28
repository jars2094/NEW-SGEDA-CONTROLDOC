namespace ControlDoc.Models.Models.Administration.Response
{
    public class SeriesDtoResponse
    {
        public int SeriesId { get; set; }
        public int ProductionOfficeId { get; set; }
        public string? ProductionOfficeName { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; } = null!;
        public string? Description { get; set; }
        public bool ActiveState { get; set; }
        public string UpdateUser { get; set; }
    }
}