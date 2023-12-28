using ControlDoc.Components.Components.Input;
using ControlDoc.Components.Components.Modals;
using ControlDoc.Models.Models.Administration.Request;
using ControlDoc.Models.Models.Administration.Response;
using ControlDoc.Models.Models.Generic;
using ControlDoc.Services.Interfaces;
using ControlDoc.Services.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Telerik.SvgIcons;

namespace ControlDoc.ComponentViews.ComponentViews.Administration
{
    public partial class ModalThirdParty:ComponentBase
    {

        #region Variables
        #region Injects
        [Inject] 
        private NavigationManager NavigationManager { get; set; }

        [Inject] 
        private ICallService CallService { get; set; }
        #endregion

        #region Parameters
        [Parameter] 
        public bool ModalStatus { get; set; } = false;

        [Parameter] 
        public string Id { get; set; }

        [Parameter] 
        public bool CrearEditar { get; set; }

        [Parameter] 
        public EventCallback<bool> OnStatusChanged { get; set; }

        [Parameter] 
        public EventCallback<bool> OnChangeData { get; set; }

        [Parameter] 
        public EventCallback<string> OnPersonType { get; set; }

        [Parameter]
        public string PersonType { get; set; }

        [Parameter] 
        public EventCallback<int> OnIdSaved { get; set; }

        [Parameter] 
        public EventCallback OnResetForm { get; set; }
        #endregion

        //Request and Response
        private ThirdPartyDtoResponse _selectedRecord;
        private ThirdPartyDtoRequest thirdPartyDtoRequest = new ThirdPartyDtoRequest();
        private ThirdPartyUpdateDtoRequest thirdPartyUpdateDtoRequest = new ThirdPartyUpdateDtoRequest();
        private ThirdPartyDtoResponse thirdPartyDtoResponse = new ThirdPartyDtoResponse();
        private ProfileUsersDtoRequestUpdate profileUsersDtoRequestUpdate = new ProfileUsersDtoRequestUpdate();
        private AddressDtoRequest addressDtoRequest;
        private AddressDtoResponse addressDtoResponse;
        private string textAddress = "";

        //Modal
        private ModalComponent? Modal { get; set; }

        //Listas
        private List<SystemFieldsDtoResponse> identificationTypeList;
        private List<SystemFieldsDtoResponse> chargeList;
        private List<SystemFieldsDtoResponse> natureList;

        //Inputs
        private InputModalComponent thirdPartyId { get; set; }
        private string personType { get; set; }
        private bool lastNameActive { get; set; }
        private int company;

        private string identificationType;
        private InputModalComponent identification { get; set; }
        private bool activeState = true;
        private InputModalComponent names { get; set; }
        private InputModalComponent login { get; set; }
        private InputModalComponent password { get; set; }

        private int addressId;

        //ThirData
        private InputModalComponent email1 { get; set; }
        private InputModalComponent email2 { get; set; }
        private InputModalComponent webpage { get; set; }
        private InputModalComponent firstAndMiddleName { get; set; }
        private InputModalComponent lastNames { get; set; }
        private string charge;
        private InputModalComponent initials { get; set; }
        private string nature { get; set; }
        private InputModalComponent phone1 { get; set; }
        private InputModalComponent phone2 { get; set; }

        //Create or Edit
        private bool IsEditForm = false;
        private string IdThirdParty;

        #endregion

        #region OnInitialized
        protected override async Task OnInitializedAsync()
        {
            ModalStatus = false;
            await GetIdentificationType();
            await GetCharge();
            await GetNature();
        }
        #endregion OnInitialized
        
        #region FormMethods

        private async Task HandleValidSubmit()
        {
            // Lógica de envío del formulario
            if (IsEditForm)
            {
                await Update();
                IsEditForm=false;
            }
            else
            {
                await Create();
            }

            StateHasChanged();
        }

        public static (string firstName, string middleName) SplitFirstAndMiddleName(string fullName)
        {
            string[] nameParts = fullName.Trim().Split(' ');

            string firstName = nameParts[0];
            string middleName = nameParts.Length > 1 ? nameParts[1] : string.Empty;

            return (firstName, middleName);
        }

