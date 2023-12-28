using ControlDoc.Services.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ControlDoc.Components.Components.Spinner
{
    public partial class SpinnerCargandoComponent
    {
        #region Inject
        [Inject] private IJSRuntime JS { get; set; }
        #endregion

        #region Methods
        private void CerrarPageLoad()
        {
            PageLoadService.OcultarSpinnerReadLoad(JS);
        }
        #endregion
    }
}
