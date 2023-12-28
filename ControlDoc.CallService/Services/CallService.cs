using ControlDoc.Services.Models;
using System.Net;
using System.Net.Http.Json;
using ControlDoc.Services.Services;
using Microsoft.JSInterop;
using ControlDoc.Models.Enums;
using System.Net.Http.Headers;

namespace ControlDoc.Services.Interfaces
{
    public class CallService : ICallService
    {
        #region Atributos

        private static string Url { get; set; } = "";
        
        private SessionStorageService SessionStorageService { get; set; }
        private HttpClient _httpClient { get; set; }

        #endregion

        #region Constructor

        // public CallService(string url)
        // {
        //     Url = url;
        // }
        
        public CallService(HttpClient httpClient, IJSRuntime jSRuntime)
        {
            _httpClient = httpClient;
            SessionStorageService = new SessionStorageService(jSRuntime);
        }

        #endregion

        #region Metodos

        #region Gets

        /// <summary>
        /// Metodo GET que accede a los servicios de API
        /// </summary>
        /// <typeparam name="T">Modelo que responde el back.</typeparam>
        /// <param name="uri">Uri del servicio.</param>
        /// <param name="headers">Diccionario con la lista de cabecera.</param>
        /// <returns></returns>
        public async Task<HttpResponseWrapperModel<T>> Get<T>(string uri,
            Dictionary<string, dynamic>? headers = null)
        {
            try
            {
                await ValidationToken();                
                await AddHeaders(_httpClient, headers);
                var httpResonse = await _httpClient.GetAsync(uri);
                await RemoveHeaders(_httpClient, headers);

                if (httpResonse.StatusCode == HttpStatusCode.OK)
                {
                    var rta = await httpResonse.Content.ReadFromJsonAsync<HttpResponseWrapperModel<T>>();
                    return rta;
                }
                else
                {
                    return new HttpResponseWrapperModel<T>();
                }


                // var response = await httpClient.GetAsync(string.Concat(Url, uri));


                // if (response.StatusCode == HttpStatusCode.OK)
                // {
                //     var rta = await response.Content.ReadFromJsonAsync<HttpResponseWrapperModel<T>>();
                //     return rta;
                // }
                // else
                // {
                //     return new HttpResponseWrapperModel<T>();
                // }
            }
            catch (Exception ex)
            {
                return new HttpResponseWrapperModel<T>();
            }
        }

        #endregion

        #region Posts

        /// <summary>
        /// Metodo POST que accede a los servicios de API
        /// </summary>
        /// <typeparam name="T">Modelo que responde el back.</typeparam>
        /// <typeparam name="M">Modelo con la informacion cargada que se debe enviar al back.</typeparam>b
        /// <param name="uri">Uri del servicio.</param>
        /// <param name="headers">Diccionario con la lista de cabecera.</param>
        /// <returns></returns>
        public async Task<HttpResponseWrapperModel<T>> Post<T, M>(string uri, M shippingModel,
            Dictionary<string, dynamic>? headers = null)
        {
            try
            {
                await ValidationToken();
                await AddHeaders(_httpClient, headers);
                var httpResonse = await _httpClient.PostAsJsonAsync(uri, shippingModel);
                await RemoveHeaders(_httpClient, headers);

                if (httpResonse.StatusCode == HttpStatusCode.OK)
                {
                    return await httpResonse.Content.ReadFromJsonAsync<HttpResponseWrapperModel<T>>();
                }
                else
                {
                    return new HttpResponseWrapperModel<T>();
                }
            }
            catch (Exception)
            {
                return new HttpResponseWrapperModel<T>();
            }
        }

        #endregion

        #region Puts

        /// <summary>
        /// Metodo PUT que accede a los servicios de API.
        /// </summary>
        /// <typeparam name="T">Modelo que responde el back.</typeparam>
        /// <typeparam name="M">Modelo con la informacion cargada que se debe enviar al back.</typeparam>
        /// <param name="uri">Uri del servicio.</param>
        /// <param name="shippingModel">Modelo con la información cargada.</param>
        /// <param name="headers">Diccionario con la lista de cabecera.</param>
        /// <returns></returns>
        public async Task<HttpResponseWrapperModel<T>> Put<T, M>(string uri, M shippingModel,
            Dictionary<string, dynamic>? headers = null)
        {
            try
            {
                await ValidationToken();
                await AddHeaders(_httpClient, headers);
                using var httpResonse = await _httpClient.PutAsJsonAsync(uri, shippingModel);        
                await RemoveHeaders(_httpClient, headers);

                if (httpResonse.StatusCode == HttpStatusCode.OK)
                {
                    return await httpResonse.Content.ReadFromJsonAsync<HttpResponseWrapperModel<T>>();
                }
                else
                {
                    return new HttpResponseWrapperModel<T>();
                }
            }
            catch (Exception E)
            {
                return new HttpResponseWrapperModel<T>();
            }
        }

        #endregion

        #region Metodos Compartidos

        #region Token
        /// <summary>
        /// El metodo se encarga de validar la existencia del token y la asignacion en la cabecera.
        /// </summary>
        /// <returns></returns>
        public async Task ValidationToken()
        {
            if (!string.IsNullOrEmpty(await SessionStorageService.GetValue<string>(ValuesKeys.TokenAth)))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await SessionStorageService.GetValue<string>(ValuesKeys.TokenAth));
            }
        }
        #endregion

        private static Task RemoveHeaders(HttpClient httpClient, Dictionary<string, dynamic>? headers = null)
        {
            if (headers == null) return Task.CompletedTask;

            foreach (var header in headers)
            {
                httpClient.DefaultRequestHeaders.Remove(header.Key);
            }

            return Task.CompletedTask;
        }

        private static Task AddHeaders(HttpClient httpClient, Dictionary<string, dynamic>? headers = null)
        {
            if (headers == null) return Task.CompletedTask;

            foreach (var header in headers)
            {
                httpClient.DefaultRequestHeaders.Add(header.Key, header.Value.ToString());
            }

            return Task.CompletedTask;
        }

        #endregion

        #endregion
    }
}