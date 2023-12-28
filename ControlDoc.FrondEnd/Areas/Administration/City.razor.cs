using ControlDoc.Models.Models.Administration.Response;
using ControlDoc.Models.Models.Pagination;
using ControlDoc.Services.Interfaces;
using ControlDoc.Services.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ControlDoc.FrondEnd.Areas.Administration
{
    public partial class City
    {


        [Inject]
        private IJSRuntime Js { get; set; }

        [Inject] private ICallService CallService { get; set; }

        private bool ModalStatusPermisos;
        #region Initialization
        private List<CountryDtoResponse>? PaisesList;
        private List<StateDtoResponse>? DepartamentosList;
        private List<CityDtoResponse>? CiudadList;
        [Parameter] public string IdModalIdentifier { get; set; }

        private int IdPaises { get; set; }
        private bool EnabledDepartamento { get; set; } = true;
        private bool EnabledMunicipio { get; set; } = true;
        private int IdDepartamento { get; set; }
        private int IdCiudad { get; set; }
        //private ModalPermisos ModalCrearOEditar;
        private CountryDtoResponse _selectedRecord;
        //private List<PermissionDtoResponse> FunctionList;

        private Meta meta;

        
        protected override async Task OnInitializedAsync()
        {
            ModalStatusPermisos = false;
            PageLoadService.MostrarSpinnerReadLoad(Js);
            await GetCountry();
            PageLoadService.OcultarSpinnerReadLoad(Js);
        }
        #endregion
        #region Metodos


        private void HandleRecordSelectedCity(CityDtoResponse selectedRecord)
        {
            //ModalCrearOEditar.UpdateSelectedRecord(selectedRecord);
        }

        //private async Task mostrarModal()
        //{
        //    ModalStatusPermisos = true;
        //    var PerfilId = IdPaises;

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
                    EnabledDepartamento = false;
                    EnabledMunicipio = false;
                }
                else
                {
                    EnabledDepartamento = false;
                    EnabledMunicipio = false;
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
                    EnabledDepartamento = true;
                    EnabledMunicipio = false;

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
                    else
                    {
                        EnabledDepartamento = false;
                        EnabledMunicipio = false;
                    }

                }
                else
                {
                    IdDepartamento = 0;
                    IdCiudad = 0;
                    EnabledDepartamento = false;
                    EnabledMunicipio = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el departamento: {ex.Message}");
            }
        }
        private async Task GetCity()
        {
            try
            {
                if (IdDepartamento > 0)
                {
                    EnabledMunicipio = true;

                    Dictionary<string, dynamic> headers = new()
                    {
                         { "stateId",IdDepartamento }
                    };

                    var response = await CallService.Get<List<CityDtoResponse>>("location/City/ByFilter", headers);
                    CiudadList = response.Data != null ? response.Data : new List<CityDtoResponse>();

                    if (CiudadList.Count > 0)
                    {
                        meta = response.Meta;
                    }
                    else
                    {
                        EnabledMunicipio = false;
                    }
                }
                else
                {
                    IdDepartamento = 0;
                    EnabledMunicipio = false;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el municipio: {ex.Message}");
            }
        }


        #endregion
    }
}