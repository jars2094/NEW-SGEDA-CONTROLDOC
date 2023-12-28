using ControlDoc.Components.Components.Input;
using ControlDoc.Components.Components.Modals;
using ControlDoc.Models.Models.Administration.Request;
using ControlDoc.Models.Models.Administration.Response;
using ControlDoc.Models.Models.Generic;
using ControlDoc.Models.Models.Pagination;
using ControlDoc.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Net;
using System.Reflection.Metadata;

namespace ControlDoc.ComponentViews.ComponentViews.Administration
{
    public partial class ModalAddress
    {
        #region Variables
        #region Injects
        [Inject] 
        private NavigationManager NavigationManager { get; set; }

        [Inject] 
        private ICallService CallService { get; set; }
        #endregion

        #region Parameters

        [Parameter] public bool ModalStatus { get; set; } = false;
        [Parameter] public string Id { get; set; }
        [Parameter] public bool CrearEditar { get; set; }
        [Parameter] public EventCallback<bool> OnStatusChanged { get; set; }
        [Parameter] public EventCallback<AddressDtoRequest> OnModalSaved { get; set; }
        [Parameter] public EventCallback<int> OnIdSaved { get; set; }

        [Parameter] public bool multipleSelection { get; set; }

        [Parameter] public EventCallback<MyEventArgs<List<(string, AddressDtoRequest)>>> OnStatusChangedMultipleSelection { get; set; }

        #endregion

        private IJSRuntime Js { get; set; }

        //Request and Response
        private AddressDtoRequest addressDtoRequest = new AddressDtoRequest();

        private AddressDtoResponse? _selectedRecord;
        private AddressDtoResponse? addressDtoResponse;

        //Modal
        private ModalComponent? Modal { get; set; }

        //Listas de DropDownLists
        private List<SystemFieldsDtoResponse> cardinalityList;
        private List<SystemFieldsDtoResponse> houseClassList;
        private List<SystemFieldsDtoResponse> houseTypeList;
        private List<SystemFieldsDtoResponse> scTypeList;
        private List<CountryDtoResponse> countryList;
        private List<StateDtoResponse> stateList { get; set; }
        private List<CityDtoResponse> cityList;

        private bool EnabledDepartamento { get; set; } = true;
        private bool EnabledMunicipio { get; set; } = true;

        //Direccion
        private string address = "";

        //Inputs
        private int addressId;
        private string stType;

        private InputModalComponent stNumber;
        private InputModalComponent stLetter;
        private bool stBis = false;
        private InputModalComponent stComplement;
        private string stCardinality;
        private string crType;
        private InputModalComponent crNumber;
        private InputModalComponent crLetter;
        private bool crBis = false;
        private InputModalComponent crComplement;
        private string crCardinality;

        private string houseType;
        private string houseClass;
        private InputModalComponent houseNumber;

        private int country;
        private int state;
        private int city;

        //Meta
        private Meta meta;
        #endregion

        #region Initialized

        protected override async Task OnInitializedAsync()
        {
            ModalStatus = false;
            await GetCardinality();
            await GetHouseClass();
            await GetHouseType();
            await GetSCType();
            await GetCountry();
        }

        private string textStCardinality = "Seleccione una Cardinalidad...";
        private string textCrCardinality = "Seleccione una Cardinalidad...";
        private string textStType = "Seleccione un tipo de Via...";
        private string textCrType = "Seleccione un tipo de Via...";
        private string textHouseType = "Seleccione un tipo de Casa...";
        private string textHouseClass = "Seleccione una clase de Casa...";

        private string textCountry = "Seleccione un Pais...";
        private string textState = "Seleccione un Departamento...";
        private string textCity = "Seleccione un Municipio...";

