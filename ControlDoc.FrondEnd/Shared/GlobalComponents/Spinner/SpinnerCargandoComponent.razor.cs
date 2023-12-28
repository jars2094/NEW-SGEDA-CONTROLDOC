using ControlDoc.Services.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ControlDoc.FrondEnd.Shared.GlobalComponents.Spinner
{
    public partial class SpinnerCargandoComponent
    {
        [Parameter] public bool Loading { get; set; } = false;

        #region Inject
        [Inject] private IJSRuntime JS { get; set; }
        #endregion
        
        #region Methods
        private void CerrarPageLoad()
        {
            PageLoadService.OcultarSpinnerReadLoad(JS);
        }

        public void UpdateLoadingStatus(bool newValue)
        {
            Loading = newValue;
        }
        #endregion
    }
}
