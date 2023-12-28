using ControlDoc.Components.Components.DropDownListTelerik;
using ControlDoc.Components.Components.Input;
using ControlDoc.Components.Components.Modals;
using ControlDoc.Models.Models.Administration.Request;
using ControlDoc.Models.Models.Administration.Response;
using ControlDoc.Models.Models.Generic;
using ControlDoc.Models.Models.Pagination;
using ControlDoc.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Reflection.Metadata;

namespace ControlDoc.ComponentViews.ComponentViews.Administration
{
    public partial class ModalAdministrativeUnit
    {
        #region Variables
        #region Injects
        [Inject]
        private ICallService CallService { get; set; }
        #endregion
        #endregion

        [Parameter] public int IDVersion { get; set; }

        private AdministrativeUnitDtoRequest adminUnitDtoRequest = new();
        private AdministrativeUnitDtoResponse adminUnitDtoResponse = new();

        private AdministrativeUnitDtoResponse _selectedRecord;

        private List<DocumentalVersionsDtoResponse> docVersionList;
        private Meta meta;
        private VUserDtoResponse BossSelected { get; set; }
        private int MySelectedDocVersions { get; }
        private DropDownListTelerik<DocumentalVersionsDtoResponse> idDocumental { get; set; }

        private string Text = "Seleccione una opción";
        private bool UpdateForm = true;

        private InputModalComponent? codeinput;
        private InputModalComponent? descriptioninput;
        private InputModalComponent? nameinput;
        private InputModalComponent? Bossinput;

        private bool IsEditForm = false;
        private bool activeState = true;

        private bool IsDisabledCode = false;

        [Parameter] public EventCallback<bool> OnStatusUpdate { get; set; }

        #region Methods

        // Método para manejar el envío válido del formulario.
        private async Task HandleValidSubmit()
        {
            // Lógica de envío del formulario
            if (IsEditForm)
            {
                await HandleFormUpdate();

                IsEditForm = false;
            }
            else
            {
                await HandleFormCreate();
            }

            StateHasChanged();
        }

        public void updateBossSelection(VUserDtoResponse boosToSelect)
        {
            BossSelected = boosToSelect;
            Bossinput.InputValue = BossSelected.FullName;
        }

        private async Task HandleFormCreate()
        {
            if (codeinput.IsInputValid && nameinput.IsInputValid)
            {
                adminUnitDtoRequest.DocumentalVersionId = IDVersion;
                adminUnitDtoRequest.Name = nameinput.InputValue;
                adminUnitDtoRequest.Code = codeinput.InputValue;
                adminUnitDtoRequest.BossId = (int)BossSelected.UserId;
                adminUnitDtoRequest.Description = descriptioninput?.InputValue;
                adminUnitDtoRequest.ActiveState = activeState;
                adminUnitDtoRequest.CreateUser = "Admin";

                var response = await CallService.Post<AdministrativeUnitDtoResponse, AdministrativeUnitDtoRequest>("paramstrd/AdministrativeUnit/CreateAdministrativeUnit", adminUnitDtoRequest);

                if (response.Succeeded)
                {
                    notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Success, response.Message, "Aceptar", true);
                    await OnStatusUpdate.InvokeAsync(true);
                }
            }
            else { notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Error, "Cannot set up", "Aceptar", true); }
        
        
        }

        private async Task HandleFormUpdate()
        {
            StateHasChanged();
            IsEditForm = true;
            _selectedRecord.BossId = BossSelected.UserId;
            _selectedRecord.Name = nameinput?.InputValue;
            _selectedRecord.Description = descriptioninput?.InputValue;
            _selectedRecord.ActiveState = activeState;
            _selectedRecord.UpdateUser = "admin";

            Dictionary<string, dynamic> headers = new() { { "adminUnitId", _selectedRecord.AdministrativeUnitId } };

            var response = await CallService.Put<AdministrativeUnitDtoResponse, AdministrativeUnitDtoResponse>("paramstrd/AdministrativeUnit/UpdateAdministrativeUnit", _selectedRecord, headers);

            if (response.Succeeded)
            {
                notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Success, response.Message, "Aceptar", true);
                await OnStatusUpdate.InvokeAsync(true);
            }
        }

        public async Task PreparedModal()
        {
            StateHasChanged();
            UpdateForm = false;
            adminUnitDtoRequest = new();
            await GetDocumentalVersions();
            Text = docVersionList.Where(x => x.DocumentalVersionId == IDVersion).Select(x => x.Name).FirstOrDefault();
        }


        public void UpdateSelectedRecord(AdministrativeUnitDtoResponse response)
        {
            _selectedRecord = response;
            activeState = _selectedRecord.ActiveState;
            UpdateForm = false;
            IsEditForm = true;
            Text = _selectedRecord.DocumentalVersionName;


            IsDisabledCode = true;

            adminUnitDtoResponse.Code = _selectedRecord.Code;
            adminUnitDtoResponse.Name = _selectedRecord.Name;
            adminUnitDtoResponse.BossName = _selectedRecord.BossName;
            adminUnitDtoResponse.Description = _selectedRecord.Description;


        }

      

        private async Task GetDocumentalVersions()
        {
            try
            {
                var response = await CallService.Get<List<DocumentalVersionsDtoResponse>>("paramstrd/DocumentalVersions/ByFilter");
                docVersionList = response.Data;
                meta = response.Meta;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener las series: {ex.Message}");
            }
        }

        // Método para restablecer el formulario.
        private async Task ResetFormAsync()
        {
            adminUnitDtoRequest = new();
        }

        //cosas modal:

        private ModalNotificationsComponent notificationModal;

        [Parameter]
        public EventCallback<bool> OnStatusChanged { get; set; }

        private bool modalStatus = false;

        private async Task OpenNewModal()
        {
            await OnStatusChanged.InvokeAsync(true);
        }

        public void UpdateModalStatus(bool newValue)
        {
            modalStatus = newValue;
            StateHasChanged();
        }

        private void HandleModalClosed(bool status)
        {
            modalStatus = status;
        }

        //modal notifiaciones:

        private void HandleModalNotiClose(ModalClosedEventArgs args)
        {
            if (notificationModal.Type == ModalNotificationsComponent.ModalType.Success)
            {
                UpdateModalStatus(args.ModalStatus);
            }
        }

        

        #endregion Methods
    }
}