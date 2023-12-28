namespace ControlDoc.Models.Models.Administration.Response
{
    public class AdministrativeUnitDtoResponse
    {
        public int AdministrativeUnitId { get; set; }
        public int DocumentalVersionId { get; set; }
        public string? DocumentalVersionName { get; set; }
        public int? BossId { get; set; }
        public string? BossName { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool ActiveState { get; set; }
        public string? CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateUser { get; set; }
    }
}