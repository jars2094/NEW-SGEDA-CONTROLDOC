namespace ControlDoc.Models.Models.Administration.Response
{
    public class DocumentaryTypologiesBagDtoResponse
    {
        public int DocumentaryTypologyBagId { get; set; }
        public string TypologyName { get; set; } = null!;
        public string TypologyDescription { get; set; } = null;
        public bool Active { get; set; }
        public string CreateUser { get; set; } = null!;
        public string UpdateUser { get; set; } = null;
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}