using ControlDoc.Components.Components.DropDownListTelerik;
using ControlDoc.Components.Components.Input;
using ControlDoc.Components.Components.Modals;
using ControlDoc.Models.Models.Administration.Request;
using ControlDoc.Models.Models.Administration.Response;
using ControlDoc.Models.Models.Generic;
using ControlDoc.Models.Models.Pagination;
using ControlDoc.Services.Interfaces;
using ControlDoc.Services.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Telerik.Blazor.Components;
using static System.Net.Mime.MediaTypeNames;

namespace ControlDoc.ComponentViews.ComponentViews.Administration

{
    public partial class ModalCompanies
    {
        #region Variables
        #region Inject
        [Inject] 
        private IJSRuntime Js { get; set; }

        [Inject] 
        private ICallService CallService { get; set; }
        #endregion

        #region Parameter
        [Parameter] 
        public bool ModalStatus { get; set; }

        [Parameter] 
        public EventCallback<bool> OnChangeData { get; set; }
        #endregion
        #endregion

        #region Identificar Editar/Crear
        private async Task HandleValidSubmit()
        {
            // Lógica de envío del formulario
            if (IsEditForm)
            {

                await putCompany();

            }
            else
            {

                await postCompany();

            }

            StateHasChanged();

        }
        #endregion

        #region Objects
        private CompaniesDtoRequest CompaniesForm = new CompaniesDtoRequest();
        private CompaniesDtoResponse CompaniesFormResponse = new CompaniesDtoResponse();
        private AddressDtoRequest AddrressformCompaniesRequest;
        private AddressDtoResponse AddrressformCompaniesResponse;
        private CompaniesDtoRequest companiesRequest = new CompaniesDtoRequest();
        private CompaniesDtoResponse _selectedRecord;
        private Meta meta;

        #region Variables
        private string textAddress = "";
    
    

        private string MySelectedIdSystemFieldsNatural { get; set; }

        private bool modalstatus2;
        private string MySelectedIdSystemFieldsJuridico { get; set; }



        private bool IsEditForm = false;
        private bool IsDisabledCode = false;

        private string textTDIJ = "Seleccione un tipo de documento...";
        private string textTDIN = "Seleccione un tipo de documento...";
        #endregion


        #region Inputs
        private bool formSubmitted = false;

        private InputModalComponent namebussinessinput;
        private InputModalComponent inputIdC;
        public InputModalComponent NITinput;
        public InputModalComponent telefonoInput;
        public InputModalComponent webInput;
        public InputModalComponent emailInput;

        public InputModalComponent numIdentificationInput;
        public InputModalComponent CelularInput;
        public InputModalComponent identificationAgenteLegalInput;
        public InputModalComponent NameAgentLegalInput;





        #endregion

        #region VariablesGets





        #endregion

        #region List
        private List<AddressDtoResponse> ListAddress;
        private List<SystemFieldsDtoResponse> DocumentTypeTDIJ;
        private List<SystemFieldsDtoResponse> DocumentTypeTDIN;

        #endregion List


        #endregion Variables

        #region Validate


        #endregion

        #region Clear

        private void ClearForm()
        {

            formSubmitted = false;
        }
        #endregion

        #region OnInitialized
        protected override async Task OnInitializedAsync()
        {

            ModalStatus = false;
            await GetDocumentTypeTDIJ();
            await GetDocumentTypeTDIN();

        }

        #endregion

        #region Methods Get

        #region GetAddress
        private async Task GetAddressAsync(CompaniesDtoResponse dato)
        {
            try
            {
                Dictionary<string, dynamic> headers = new()
                {
                    { "IdAddress", dato.AddressId}
                };

                var response = await CallService.Get<AddressDtoResponse>("administration/Address/ByFilterId", headers);

                AddrressformCompaniesResponse = response.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los parámetros: {ex.Message}");
            }
        }
        #endregion

        #region GetDocumentTypeTDIN

        private async Task GetDocumentTypeTDIN()
        {
            try
            {

                Dictionary<string, dynamic> headers = new()
                {
                    { "ParamCode","TDIN" }
                };
                var response = await CallService.Get<List<SystemFieldsDtoResponse>>("params/SystemFields/ByFilter", headers);
                DocumentTypeTDIN = response.Data;

                meta = response.Meta;

            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error al obtener los tipos de documento: {ex.Message}");
            }
        }
        #endregion GetDocumentTypeTDIN

        #region GetDocumentTypeTDIJ
        private async Task GetDocumentTypeTDIJ()
        {
            try
            {

                Dictionary<string, dynamic> headers = new()
                {
                    { "ParamCode","TDIJ" }
                };
                var response = await CallService.Get<List<SystemFieldsDtoResponse>>("params/SystemFields/ByFilter", headers);
                DocumentTypeTDIJ = response.Data;

                meta = response.Meta;

            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error al obtener los tipos de documento: {ex.Message}");
            }
        }


