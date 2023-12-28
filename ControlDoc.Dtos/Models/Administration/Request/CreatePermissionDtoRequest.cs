namespace ControlDoc.Models.Models.Administration.Request
{
    public class CreatePermissionDtoRequest
    {
        public int functionalityId { get; set; }
        public int userId { get; set; }
        public int profileId { get; set; }
        public bool accessF { get; set; }
        public bool createF { get; set; }
        public bool modifyF { get; set; }
        public bool consultF { get; set; }
        public bool deleteF { get; set; } = false;
        public bool printF { get; set; }
    }
}