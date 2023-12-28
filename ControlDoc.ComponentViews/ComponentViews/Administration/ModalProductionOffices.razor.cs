using ControlDoc.Components.Components.Input;
using ControlDoc.Components.Components.Modals;
using ControlDoc.Models.Models.Administration.Request;
using ControlDoc.Models.Models.Administration.Response;
using ControlDoc.Models.Models.Generic;
using ControlDoc.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Runtime.CompilerServices;
using Telerik.Blazor.Components.ListBox.EventArgs.Internal;

namespace ControlDoc.ComponentViews.ComponentViews.Administration
{
    public partial class ModalProductionOffices
    {
        #region Variables
        #region Injects
        [Inject] 
        private ICallService CallService { get; set; }
        #endregion

        #region Parameters
        [Parameter] 
        public string idModalIdentifier { get; set; } = null!;

        [Parameter]
        public bool modalStatus { get; set; }

        [Parameter] 
        public EventCallback<bool> OnStatusChanged { get; set; }

        [Parameter] 
        public EventCallback<bool> OnStatusChangedUpdate { get; set; }
        #endregion
        #endregion

        #region Fields

        private ModalNotificationsComponent notificationModal { get; set; } = new();
        private string responseMessage { get; set; } = null!;
        private VUserDtoResponse bossSelected { get; set; } = new();
        public bool userSearchModalStatus { get; set; } = new();
        public string bossNamme { get; set; } = null!;
        private int userId { get; set; }
        private InputModalComponent inputId { get; set; } = new();
        private InputModalComponent inputCode { get; set; } = new();
        private InputModalComponent inputName { get; set; } = new();
        private InputModalComponent inputBoss { get; set; } = new();
        private InputModalComponent inputDescription { get; set; } = new();
        private ProductionOfficeDtoRequest productionOfficeRequest { get; set; } = new();
        private ProductionOfficeUpdateDtoRequest productionOfficeUpdateRequest { get; set; } = new();
        private ProductionOfficesDtoResponse productionOfficeResponse { get; set; } = new();
        private int administrativeUnitId { get; set; }
        private string text { get; set; } = "";
        private bool isEditForm { get; set; } = false;
        private bool activeState { get; set; } = true;

        #endregion Fields

        #region Administrative Unit Fields

        private List<AdministrativeUnitDtoResponse> administrativeUnitList { get; set; } = new();
        private int selectAdministriveUnitId { get; set; } = new();

        #endregion Administrative Unit Fields

        #region Event Handlers

        public void updateBossSelection(VUserDtoResponse boosToSelect)
        {
            bossSelected = boosToSelect;
            bossNamme = bossSelected.FullName;
            userId = (int)bossSelected.UserId;
            StateHasChanged();
        }

        private async Task OpenNewModal()
        {
            await OnStatusChanged.InvokeAsync(true);
        }

        private void ResetFormAsync()
        {
            bossSelected = new();
            bossNamme = "";
            productionOfficeRequest.Code = "";
            productionOfficeRequest.Name = "";
            productionOfficeRequest.AdministrativeUnitId = 0;
            productionOfficeRequest.Description = "";
            selectAdministriveUnitId = 0;
        }

        protected override async Task OnInitializedAsync()
        {
            FillAdministrativeUnitDdl();
        }

        public void UpdateAdministrativeUnitSelected(int idSelected, string textSelected)
        {
            text = textSelected;
            administrativeUnitId = idSelected;
        }

        public void UpdateModalStatus(bool newValue)
        {
            modalStatus = newValue;
            StateHasChanged();
        }

        private async Task FillAdministrativeUnitDdl()
        {
            var response = await CallService.Get<List<AdministrativeUnitDtoResponse>>($"paramstrd/AdministrativeUnit/ByAdministrativeUnits");

            administrativeUnitList = response.Data;
            administrativeUnitList.Insert(0, new() { Name = "" });
        }

        public void OnChangeUA(int value)
        {
            selectAdministriveUnitId = value;
        }

        private void HandleModalClosed(bool status)
        {
            ResetFormAsync();
            modalStatus = status;
            StateHasChanged();
        }