        #endregion GetDocumentTypeTDIJ

        #endregion Methods Get

        #region Method Post
        private async Task postCompany()
        {
            IsEditForm = false;
            StateHasChanged();
            CompaniesForm.CompanyData = new();
            CompaniesForm.CompanyData.Address = new();

            CompaniesForm.CompanyData.Address = AddrressformCompaniesRequest;

            //Datos CompanyData
            CompaniesForm.CompanyData.LegalAgentFullName = NameAgentLegalInput.InputValue;
            CompaniesForm.CompanyData.CellPhoneNumber = CelularInput.InputValue;
            CompaniesForm.CompanyData.LegalAgentId = numIdentificationInput.InputValue;
            CompaniesForm.CompanyData.PhoneNumber = telefonoInput.InputValue;
            CompaniesForm.CompanyData.Email = emailInput.InputValue;
            CompaniesForm.CompanyData.WebAddress = webInput.InputValue;
            CompaniesForm.CompanyData.Domain = "GoDaddy";
            CompaniesForm.CompanyData.LegalAgentIdType = "TDIN," + MySelectedIdSystemFieldsNatural;

            //Datos Companies
            CompaniesForm.Identification = NITinput.InputValue;
            CompaniesForm.IdentificationType = "TDIJ," + MySelectedIdSystemFieldsJuridico;
            CompaniesForm.BusinessName = namebussinessinput.InputValue;

            var response = await CallService.Post<CompaniesDtoResponse, CompaniesDtoRequest>("companies/Company/CreateCompany", CompaniesForm);
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


        #endregion

        #region Method Put

        #region RecibirCampos

        public void RecibirRegistro(CompaniesDtoResponse response)
        {
            _selectedRecord = response;
            IsDisabledCode = true;
            IsEditForm = true;
            string comp = _selectedRecord.IdentificationType;
            string agent = _selectedRecord.LegalAgentIdType;


            CompaniesFormResponse.CompanyId = _selectedRecord.CompanyId;
            CompaniesFormResponse.AddressId = _selectedRecord.AddressId;
            CompaniesFormResponse.Identification = _selectedRecord.Identification;
            //Request
            CompaniesFormResponse.AddressId = _selectedRecord.AddressId;
            CompaniesFormResponse.Address = _selectedRecord.Address;
            textAddress = _selectedRecord.Address;
            CompaniesFormResponse.LegalAgentFullName = _selectedRecord.LegalAgentFullName;
            CompaniesFormResponse.CellPhoneNumber = _selectedRecord.CellPhoneNumber;
            CompaniesFormResponse.LegalAgentId = _selectedRecord.LegalAgentId;
            CompaniesFormResponse.PhoneNumber = _selectedRecord.PhoneNumber;
            CompaniesFormResponse.Email = _selectedRecord.Email;
            CompaniesFormResponse.WebAddress = _selectedRecord.WebAddress;
            CompaniesFormResponse.Domain = _selectedRecord.Domain;
            CompaniesFormResponse.LegalAgentIdType = _selectedRecord.LegalAgentIdType;
            textTDIJ = NameTDIJ(_selectedRecord.IdentificationType);
            textTDIN = NameTDIN(_selectedRecord.LegalAgentIdType);

            //Companies Request
            CompaniesFormResponse.Identification = _selectedRecord.Identification;
            CompaniesFormResponse.IdentificationType = _selectedRecord.IdentificationType;
            CompaniesFormResponse.BusinessName = _selectedRecord.BusinessName;

            MySelectedIdSystemFieldsJuridico = agent;
            MySelectedIdSystemFieldsNatural = comp;

           


        }
        #endregion

        #region ObtenerNombreDropDownList

        #region TDIJ
        private string NameTDIJ(string selectedValue)
        {

            var selectedDataItem = DocumentTypeTDIJ.FirstOrDefault(item => item.code == selectedValue);
            textTDIJ = selectedDataItem.value;


            return textTDIJ;
        }
        #endregion

        #region TDIN
        private string NameTDIN(string selectedValue)
        {

            var selectedDataItem = DocumentTypeTDIN.FirstOrDefault(item => item.code == selectedValue);
            textTDIN = selectedDataItem.value;


            return textTDIN;
        }
        #endregion

        #endregion ObtenerNombreDropDownList

        #region Campos Address 
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

        #endregion

       
        #region Actualizar
        public async Task putCompany()
        {
            IsEditForm = true;
            StateHasChanged();

            Dictionary<string, dynamic> headers = new() { { "id", _selectedRecord.CompanyId } };
            CompaniesForm = new();
            CompaniesForm.CompanyData = new();
            await GetAddressAsync(CompaniesFormResponse);
            var addressDtoRequestOld = fillAddress(AddrressformCompaniesResponse);
            var empty = new AddressDtoRequest();
           
            



            try
            {
                //CompanyData Response
                _selectedRecord.LegalAgentFullName = NameAgentLegalInput?.InputValue;
                _selectedRecord.CellPhoneNumber = CelularInput.InputValue;
                _selectedRecord.LegalAgentId = numIdentificationInput.InputValue;
                _selectedRecord.PhoneNumber = telefonoInput.InputValue;
                _selectedRecord.Email = emailInput.InputValue;
                _selectedRecord.WebAddress = webInput.InputValue;
                _selectedRecord.Domain = _selectedRecord.Domain;
                _selectedRecord.LegalAgentIdType = MySelectedIdSystemFieldsNatural;
                
               

                //Companies Response
                _selectedRecord.Identification = NITinput.InputValue;
                _selectedRecord.IdentificationType = MySelectedIdSystemFieldsJuridico;
                _selectedRecord.BusinessName = namebussinessinput.InputValue;

                //Company Data Request
                
                CompaniesForm.CompanyData.LegalAgentFullName = _selectedRecord.LegalAgentFullName;
                CompaniesForm.CompanyData.CellPhoneNumber = _selectedRecord.CellPhoneNumber;
                CompaniesForm.CompanyData.LegalAgentId = _selectedRecord.LegalAgentId;
                CompaniesForm.CompanyData.PhoneNumber = _selectedRecord.PhoneNumber;
                CompaniesForm.CompanyData.Email = _selectedRecord.Email;
                CompaniesForm.CompanyData.WebAddress = _selectedRecord.WebAddress;
                CompaniesForm.CompanyData.Domain = _selectedRecord.Domain;
                CompaniesForm.CompanyData.LegalAgentIdType = "TDIN," + _selectedRecord.LegalAgentIdType;

                CompaniesForm.CompanyData.Address = (AddrressformCompaniesRequest == empty || AddrressformCompaniesRequest == null) ? addressDtoRequestOld : AddrressformCompaniesRequest;


                //Companies Request
                CompaniesForm.Identification = _selectedRecord.Identification;
                CompaniesForm.IdentificationType = "TDIJ," + _selectedRecord.IdentificationType;
                CompaniesForm.BusinessName = _selectedRecord.BusinessName;

                var response = await CallService.Put<CompaniesDtoResponse, CompaniesDtoRequest>("companies/Company/UpdateCompany", CompaniesForm, headers);
                if (response.Succeeded)
                {
                    notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Success, "se actualizo exitosamente la compañía", "aceptar", true);
                    await OnChangeData.InvokeAsync(true);
                }
                else
                {

                    notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Error, "No se actualizo exitosamente la compañía", "aceptar", true);

                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }

        }
        #endregion

