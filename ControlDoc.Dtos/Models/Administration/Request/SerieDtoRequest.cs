namespace ControlDoc.Models.Models.Administration.Request
{
    public class SerieDtoRequest
    {
        public int ProductionOfficeId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; } = null!;
        public string Description { get; set; }
        public bool ActiveState { get; set; }
        public string CreateUser { get; set; }
    }
}