        private async Task Create()
        {
            thirdPartyDtoRequest.PersonType = personType;
            thirdPartyDtoRequest.Address = addressDtoRequest;

            //TODO: Asignar la compañia del usuario
            thirdPartyDtoRequest.CompanyId = 16;

            thirdPartyDtoRequest.IdentificationType = codeIdentification(identificationType);
            thirdPartyDtoRequest.IdentificationNumber = identification.InputValue;
            thirdPartyDtoRequest.ActiveState = activeState;
            thirdPartyDtoRequest.Names = names.InputValue;
            thirdPartyDtoRequest.Login = "";
            thirdPartyDtoRequest.Password = "";

            // ThirData
            if (personType == "PJ")
            {
                thirdPartyDtoRequest.Names = names.InputValue;
                thirdPartyDtoRequest.FirstName = "";
                thirdPartyDtoRequest.MiddleName = "";
                thirdPartyDtoRequest.LastName = "";
                thirdPartyDtoRequest.Initials = initials.InputValue;
            }
            else
            {
                var firstAndMiddleNameValue = names.InputValue ?? string.Empty;
                (string firstName, string middleName) = SplitFirstAndMiddleName(firstAndMiddleNameValue);
                thirdPartyDtoRequest.FirstName = firstName;
                thirdPartyDtoRequest.MiddleName = middleName;
                thirdPartyDtoRequest.LastName = lastNames.InputValue;
                thirdPartyDtoRequest.Names = $"{firstName} {middleName} {lastNames.InputValue}";
                thirdPartyDtoRequest.Initials = "";
            }
            
            
            thirdPartyDtoRequest.Email1 = email1.InputValue;
            thirdPartyDtoRequest.Email2 = email2.InputValue == null ? "" : email2.InputValue;
            thirdPartyDtoRequest.WebPage = webpage.InputValue;
            thirdPartyDtoRequest.ChargeCode = codeCharge(charge);
            thirdPartyDtoRequest.NatureCode = codeNature(nature);
            thirdPartyDtoRequest.Phone1 = phone1.InputValue;
            thirdPartyDtoRequest.Phone2 = phone2.InputValue==null ? "":phone2.InputValue;


            try
            {
                var response = await CallService.Post<ThirdPartyDtoResponse, ThirdPartyDtoRequest>("administration/ThirdParty/CreateThirdParty", thirdPartyDtoRequest);
                if (response.Succeeded)
                {
                    notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Success, "se creo el registro exitosamente", "aceptar", true);
                    await OnChangeData.InvokeAsync(true);
                }
                else
                {
                    notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Error, "No se creo el registro exitosamente", "aceptar", true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear un Tercero {ex.Message}");
            }
        }

        private async Task Update()
        {
            await GetAddressAsync(addressId);
            var addressDtoRequestOld = fillAddress(addressDtoResponse);
            var empty = new AddressDtoRequest();
            thirdPartyUpdateDtoRequest.Address = (addressDtoRequest == null || addressDtoRequest == empty || addressDtoRequest.CountryId == 0) ? addressDtoRequestOld:addressDtoRequest;

            thirdPartyUpdateDtoRequest.IdentificationType = codeIdentification(identificationType);
            thirdPartyUpdateDtoRequest.IdentificationNumber = identification.InputValue;
            thirdPartyUpdateDtoRequest.Names = names.InputValue;
            thirdPartyUpdateDtoRequest.Login = "";
            thirdPartyUpdateDtoRequest.Password = "";
            thirdPartyUpdateDtoRequest.ActiveState = activeState;

            // ThirData
            if (personType == "PJ")
            {
                thirdPartyUpdateDtoRequest.Names = names.InputValue;
                thirdPartyUpdateDtoRequest.FirstName = "";
                thirdPartyUpdateDtoRequest.MiddleName = "";
                thirdPartyUpdateDtoRequest.LastName = "";
                thirdPartyUpdateDtoRequest.Initials = initials.InputValue;
                thirdPartyUpdateDtoRequest.Email2 = email2.InputValue;
            }
            else
            {
                var firstAndMiddleNameValue = names.InputValue ?? string.Empty;
                (string firstName, string middleName) = SplitFirstAndMiddleName(firstAndMiddleNameValue);
                thirdPartyUpdateDtoRequest.FirstName = firstName;
                thirdPartyUpdateDtoRequest.MiddleName = middleName;
                thirdPartyUpdateDtoRequest.LastName = lastNames.InputValue;
                thirdPartyUpdateDtoRequest.Names = $"{firstName} {middleName}";
                thirdPartyUpdateDtoRequest.Initials = "";
                thirdPartyUpdateDtoRequest.Email2 = "";
            }

            thirdPartyUpdateDtoRequest.Email1 = email1.InputValue;
            thirdPartyUpdateDtoRequest.WebPage = webpage.InputValue;
            thirdPartyUpdateDtoRequest.ChargeCode = codeCharge(charge);
            thirdPartyUpdateDtoRequest.NatureCode = codeNature(nature);
            thirdPartyUpdateDtoRequest.Phone1 = phone1.InputValue;
            thirdPartyUpdateDtoRequest.Phone2 = phone2.InputValue;

            Dictionary<string, dynamic> headers = new() { { "thirdPartyIdToUpdate", thirdPartyId.InputValue } };

            try
            {
                var response = await CallService.Put<ThirdPartyDtoResponse, ThirdPartyUpdateDtoRequest>("administration/ThirdParty/UpdateThirdParty", thirdPartyUpdateDtoRequest, headers);
                if (response.Succeeded)
                {

                    notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Success, "se actualizo exitosamente el registro", "Aceptar", true);
                    await OnChangeData.InvokeAsync(true);
                }
                else
                {
                    notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Error, "No se actualizo el registro, intente de nuevo", "Aceptar", true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar un Tercero {ex.Message}");
            }
        }