        #endregion Methods Put

        #region modal

        private bool modalStatus = false;
        private ModalNotificationsComponent notificationModal;
        [Parameter]
        public EventCallback<bool> OnStatusChanged { get; set; }
        [Parameter] public EventCallback<int> OnIdSaved { get; set; }
        [Parameter] public EventCallback OnResetForm { get; set; }

        //metodo para abrir nueva modal dentro de otra modal
        private async Task OpenNewModal()
        {
            await OnStatusChanged.InvokeAsync(true);
            if (textAddress != "" && _selectedRecord != null)
            {
                await OnIdSaved.InvokeAsync(_selectedRecord.AddressId);
            }
        }

        public void UpdateSelectedRecord(CompaniesDtoResponse record)
        {
            _selectedRecord = record;
        }
        public void UpdateModalStatus(bool newValue)
        {
            modalStatus = newValue;
            StateHasChanged();
        }

        public async Task ResetFormAsync()
        {
            CompaniesFormResponse = new CompaniesDtoResponse();
            textTDIJ = "Seleccione un tipo de documento...";
            textTDIN = "Seleccione un tipo de documento...";
            AddrressformCompaniesRequest = new AddressDtoRequest();
            textAddress = "";
        }

        private void HandleModalClosed(bool status)
        {
            modalStatus = status;
            OnResetForm.InvokeAsync();
            ResetFormAsync();
            StateHasChanged();
        }


        //modal notifiaciones:

        private void HandleModalNotiClose(ModalClosedEventArgs args)
        {
            if (notificationModal.Type == ModalNotificationsComponent.ModalType.Success)
            {
                ResetFormAsync();
                OnResetForm.InvokeAsync();
                UpdateModalStatus(args.ModalStatus);
            }


        }
        #endregion Modal

        #region ModalAddressMethods

        public void updateAddressSelection(List<(string, AddressDtoRequest)> address)
        {
            if (address != null && address.Count > 0)
            {
                (textAddress, AddrressformCompaniesRequest) = address[0];
            }

        }


        #endregion
    }
}