        public async Task RecibirRegistrosAsync(AddressDtoResponse response)
        {
            _selectedRecord = response;
            
            addressDtoRequest.StNumber = _selectedRecord.StNumber;
            addressDtoRequest.StType = _selectedRecord.StType;
            addressDtoRequest.StLetter = _selectedRecord.StLetter;
            addressDtoRequest.StBis = _selectedRecord.StBis;
            addressDtoRequest.StComplement = _selectedRecord.StComplement;
            addressDtoRequest.CrNumber = _selectedRecord.CrNumber;
            addressDtoRequest.CrType = _selectedRecord.CrType;
            addressDtoRequest.CrLetter = _selectedRecord.CrLetter;
            addressDtoRequest.CrBis = _selectedRecord.CrBis;
            addressDtoRequest.CrComplement = _selectedRecord.CrComplement;
            addressDtoRequest.HouseType = _selectedRecord.HouseType;
            addressDtoRequest.HouseClass = _selectedRecord.HouseClass;
            addressDtoRequest.HouseNumber = _selectedRecord.HouseNumber;

            stBis = addressDtoRequest.StBis;
            crBis = addressDtoRequest.CrBis;

            // dropdown StCardinality
            textStCardinality = valueCardinality(_selectedRecord.StCardinality);
            stCardinality = _selectedRecord.StCardinality;

            // dropdown CrCardinality
            textCrCardinality = valueCardinality(_selectedRecord.CrCardinality);
            crCardinality = _selectedRecord.CrCardinality;

            //dropdown StType
            textStType = valueStType(_selectedRecord.StType);
            stType = _selectedRecord.StType;

            //dropdown CrType
            textCrType = valueCrType(_selectedRecord.CrType);
            crType = _selectedRecord.CrType;

            //dropdown HouseType
            textHouseType = valueHouseType(_selectedRecord.HouseType);
            houseType = _selectedRecord.HouseType;

            //dropdown HouseCLass
            textHouseClass = valueHouseClass(_selectedRecord.HouseClass);
            houseClass = _selectedRecord.HouseClass;

            //dropdown Country
            textCountry = nameCountry(_selectedRecord.CountryId);
            addressDtoRequest.CountryId = _selectedRecord.CountryId;
            country = _selectedRecord.CountryId;

            //dropdown State
            state = _selectedRecord.StateId;
            addressDtoRequest.StateId = _selectedRecord.StateId;
            textState = await nameState(_selectedRecord.StateId);

            //dropdown City
            city = _selectedRecord.CityId;
            addressDtoRequest.CityId = _selectedRecord.CityId;
            textCity = await nameCity(_selectedRecord.CityId);

            ActualizarDireccion();
            StateHasChanged();

        }

        private async Task GetInfoAsync()
        {
            try
            {
                Dictionary<string, dynamic> headers = new()
                {
                    { "IdAddress", addressId}
                };

                var response = await CallService.Get<AddressDtoResponse>("administration/Address/ByFilterId", headers);

                addressDtoResponse = response.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los parámetros: {ex.Message}");
            }
        }

        #endregion Initialized

        #region FormMethods

        public async Task ResetFormAsync()
        {
            addressDtoRequest = new AddressDtoRequest();
            address = "";
            stBis = false;
            crBis = false;
            textStCardinality = "Seleccione una Cardinalidad...";
            stCardinality = "";
            textCrCardinality = "Seleccione una Cardinalidad...";
            crCardinality = "";
            textStType = "Seleccione un tipo de Via...";
            stType = "";
            textCrType = "Seleccione un tipo de Via...";
            crType = "";
            textHouseType = "Seleccione un tipo de Casa...";
            houseType = "";
            textHouseClass = "Seleccione una clase de Casa...";
            houseClass = "";
            textCountry = "Seleccione un Pais...";
            country = 0;
            textState = "Seleccione un Departamento...";
            state = 0;
            textCity = "Seleccione un Departamento...";
            city = 0;
        }
        private async Task HandleValidSubmit()
        {          
            await Create();
            StateHasChanged();
        }

