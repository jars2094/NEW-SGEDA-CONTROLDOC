using ControlDoc.Services.Models;

namespace ControlDoc.Services.Interfaces;

public interface ICallService
{
    Task<HttpResponseWrapperModel<T>> Get<T>(string uri, Dictionary<string, dynamic>? headers = null);

    Task<HttpResponseWrapperModel<T>> Post<T, M>(string uri, M shippingModel,
        Dictionary<string, dynamic>? headers = null);

    Task<HttpResponseWrapperModel<T>> Put<T, M>(string uri, M shippingModel,
        Dictionary<string, dynamic>? headers = null);
}