        #endregion Event Handlers

        #region Form Handling

        private async Task HandleValidSubmit()
        {
            if (isEditForm)
            {
                await HandleFormUpdate();
                isEditForm = false;
            }
            else
            {
                await HandleFormCreate();
            }
            ResetForm();
        }

        private void ResetForm()
        {
            productionOfficeRequest = new();
        }

        private async Task HandleFormCreate()
        {
            try
            {
                if (inputCode.IsInputValid && inputName.IsInputValid && inputBoss.IsInputValid)
                {
                    productionOfficeRequest.BossId = userId;
                    productionOfficeRequest.Name = inputName.InputValue;
                    productionOfficeRequest.AdministrativeUnitId = administrativeUnitId;
                    productionOfficeRequest.Code = inputCode.InputValue;
                    productionOfficeRequest.ActiveState = activeState;
                    productionOfficeRequest.Description = inputDescription.InputValue;
                    productionOfficeRequest.CreateUser = "Val";

                    var response = await CallService.Post<ProductionOfficesDtoResponse, ProductionOfficeDtoRequest>("paramstrd/ProductionOffice/CreateProductionOffice", productionOfficeRequest);

                    if (response.Succeeded)
                    {
                        responseMessage = response.Message;
                        notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Success, response.Message, "Aceptar", true);
                    }
                    else
                    {
                        notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Error, response.Message, "Aceptar", true);
                    }
                }
                else
                {
                    notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Error, "Cannot set up", "Aceptar", true);
                }
                productionOfficeRequest = new();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener las series: {ex.Message}");
            }
        }

        private async Task HandleFormUpdate()
        {
            StateHasChanged();

            productionOfficeUpdateRequest.Name = inputName.InputValue;
            productionOfficeUpdateRequest.AdministrativeUnitId = administrativeUnitId;
            productionOfficeUpdateRequest.BossId = userId;
            productionOfficeUpdateRequest.Description = inputDescription.InputValue;
            productionOfficeUpdateRequest.ActiveState = activeState;
            productionOfficeUpdateRequest.UpdateUser = "Val";

            Dictionary<string, dynamic> headers = new() { { "ProOfficeId", productionOfficeResponse.ProductionOfficeId } };

            var response = await CallService.Put<ProductionOfficesDtoResponse, ProductionOfficeUpdateDtoRequest>("paramstrd/ProductionOffice/UpdateProductionOffice", productionOfficeUpdateRequest, headers);

            if (response.Succeeded) { notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Success, response.Message, "Aceptar", true); }
            else
            {
                notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Error, "Cannot set update", "Aceptar", true);
            }
        }

        #endregion Form Handling

        #region Modal Notifications

        private async Task HandleModalNotiClose(ModalClosedEventArgs args)
        {
            if (notificationModal.Type == ModalNotificationsComponent.ModalType.Success)
            {
                await OnStatusChangedUpdate.InvokeAsync(false);
            }
        }

        #endregion Modal Notifications

        #region Record Selection

        public async Task UpdateSelectedRecord(ProductionOfficesDtoResponse record)
        {
            isEditForm = true;
            productionOfficeResponse = record;
            text = productionOfficeResponse.AdministrativeUnitName;
            administrativeUnitId = productionOfficeResponse.AdministrativeUnitId;
            userId = (int)productionOfficeResponse.BossId;
            productionOfficeRequest.ActiveState = (bool)productionOfficeResponse.ActiveState;
            productionOfficeRequest.Description = record.Description;
            productionOfficeRequest.Name = productionOfficeResponse.Name;
            productionOfficeRequest.Code = productionOfficeResponse.Code;

            bossNamme = (await GetUserName((int)record.BossId)).ToString();
        }

        #endregion Record Selection

        #region Helper Methods

        private async Task<string> GetUserName(int userIdToSearch)
        {
            Dictionary<string, dynamic> header = new()
            {
                {"userId",userIdToSearch }
            };

            var response = await CallService.Get<UserDtoResponse>($"security/User/ByFilterId", header);

            var resultUser = response.Data;

            return resultUser.FullName;
        }

        #endregion Helper Methods
    }
}