        private async Task<List<(string, AddressDtoRequest)>> Create()
        {
            try
            {
                var AddressDtoRequest = new AddressDtoRequest
                {
                    CountryId = country,
                    StateId = state,
                    CityId = city,
                    StType = codeStType(stType),
                    StNumber = stNumber.InputValue,
                    StLetter = stLetter.InputValue,
                    StBis = stBis,
                    StComplement = stComplement.InputValue,
                    StCardinality = codeStCardinality(stCardinality),
                    CrType = codeCrType(crType),
                    CrNumber = crNumber.InputValue,
                    CrLetter = crLetter.InputValue,
                    CrBis = crBis,
                    CrComplement = crComplement.InputValue,
                    CrCardinality = codeCrCardinality(crCardinality),
                    HouseType = codeHouseType(houseType),
                    HouseClass = codeHouseClass(houseClass),
                    HouseNumber = houseNumber.InputValue
                };

                // Devuelve el mensaje y el objeto AddressDtoRequest
                List<(string, AddressDtoRequest)> result = new List<(string, AddressDtoRequest)>
                {
                    (address, AddressDtoRequest),
                };


                return result;

            }
            catch (Exception ex)
            {
                List<(string, AddressDtoRequest)> result = new List<(string, AddressDtoRequest)>
                {
                    ("Error al guardar", null),
                };

                return result;
            }
        }

        private async Task HandleModalClosed(bool status)
        {
             var createResult = await Create();

             var eventArgs = new MyEventArgs<List<(string, AddressDtoRequest)>>
             {
                 Data = createResult,
                 ModalStatus = status
             };
             await OnStatusChangedMultipleSelection.InvokeAsync(eventArgs);
            
        }

        private void Save()
        {
            HandleModalClosed(false);
        }

        #endregion FormMethods

        #region ModalMethods

        private ModalNotificationsComponent notificationModal;

        //metodo para abrir nueva modal dentro de otra modal
        private async Task OpenNewModal()
        {
            await OnStatusChanged.InvokeAsync(true);
        }

        public async Task resetForm()
        {
            addressDtoRequest = new AddressDtoRequest();
            await OpenNewModal();
        }

        public void UpdateModalStatus(bool newValue)
        {
            ModalStatus = newValue;
            StateHasChanged();
        }

        public async Task UpdateModalIdAsync(int id)
        {
            addressId = id;
            await GetInfoAsync();
            RecibirRegistrosAsync(addressDtoResponse);
            StateHasChanged();
        }

        public async Task ResetForm()
        {
            await ResetFormAsync();
            StateHasChanged();
        }

        #endregion ModalMethods

        #region NotificationModal

        private void HandleModalNotiClose(ModalClosedEventArgs args)
        {
            if (notificationModal.Type == ModalNotificationsComponent.ModalType.Success)
            {
                UpdateModalStatus(args.ModalStatus);
            }
        }

        #endregion NotificationModal

        #region ReloadPage

        private void RecargarPagina()
        {
            NavigationManager.NavigateTo(NavigationManager.Uri, true);
        }

        #endregion ReloadPage

        #region GetsDropdownList

