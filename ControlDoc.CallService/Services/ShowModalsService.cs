using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDoc.Services.Services
{
    public class ShowModalsService
    {
        #region Manejo de Mostrar/Ocultar Modal
        public static async void ShowModal(IJSRuntime js, string elementId)
        {
            await js.InvokeVoidAsync("ShowModalComponent", elementId);
        }

        public static async void hideModal(IJSRuntime js, string elementId)
        {
            await js.InvokeVoidAsync("HideModelComponent", elementId);
        }
        #endregion
    }
}
