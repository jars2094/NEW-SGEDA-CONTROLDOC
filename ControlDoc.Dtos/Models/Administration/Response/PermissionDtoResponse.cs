namespace ControlDoc.Models.Models.Administration.Response
{


    public class PermissionDtoResponse
    {
        public int permissionId { get; set; }
        public int functionalityId { get; set; }
        public string functionalityName { get; set; }
        public int profileId { get; set; }
        public bool accessF { get; set; }
        public bool createF { get; set; }
        public bool modifyF { get; set; }
        public bool consultF { get; set; }
        public bool deleteF { get; set; }
        public bool printF { get; set; }
        public bool activeState { get; set; }
    } 
}