        private async Task GetCountry()
        {
            try
            {
                var response = await CallService.Get<List<CountryDtoResponse>>("location/Country/ByFilter", null);
                countryList = response.Data != null ? response.Data : new List<CountryDtoResponse>();
                
                if (countryList.Count > 0)
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
                Console.WriteLine($"Error al obtener los parámetros: {ex.Message}");
            }
        }
        private async Task GetState()
        {
            try
            {
                

                if (country > 0)
                {
                    EnabledDepartamento = true;
                    EnabledMunicipio = false;

                    Dictionary<string, dynamic> headers = new()
                    {
                         { "countryId",country }
                    };

                    var response = await CallService.Get<List<StateDtoResponse>>("location/State/ByFilter", headers);
                    stateList = response.Data != null ? response.Data : new List<StateDtoResponse>();

                    if (stateList.Count > 0)
                    {
                        meta = response.Meta;
                        state = 0;
                    }
                    else
                    {
                        EnabledDepartamento = false;
                        EnabledMunicipio = false;
                    }

                }
                else
                {
                    state = 0;
                    city = 0;
                    EnabledDepartamento = false;
                    EnabledMunicipio = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los parámetros: {ex.Message}");
            }
        }
        private async Task GetCity()
        {
            try
            {
                if (state > 0)
                {
                    EnabledMunicipio = true;

                    Dictionary<string, dynamic> headers = new()
                    {
                         { "stateId",state }
                    };

                    var response = await CallService.Get<List<CityDtoResponse>>("location/City/ByFilter", headers);
                    cityList = response.Data != null ? response.Data : new List<CityDtoResponse>();

                    if (cityList.Count > 0)
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
                    city = 0;
                    EnabledMunicipio = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los parámetros: {ex.Message}");
            }
        }

        private async Task GetCardinality()
        {
            try
            {
                Dictionary<string, dynamic> headers1 = new()
                {
                    { "ParamCode", "CARD" }
                };

                var response = await CallService.Get<List<SystemFieldsDtoResponse>>("params/SystemFields/ByFilter", headers1);

                cardinalityList = response.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los parámetros: {ex.Message}");
            }
        }
        private async Task GetHouseClass()
        {
            try
            {
                Dictionary<string, dynamic> headers1 = new()
                {
                    { "ParamCode", "HC" }
                };

                var response = await CallService.Get<List<SystemFieldsDtoResponse>>("params/SystemFields/ByFilter", headers1);

                houseClassList = response.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los parámetros: {ex.Message}");
            }
        }
        private async Task GetHouseType()
        {
            try
            {
                Dictionary<string, dynamic> headers = new()
                {
                    { "ParamCode", "HT" }
                };

                var response = await CallService.Get<List<SystemFieldsDtoResponse>>("params/SystemFields/ByFilter", headers);

                houseTypeList = response.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los parámetros: {ex.Message}");
            }
        }
        private async Task GetSCType()
        {
            try
            {
                Dictionary<string, dynamic> headers = new()
                {
                    { "ParamCode", "SCT" }
                };

                var response = await CallService.Get<List<SystemFieldsDtoResponse>>("params/SystemFields/ByFilter", headers);

                scTypeList = response.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los parámetros: {ex.Message}");
            }
        }

        #endregion GetsDropdownList

        #region Address

        private void ActualizarDireccion()
        {
            
            var stnumber = stNumber.InputValue;
            var stletter = stLetter.InputValue;
            var stcomplement = stComplement.InputValue;
            var crnumber = crNumber.InputValue;
            var crletter = crLetter.InputValue;
            var crcomplement = crComplement.InputValue;
            var housenumbert = houseNumber.InputValue;

            var stcardinality = valueCardinality(stCardinality);
            var crcardinality = valueCardinality(crCardinality);
            var sct = valueStType(stType);
            var crt = valueCrType(crType);
            var hst = valueHouseType(houseType);
            var hsc = valueHouseClass(houseClass);

            address = $"{sct} {stnumber} {stletter} {(stBis ? "Bis" : "")} {stcomplement} {stcardinality} {crt} {crnumber} {crletter} {(crBis ? "Bis" : "")} {crcomplement} {crcardinality} {hst} {hsc} {housenumbert}";
            StateHasChanged();
            

            
        }

        #endregion Address

        #region valueSelected

        //Metodo para tomar el Value de StType, CrType, HouseType y HouseClass
        private string GetValueFromList(List<SystemFieldsDtoResponse> itemList, string selectedValue, string systemParamCode)
        {
            var systemparam = "";
            var code = "";

            // Verificar si selectedValue es nulo o vacío
            if (string.IsNullOrEmpty(selectedValue))
            {
                return "";
            }

            if (selectedValue.Contains(","))
            {
                string[] partes = selectedValue.Split(',');
                (systemparam, code) = (partes[0].Trim(), partes[1].Trim());
            }
            else
            {
                code = selectedValue.Trim();
                systemparam = systemParamCode;
            }

            var selectedDataItem = itemList.FirstOrDefault(item => item.code == code && item.systemParam.paramCode == systemparam);
            return selectedDataItem?.value ?? "";
        }

        private string valueCardinality(string selectedValue)
        {
            return GetValueFromList(cardinalityList, selectedValue, "CARD");
        }
        private string valueStType(string selectedValue)
        {
            return GetValueFromList(scTypeList, selectedValue, "SCT");
        }
        private string valueCrType(string selectedValue)
        {
            return GetValueFromList(scTypeList, selectedValue, "SCT");
        }
        private string valueHouseType(string selectedValue)
        {
            return GetValueFromList(houseTypeList, selectedValue, "HT");
        }
        private string valueHouseClass(string selectedValue)
        {
            return GetValueFromList(houseClassList, selectedValue, "HC");
        }


        //Metodo para tomar el Code de StType, CrType, HouseType y HouseClass
        private string GetCodeFromList(List<SystemFieldsDtoResponse> itemList, string selectedValue, string systemParamCode)
        {
            var selectedDataItem="";
            // Verificar si selectedValue es nulo o vacío
            if (string.IsNullOrEmpty(selectedValue))
            {
                return "";
            }

            if (selectedValue.Contains(","))
            {
                selectedDataItem = selectedValue;
            }
            else
            {
                selectedDataItem = $"{systemParamCode},{selectedValue}";
            }

            
            return selectedDataItem ?? "";
        }

        private string codeStType(string selectedValue)
        {
            return GetCodeFromList(scTypeList, selectedValue, "SCT");
        }
        private string codeCrType(string selectedValue)
        {
            return GetCodeFromList(scTypeList, selectedValue, "SCT");
        }
        private string codeHouseType(string selectedValue)
        {
            return GetCodeFromList(houseTypeList, selectedValue, "HT");
        }
        private string codeHouseClass(string selectedValue)
        {
            return GetCodeFromList(houseClassList, selectedValue, "HC");
        }
        private string codeStCardinality(string selectedValue)
        {
            return GetCodeFromList(scTypeList, selectedValue, "CARD");
        }
        private string codeCrCardinality(string selectedValue)
        {
            return GetCodeFromList(scTypeList, selectedValue, "CARD");
        }

        //Metodos para asignar los nombres de Country, State y City de la informacion a editar
        private string nameCountry(int selectedValue)
        {
            // Dividir el valor en dos partes y asignarlas directamente
            var text = "";

            var selectedDataItem = countryList.FirstOrDefault(item => item.countryId == selectedValue);
            text = selectedDataItem.name;
            
            return text;
        }
        private async Task<string> nameState(int selectedValue)
        {
            var stateList2 = await GetStateSelected(country);
            var selectedDataItem = stateList2.FirstOrDefault(item => item.stateId == selectedValue);
            var text = selectedDataItem.name;

            return text;
        }
        private async Task<string> nameCity(int selectedValue)
        {
            var cityList2 = await GetCitySelected(state);
            var selectedDataItem = cityList2.FirstOrDefault(item => item.cityId == selectedValue);
            var text = selectedDataItem.name;

            return text;
        }


        private async Task<List<StateDtoResponse>> GetStateSelected(int country)
        {
            try
            {
                Dictionary<string, dynamic> headers = new()
                {
                    { "countryId", country}
                };

                var response = await CallService.Get<List<StateDtoResponse>>("location/State/ByFilter", headers);

                stateList = response.Data;
                return stateList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los parámetros: {ex.Message}");
                return null;
            }
        }
        private async Task<List<CityDtoResponse>> GetCitySelected(int state)
        {
            try
            {
                Dictionary<string, dynamic> headers = new()
                {
                    { "stateId", state }
                };

                var response = await CallService.Get<List<CityDtoResponse>>("location/City/ByFilter", headers);

                cityList = response.Data;
                return cityList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los parámetros: {ex.Message}");
                return null;
            }
        }

        

        #endregion valueSelected
    }
}