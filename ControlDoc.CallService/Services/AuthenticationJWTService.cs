using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using ControlDoc.Models.Enums;
using ControlDoc.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Timer = System.Timers.Timer;

namespace ControlDoc.Services.Services
{
    public class AuthenticationJWTService : AuthenticationStateProvider, IAuthenticationJWT
    {
        //Variables
        private SessionStorageService SessionStorageService { get; set; }
        private CallService CallService { get; set; }

        private HttpClient _httpClient { get; set; }

        //Creando la authenticatión para usuario anonimo
        private static AuthenticationState Anonymous => new(new ClaimsPrincipal(new ClaimsIdentity()));

        //Constructor
        public AuthenticationJWTService(HttpClient httpClient, IJSRuntime jSRuntime)
        {
            _httpClient = httpClient;
            SessionStorageService = new SessionStorageService(jSRuntime);
            CallService = new CallService(httpClient, jSRuntime);
        }

        //Validar que el usuario este autenticado
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await SessionStorageService.GetValue<string>(ValuesKeys.Token);

            if (string.IsNullOrEmpty(token))
            {
                return Anonymous;
            }

            var timeExpiration = await SessionStorageService.GetValue<string>(ValuesKeys.TimeExpiration);
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(timeExpiration));
            var stringExpirationTime = dateTimeOffset.LocalDateTime.ToString();

            if (DateTime.TryParse(stringExpirationTime, out var expirationTime))
            {
                Console.WriteLine(ExpiredToken(expirationTime));
                if (ExpiredToken(expirationTime))
                {
                    await Clean();
                    return Anonymous;
                }
                else
                {
                    token = await RenewToken(token);
                }
            }


            //if (TokenMustBeRenewed(expirationTime, stringExpirationTime))
            //{
            //    token = await RenewToken(token);
            //}

            return await BuildAuthenticationStatus(token);
        }

        //Construir la autenticacion para el usuario
        private async Task<AuthenticationState> BuildAuthenticationStatus(string token)
        {
            var claims = ParseClaimsFromJwt(token);

            await SessionStorageService.RemoveItem(ValuesKeys.Token);
            await SessionStorageService.SetValue(ValuesKeys.Token, token);
            await SessionStorageService.RemoveItem(ValuesKeys.TimeExpiration);
            await SessionStorageService.SetValue(ValuesKeys.TimeExpiration, claims.FirstOrDefault(s => s.Type.Equals("exp")).Value);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                claims.FirstOrDefault(s => s.Type.Equals("IdentifierO")).Value);
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, "controlDoc")));
        }

        #region Renovacion token

        public async Task TokenRenewalManagement()
        {
            var timeExpiration = await SessionStorageService.GetValue<string>(ValuesKeys.TimeExpiration);
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(timeExpiration));
            var tiempoExpiracionString = dateTimeOffset.LocalDateTime.ToString();

            if (DateTime.TryParse(tiempoExpiracionString, out var expirationTime))
            {
                Console.WriteLine(ExpiredToken(expirationTime));
                if (ExpiredToken(expirationTime))
                {
                    await LogoutToken();
                }
                else
                {
                    var token = await SessionStorageService.GetValue<string>(ValuesKeys.Token);
                    var nuevoToken = await RenewToken(token);
                    Console.WriteLine(nuevoToken);
                    var authState = await BuildAuthenticationStatus(nuevoToken);
                    NotifyAuthenticationStateChanged(Task.FromResult(authState));
                }

                //if (TokenMustBeRenewed(expirationTime, tiempoExpiracionString))
                //{
                //    var token = await SessionStorageService.GetValue<string>(ValuesKeys.Token);
                //    var nuevoToken = await RenewToken(token);
                //    var authState = await BuildAuthenticationStatus(nuevoToken);
                //    NotifyAuthenticationStateChanged(Task.FromResult(authState));
                //}
            }
        }

        /// <summary>
        /// Metodo se encarga de renovar el token
        /// </summary>
        /// <param name="token">token anterior para la renovacion "Token refresh" ante que vensa</param>
        /// <returns></returns>
        private async Task<string> RenewToken(string token)
        {
            var claims = ParseClaimsFromJwt(token);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                claims.FirstOrDefault(s => s.Type.Equals("IdentifierT")).Value);
            var answer = await CallService.Get<string>("security/Session/RenewToken");
            return answer.Data;
        }

        /// <summary>
        /// El metodo se encarga de validar si expiro el tiempo del token.
        /// </summary>
        /// <param name="expirationTime">Tiempo de expiración</param>
        /// <returns>Retorna un bolean</returns>
        private static bool TokenMustBeRenewed(DateTime expirationTime, string timeExp)
        {
            var timeExpirationMinute = DateTime.Parse(timeExp).Minute;
            return expirationTime.Subtract(DateTime.Now) < TimeSpan.FromMinutes(timeExpirationMinute);
        }

        /// <summary>
        /// El metodo se encarga de validar el tiempo de expiración.
        /// </summary>
        /// <param name="expiryTime"></param>
        /// <returns>Retorna un bolean</returns>
        private static bool ExpiredToken(DateTime expiryTime)
        {
            return expiryTime <= DateTime.Now;
        }

        #endregion


        #region Realizado por Microsoft

        private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            List<Claim> claims = new();
            var keyValuePairs =
                JsonSerializer.Deserialize<Dictionary<string, object>>(ParseBase64WithoutPadding(jwt.Split('.')[1]));

            keyValuePairs.TryGetValue(ClaimTypes.Role, out var roles);

            if (roles != null)
            {
                if (roles.ToString().Trim().StartsWith("["))
                {
                    var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());

                    claims.AddRange(parsedRoles.Select(parsedRole => new Claim(ClaimTypes.Role, parsedRole)));
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
                }

                keyValuePairs.Remove(ClaimTypes.Role);
            }

            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

            return claims;
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2:
                    base64 += "==";
                    break;
                case 3:
                    base64 += "=";
                    break;
            }

            return Convert.FromBase64String(base64);
        }

        #endregion

        #region Login y Logout

        //Almacenar el token y la expiracion cuando se realiza la sesion de forma exitosa y notifica al blazor que se autentico
        public async Task LoginToken(string token)
        {
            var authState = await BuildAuthenticationStatus(token);
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }

        public async Task LogoutToken()
        {
            await Clean();
            NotifyAuthenticationStateChanged(Task.FromResult(Anonymous));
        }

        //Limpiar el local storage y borra el token del httpclient
        public async Task Clean()
        {
            await SessionStorageService.ClearAll();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        #endregion
    }

    /// <summary>
    /// Renovacion del token despues de cierto tiempo
    /// </summary>
    public class RenewTokenService : IDisposable
    {
        private Timer? _timer;
        private readonly IAuthenticationJWT _authenticationJwt;

        public RenewTokenService(IAuthenticationJWT authenticationJwt)
        {
            _authenticationJwt = authenticationJwt;
        }

        public void Start(int time)
        {
            _timer = new Timer
            {
                Interval = 1000 * 60 * time
            };
            _timer.Elapsed += Timer_Elapsed;
            _timer.Start();
        }

        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            _authenticationJwt.TokenRenewalManagement();
        }

        public void Dispose() => _timer?.Dispose();
    }
}