namespace ControlDoc.Models.Models.Authentication.Login.Request
{
    public class LoginUserDtoRequest
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Ip { get; set; }
        public string? Uuid { get; set; }
        public int? CompanyId { get; set; }
    }
}
