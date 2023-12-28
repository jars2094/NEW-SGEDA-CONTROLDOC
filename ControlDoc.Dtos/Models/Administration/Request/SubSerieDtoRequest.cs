namespace ControlDoc.Models.Models.Administration.Request
{
    public class SubSerieDtoRequest
    {
        public int SeriesId { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; }
        public string Description { get; set; }
        public bool ActiveState { get; set; }
        public string CreateUser { get; set; }
    }
}