        #endregion FormMethods

        #region ModalMethods

        private ModalNotificationsComponent notificationModal;

        //metodo para abrir nueva modal dentro de otra modal
        private async Task OpenNewModal()
        {
            await OnStatusChanged.InvokeAsync(true);
            if (_selectedRecord != null && addressId!=0)
            {
                await OnIdSaved.InvokeAsync(_selectedRecord.AddressId);
            }
        }

        public async Task ResetFormAsync()
        {
            thirdPartyDtoRequest = new ThirdPartyDtoRequest();
            thirdPartyUpdateDtoRequest = new ThirdPartyUpdateDtoRequest();
            thirdPartyDtoResponse = new ThirdPartyDtoResponse();
            addressDtoRequest = new AddressDtoRequest();
            addressId = 0;
            textAddress = "";
            IdThirdParty = "";
            personType = "";
            identificationType = "";
            charge = "";
            nature = "";
            textIdentificationType = "Seleccione un tipo de identificación...";
            textCharge = "Seleccione un Cargo...";
            textNature = "Seleccione una Naturaleza...";
            activeState = true;
            StateHasChanged();
        }

        public void UpdateModalStatus(bool newValue)
        {
            ModalStatus = newValue;
            StateHasChanged();
        }

        public void selectPersonType(int persontype)
        {
            personType = (persontype == 0) ? "PN" : "PJ";
            lastNameActive = (persontype == 0) ? true : false;
            GetIdentificationType();
            StateHasChanged();
        }

        private void HandleModalClosed(bool status)
        {
            ModalStatus = status;
            OnResetForm.InvokeAsync();
            ResetFormAsync();
            StateHasChanged();
        }

        private string textIdentificationType = "Seleccione un tipo de identificación...";
        private string textCharge = "Seleccione un Cargo...";
        private string textNature = "Seleccione una Naturaleza...";
        public void RecibirRegistro(ThirdPartyDtoResponse response)
        {
            _selectedRecord = response;

            IdThirdParty = _selectedRecord.ThirdPartyId.ToString();
            addressId = _selectedRecord.AddressId;
            activeState = _selectedRecord.ActiveState;
            thirdPartyDtoResponse.IdentificationNumber = _selectedRecord.IdentificationNumber;
            thirdPartyDtoResponse.Names = _selectedRecord.Names;
            thirdPartyDtoResponse.ActiveState = _selectedRecord.ActiveState;
            thirdPartyDtoResponse.Email1 = _selectedRecord.Email1;
            thirdPartyDtoResponse.Email2 = _selectedRecord.Email2;
            thirdPartyDtoResponse.WebPage = _selectedRecord.WebPage;
            thirdPartyDtoResponse.FullName = _selectedRecord.FullName;
            thirdPartyDtoResponse.FirstName = _selectedRecord.FirstName;
            thirdPartyDtoResponse.MiddleName = _selectedRecord.MiddleName;
            thirdPartyDtoResponse.LastName = _selectedRecord.LastName;
            thirdPartyDtoResponse.Initials = _selectedRecord.Initials;
            thirdPartyDtoResponse.Phone1 = _selectedRecord.Phone1;
            thirdPartyDtoResponse.Phone2 = _selectedRecord.Phone2;

            textIdentificationType = valueIdentification(_selectedRecord.IdentificationType);
            identificationType= _selectedRecord.IdentificationType;
            thirdPartyDtoRequest.IdentificationType = _selectedRecord.IdentificationType;

            textCharge = valueCharge(_selectedRecord.ChargeCode);
            charge = _selectedRecord.ChargeCode;
            thirdPartyDtoResponse.ChargeCode = _selectedRecord.ChargeCode;

            textNature = valueNature(_selectedRecord.NatureCode);
            nature = _selectedRecord.NatureCode;
            thirdPartyDtoResponse.NatureCode = _selectedRecord.NatureCode;

            IsEditForm = true;

        }

