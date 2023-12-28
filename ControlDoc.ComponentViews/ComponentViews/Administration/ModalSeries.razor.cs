using ControlDoc.Components.Components.DropDownListTelerik;
using ControlDoc.Components.Components.Input;
using ControlDoc.Components.Components.Modals;
using ControlDoc.Models.Models.Administration.Request;
using ControlDoc.Models.Models.Administration.Response;
using ControlDoc.Models.Models.Generic;
using ControlDoc.Models.Models.Pagination;
using ControlDoc.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace ControlDoc.ComponentViews.ComponentViews.Administration
{
    public partial class ModalSeries
    {
        #region Variables
        #region Injects
        [Inject] 
        private ICallService CallService { get; set; }
        #endregion
        #endregion

        [Parameter] public int proOfficeID { get; set; }

        private DropDownListTelerik<ProductionOfficesDtoResponse> idProOffice { get; set; }

        private int MySelectedProOffice { get; }

        private SerieDtoRequest serieDtoRequest = new SerieDtoRequest();
        private SeriesDtoResponse _selectedRecord;
        private List<ProductionOfficesDtoResponse> proOfficeList;
        private Meta meta;

        [Parameter] public EventCallback<bool> OnStatusUpdate { get; set; }

        private string responseMessage;
        private bool IsEditForm = false;
        private bool activeState = true;
        private bool UpdateForm = true;

        private InputModalComponent codeInput;
        private InputModalComponent nameInput;
        private InputModalComponent descriptionInput;

        private string Text = "Seleccione una opción";

        private bool IsDisabledCode = false;

        #region Methods

        private async Task HandleValidSubmit()
        {
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

        private async Task HandleFormCreate()
        {
            try
            {
                if (codeInput.IsInputValid && nameInput.IsInputValid)
                {
                    serieDtoRequest.ProductionOfficeId = proOfficeID;
                    serieDtoRequest.Code = codeInput?.InputValue;
                    serieDtoRequest.Name = nameInput?.InputValue;
                    serieDtoRequest.Description = descriptionInput?.InputValue;
                    serieDtoRequest.ActiveState = activeState;
                    serieDtoRequest.CreateUser = "admin";

                    var response = await CallService.Post<SeriesDtoResponse, SerieDtoRequest>("paramstrd/Series/CreateSeries", serieDtoRequest);

                    if (response.Succeeded)
                    {
                        responseMessage = response.Message;
                        await OnStatusUpdate.InvokeAsync(true);
                    }
                    else { notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Success, response.Message, "Aceptar", true); }
                }
                else { notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Error, "Cannot set up", "Aceptar", true); }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener las series: {ex.Message}");
            }
        }

        private async Task HandleFormUpdate()
        {
            try
            {
                IsEditForm = true;
                _selectedRecord.Name = nameInput?.InputValue;
                _selectedRecord.Description = descriptionInput?.InputValue;
                _selectedRecord.ActiveState = activeState;
                _selectedRecord.UpdateUser = "admin";

                Dictionary<string, dynamic> headers = new()
                {
                    { "SerieId", _selectedRecord.SeriesId }
                };

                var response = await CallService.Put<SeriesDtoResponse, SeriesDtoResponse>("paramstrd/Series/UpdateSeries", _selectedRecord, headers);

                if (response.Succeeded)
                {
                    responseMessage = response.Message;
                    await OnStatusUpdate.InvokeAsync(true);
                }
                else { notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Success, response.Message, "Aceptar", true); }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener las series: {ex.Message}");
            }
        }

        public async Task PreparedModal()
        {
            StateHasChanged();
            UpdateForm = false;
            serieDtoRequest = new SerieDtoRequest();
            await GetProductionOffices();
            Text = proOfficeList.Where(x => x.ProductionOfficeId == proOfficeID).Select(x => x.Name).FirstOrDefault();
        }



        public void UpdateSelectedRecord(SeriesDtoResponse response)
        {
            _selectedRecord = response;

            activeState = _selectedRecord.ActiveState;
            UpdateForm = false;
            IsEditForm = true;
            Text = _selectedRecord.ProductionOfficeName;


            IsDisabledCode = true;

            serieDtoRequest.Code = _selectedRecord.Code;
            serieDtoRequest.Name = _selectedRecord.Name;
            serieDtoRequest.Description = _selectedRecord.Description;


        }



        private async Task GetProductionOffices()
        {
            try
            {
                var response = await CallService.Get<List<ProductionOfficesDtoResponse>>("paramstrd/ProductionOffice/ByFilter");
                proOfficeList = response.Data;
                meta = response.Meta;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener las oficinas productoras: {ex.Message}");
            }
        }

        #endregion Methods

        // Método para restablecer el formulario.
        private async Task ResetFormAsync()
        {
            serieDtoRequest = new SerieDtoRequest();
        }

        //cosas modal:

        private ModalNotificationsComponent notificationModal;


        private bool modalStatus = false;

  
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
    }
}