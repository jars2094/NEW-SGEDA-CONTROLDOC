namespace ControlDoc.Services.Interfaces
{
    public interface IAuthenticationJWT
    {
        Task LoginToken(string token);
        Task LogoutToken();
        Task TokenRenewalManagement();
    }
}