        private void HandleModalNotiClose(ModalClosedEventArgs args)
        {
            if (notificationModal.Type == ModalNotificationsComponent.ModalType.Success)
            {
                ResetFormAsync();
                OnResetForm.InvokeAsync();
                UpdateModalStatus(args.ModalStatus);
            }
        }

        #endregion ModalMethods

        #region ModalAddressMethods

        public void updateAddressSelection(List<(string, AddressDtoRequest)> address)
        {
            if (address != null && address.Count > 0)
            {
                (textAddress, addressDtoRequest) = address[0];
            }

        }


        #endregion

        #region GetsDropdownList

        private async Task GetAddressAsync(int addressId)
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

        private AddressDtoRequest fillAddress(AddressDtoResponse address)
        {
            AddressDtoRequest request = new AddressDtoRequest();

            request.CountryId = address.CountryId;
            request.StateId = address.StateId;
            request.CityId = address.CityId;
            request.StType = address.StType;
            request.StNumber = address.StNumber;
            request.StLetter = address.StLetter;
            request.StBis = address.StBis;
            request.StComplement = address.StComplement;
            request.StCardinality = address.StCardinality;
            request.CrType = address.CrType;
            request.CrNumber = address.CrNumber;
            request.CrLetter = address.CrLetter;
            request.CrBis = address.CrBis;
            request.CrComplement = address.CrComplement;
            request.CrCardinality = address.CrCardinality;
            request.HouseType = address.HouseType;
            request.HouseClass = address.HouseClass;
            request.HouseNumber = address.HouseNumber;
            
            return request;
        }

        private async Task GetIdentificationType()
        {
            try
            {
                var type = "";
                if (personType == "" || personType == null) 
                {
                    type = "TDIN";
                }
                else
                {
                    type = (personType == "PN") ? "TDIN" : "TDIJ";
                }
                

                Dictionary<string, dynamic> headers = new()
                {
                    { "ParamCode", type }
                };

                var response = await CallService.Get<List<SystemFieldsDtoResponse>>("params/SystemFields/ByFilter", headers);

                identificationTypeList = response.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los parámetros: {ex.Message}");
            }
        }

        private async Task GetCharge()
        {
            try
            {
                Dictionary<string, dynamic> headers = new()
                {
                    { "ParamCode", "CAR" }
                };

                var response = await CallService.Get<List<SystemFieldsDtoResponse>>("params/SystemFields/ByFilter", headers);

                chargeList = response.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los parámetros: {ex.Message}");
            }
        }

        private async Task GetNature()
        {
            try
            {
                Dictionary<string, dynamic> headers = new()
                {
                    { "ParamCode", "NAT" }
                };

                var response = await CallService.Get<List<SystemFieldsDtoResponse>>("params/SystemFields/ByFilter", headers);

                natureList = response.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los parámetros: {ex.Message}");
            }
        }

        #endregion GetsDropdownList

        #region valueSelected

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
                var acode = partes[0].Trim();
                var asystemparam = partes[1].Trim();
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

        private string valueIdentification(string selectedValue)
        {
            return GetValueFromList(identificationTypeList, selectedValue, "TDI");
        }
        private string valueCharge(string selectedValue)
        {
            return GetValueFromList(chargeList, selectedValue, "CAR");
        } 
        private string valueNature(string selectedValue)
        {
            return GetValueFromList(natureList, selectedValue, "NAT");
        }



        //Metodo para tomar el Code de StType, CrType, HouseType y HouseClass
        private string GetCodeFromList(List<SystemFieldsDtoResponse> itemList, string selectedValue, string systemParamCode)
        {
            var selectedDataItem = "";

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

        private string codeIdentification(string selectedValue)
        {
            return GetCodeFromList(identificationTypeList, selectedValue, (personType=="PN" ? "TDIN":"TDIJ"));
        }
        private string codeCharge(string selectedValue)
        {
            return GetCodeFromList(chargeList, selectedValue, "CAR");
        }
        private string codeNature(string selectedValue)
        {
            return GetCodeFromList(natureList, selectedValue, "NAT");
        }

        #endregion valueSelected
    }
}
