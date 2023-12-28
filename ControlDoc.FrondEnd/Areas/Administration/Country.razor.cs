using ControlDoc.Models.Models.Administration.Response;
using ControlDoc.Models.Models.Pagination;
using ControlDoc.Services.Interfaces;
using ControlDoc.Services.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ControlDoc.FrondEnd.Areas.Administration
{
    public partial class Country
    {
        #region Variables
        #region Injects
        [Inject]
        private IJSRuntime Js { get; set; }

        [Inject] 
        ICallService CallService { get; set; }
        #endregion
        #endregion

        #region Initialization
        private List<CountryDtoResponse> PaisesList;
        [Parameter] public string IdModalIdentifier { get; set; }

        private int IdPaises { get; set; }
        private int IdDepartamento { get; set; }
        //private ModalPermisos ModalCrearOEditar;
        private CountryDtoResponse _selectedRecord;
        //private List<PermissionDtoResponse> FunctionList;

        private Meta meta;

       
        protected override async Task OnInitializedAsync()
        {
            PageLoadService.MostrarSpinnerReadLoad(Js);
            await GetCountry();
            PageLoadService.OcultarSpinnerReadLoad(Js);
        }
        #endregion
        #region Metodos

        private void HandleRecordSelectedState(StateDtoResponse selectedRecord)
        {
            //ModalCrearOEditar.UpdateSelectedRecord(selectedRecord);
        }
        //private async Task mostrarModal()
        //{
        //    ModalStatusPermisos = true;
        //    var PerfilId = IdPaises;

        //}

        //PAra el modal
        //private void HandleRecordSelected(CountryDtoResponse selectedRecord)
        //{
        //    ModalCrearOEditar.UpdateSelectedRecord(selectedRecord);
        //}


        private async Task GetCountry()
        {
            try
            {
                var response = await CallService.Get<List<CountryDtoResponse>>("location/Country/ByFilter");
                PaisesList = response.Data != null ? response.Data : new List<CountryDtoResponse>();

                if (PaisesList.Count > 0)
                {
                    meta = response.Meta;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el país: {ex.Message}");
            }
        }

        //Endpoint para mostrar en la grilla


        #endregion
    }
}
