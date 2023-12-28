using ControlDoc.Components.Components.Grids;
using ControlDoc.Components.Components.Input;
using ControlDoc.Models.Models.Administration.Response;
using ControlDoc.Models.Models.Administration.Request;
using ControlDoc.Services.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlDoc.Components.Components.Modals;
using Microsoft.JSInterop;
using CurrieTechnologies.Razor.SweetAlert2;
using ControlDoc.Services.Interfaces;
using ControlDoc.Models.Models.Generic;

namespace ControlDoc.ComponentViews.ComponentViews.Administration
{
    public partial class ModalBranchOffices : ComponentBase
    {
        #region Variables
        #region Injects
        [Inject]
        private IJSRuntime Js { get; set; }

        [Inject] 
        private ICallService CallService { get; set; }
        #endregion

        private BranchOfficesDtoResponse _selectedRecord;

        private BranchOfficeDtoRequest branchOfficeRequest = new BranchOfficeDtoRequest();
        private BranchOfficeDtoRequest branchOfficeRequestEdit = new BranchOfficeDtoRequest();
        private ModalComponent? modal { get; set; }

        private InputModalComponent inputId;
        private InputModalComponent inputCode;
        private InputModalComponent inputName;
        private InputModalComponent inputRegion;
        private InputModalComponent inputTerritory;
        private ModalNotificationsComponent notificationModal;

        private bool IsDisabledCode = false;
        private string IdBranchOffice;

        private bool IsEditForm = false;
        private bool modalStatus = false;
        #endregion

        #region Component Parameters

        // El parámetro "IdModalIdentifier" se utiliza como un identificador único de la modal.
        [Parameter] public string IdModalIdentifier { get; set; }
        [Parameter] public EventCallback<bool> OnChangeData { get; set; }

        


        #endregion

        #region Methods
        // Método para manejar el envío válido del formulario.
        private async Task HandleValidSubmit()
        {
            // Lógica de envío del formulario
            if (IsEditForm)
            {
                await HandleFormUpdate();
            }
            else
            {
                await HandleFormCreate();
            }

            StateHasChanged();            
        }

        private async Task HandleFormCreate()
        {
            if (inputCode.IsInputValid && inputName.IsInputValid)
            {
                branchOfficeRequest.AddressId = 12;
                branchOfficeRequest.Code = inputCode.InputValue;
                branchOfficeRequest.NameOffice = inputName.InputValue;
                branchOfficeRequest.Region = inputRegion.InputValue;
                branchOfficeRequest.Territory = inputTerritory.InputValue;

                var response = await CallService.Post<BranchOfficesDtoResponse, BranchOfficeDtoRequest>("params/BranchOffice/CreateBranchOffice", branchOfficeRequest);

                if (response.Succeeded) {
                    notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Success, "se creo el registro exitosamente", "aceptar", true);
                    await OnChangeData.InvokeAsync(true);
                }
            }
            else {
                notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Error, "No se creo el registro exitosamente", "aceptar", true);
            }
            

        }

        private async Task HandleFormUpdate()
        {
            if (inputCode.IsInputValid && inputName.IsInputValid)
            {
                _selectedRecord.Code = inputCode.InputValue;
                _selectedRecord.NameOffice = inputName.InputValue;
                _selectedRecord.Region = String.IsNullOrEmpty(inputRegion.InputValue) ? _selectedRecord.Region : inputRegion.InputValue;
                _selectedRecord.Territory = String.IsNullOrEmpty(inputTerritory.InputValue) ? _selectedRecord.Territory : inputTerritory.InputValue;

                Dictionary<string, dynamic> headers = new() { { "branchOfficeUpdateId", _selectedRecord.BranchOfficeId } };

                var response = await CallService.Put<BranchOfficesDtoResponse, BranchOfficesDtoResponse>("params/BranchOffice/UpdateBranchOffice", _selectedRecord, headers);

                if (response.Succeeded) {

                    notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Success, "se actualizo exitosamente el registro", "Aceptar", true);
                    await OnChangeData.InvokeAsync(true);
                }                
            }
            else
            {
                notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Error, "No se actualizo el registro, intente de nuevo", "Aceptar", true);
            }


            // Aca iria el consumo del micro servicio para actualizar

        }

        // Método para restablecer el formulario.
        private async Task ResetFormAsync()
        {
            if (!IsEditForm)
            {
                branchOfficeRequest = new BranchOfficeDtoRequest();
            }
            else
            {
                branchOfficeRequest = branchOfficeRequestEdit;
            }
        }

        // Método para actualizar el registro seleccionado.
        public void RecibirRegustro(BranchOfficesDtoResponse response)
        {
            _selectedRecord = response;
            branchOfficeRequestEdit.Code = _selectedRecord.Code;
            branchOfficeRequestEdit.NameOffice = _selectedRecord.NameOffice;
            branchOfficeRequestEdit.Region = _selectedRecord.Region;
            branchOfficeRequestEdit.Territory = _selectedRecord.Territory;

            branchOfficeRequest.Code = _selectedRecord.Code;
            branchOfficeRequest.NameOffice = _selectedRecord.NameOffice;
            branchOfficeRequest.Region = _selectedRecord.Region;
            branchOfficeRequest.Territory = _selectedRecord.Territory;
            IdBranchOffice = _selectedRecord.BranchOfficeId.ToString();
            IsDisabledCode = true;
            IsEditForm = true;
        }

        public void UpdateModalStatus(bool newValue)
        {
            modalStatus = newValue;
            StateHasChanged();
        }

        private void HandleModalClosed(bool status)
        {            
            modalStatus = status;
            branchOfficeRequest = new BranchOfficeDtoRequest();
            IsDisabledCode = false;
            IdBranchOffice = "";
            StateHasChanged();
        }

        private void HandleModalNotiClose(ModalClosedEventArgs args)
        {
            if(notificationModal.Type == ModalNotificationsComponent.ModalType.Success)
            {
                UpdateModalStatus(args.ModalStatus);
            }          
        }
        #endregion
    }
}
