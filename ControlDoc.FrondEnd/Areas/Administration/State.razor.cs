using ControlDoc.Models.Models.Administration.Response;
using ControlDoc.Models.Models.Pagination;
using ControlDoc.Services.Interfaces;
using ControlDoc.Services.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ControlDoc.FrondEnd.Areas.Administration
{
    public partial class State
    {
        #region Variables
        #region Injects
        [Inject]
        private IJSRuntime Js { get; set; }

        [Inject] 
        private ICallService CallService { get; set; }
        #endregion
        #endregion

        private bool ModalStatusPermisos;

        #region Initialization
        private List<CountryDtoResponse> PaisesList;
        private List<StateDtoResponse> DepartamentosList;
        [Parameter] public string IdModalIdentifier { get; set; }

        private int IdPaises { get; set; }
        private int IdDepartamento { get; set; }
        //private ModalPermisos ModalCrearOEditar;
        private CountryDtoResponse _selectedRecord;
        //private List<PermissionDtoResponse> FunctionList;

        private Meta meta;

        private void HandleRecordSelectedState(StateDtoResponse selectedRecord)
        {
            //ModalCrearOEditar.UpdateSelectedRecord(selectedRecord);
        }
        protected override async Task OnInitializedAsync()
        {
            ModalStatusPermisos = false;
            PageLoadService.MostrarSpinnerReadLoad(Js);
            await GetCountry();
            PageLoadService.OcultarSpinnerReadLoad(Js);
        }
        #endregion

        #region Metodos


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
        private async Task GetState()
        {
            try
            {
                if (IdPaises > 0)
                {
                    

                    Dictionary<string, dynamic> headers = new()
                    {
                         { "countryId",IdPaises }
                    };

                    var response = await CallService.Get<List<StateDtoResponse>>("location/State/ByFilter", headers);
                    DepartamentosList = response.Data != null ? response.Data : new List<StateDtoResponse>();

                    if (DepartamentosList.Count > 0)
                    {
                        meta = response.Meta;
                        IdDepartamento = 0;
                    }
                    

                }
                else
                {
                    IdDepartamento = 0;
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error al obtener el departamento: {ex.Message}");
            }
        }


        #endregion
    }
}
