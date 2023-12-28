namespace ControlDoc.Models.Models.Authentication.Login.Request
{
    public class UpdateRecoveryCodeDtoRequest
    {
        public string? Code { get; set; }
        public string? Ip { get; set; }
        public string? Uuid { get; set; }
    }
}
