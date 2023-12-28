using Microsoft.JSInterop;

namespace ControlDoc.Services.Services
{
    public class PageLoadService
    {
        #region Manejo de mostrar/ocultar spinnerloading
        public static async void MostrarSpinnerReadLoad(IJSRuntime js)
        {
            await js.InvokeVoidAsync("MostrarLoadSpinner");
        }

        public static async void OcultarSpinnerReadLoad(IJSRuntime js)
        {
            await js.InvokeVoidAsync("OcultarLoadSpinner");
        }
        #endregion
    }
}
