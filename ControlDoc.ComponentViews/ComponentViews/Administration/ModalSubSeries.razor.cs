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
using Microsoft.JSInterop;
using Telerik.DataSource.Extensions;

namespace ControlDoc.ComponentViews.ComponentViews.Administration
{
    public partial class ModalSubSeries
    {
        [Inject] private ICallService CallService { get; set; }

        [Parameter] public int serieID { get; set; }

        private DropDownListTelerik<SeriesDtoResponse> idSerie { get; set; }
        private int MySelectedSeries { get; }

        private SubSerieDtoRequest subSerieDtoRequest = new SubSerieDtoRequest();
        private SubSeriesDtoResponse _selectedRecord;
        private List<SeriesDtoResponse> seriesList;
        private Meta meta;

        [Parameter] public EventCallback<bool> OnStatusUpdate { get; set; }

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
                    subSerieDtoRequest.SeriesId = serieID;
                    subSerieDtoRequest.Code = codeInput?.InputValue;
                    subSerieDtoRequest.Name = nameInput?.InputValue;
                    subSerieDtoRequest.Description = descriptionInput?.InputValue;
                    subSerieDtoRequest.ActiveState = activeState;
                    subSerieDtoRequest.CreateUser = "admin";

                    var response = await CallService.Post<SubSeriesDtoResponse, SubSerieDtoRequest>("paramstrd/SubSeries/CreateSubSerie", subSerieDtoRequest);

                    if (response.Succeeded) 
                    { 
                        notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Success, response.Message, "Aceptar", true);
                        await OnStatusUpdate.InvokeAsync(true);

                    }
                    else { notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Error, "Cannot set up", "Aceptar", true); }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener las subSeries: {ex.Message}");
            }
        }

        private async Task HandleFormUpdate()
        {
            try
            {
                StateHasChanged();
                IsEditForm = true;
                _selectedRecord.Name = nameInput?.InputValue;
                _selectedRecord.Description = descriptionInput?.InputValue;
                _selectedRecord.ActiveState = activeState;
                _selectedRecord.UpdateUser = "admin";

                Dictionary<string, dynamic> headers = new()
                {
                    { "SubSerieId", _selectedRecord.SubSeriesId }
                };

                var response = await CallService.Put<SubSeriesDtoResponse, SubSeriesDtoResponse>("paramstrd/SubSeries/UpdateSubSerie", _selectedRecord, headers);
                if (response.Succeeded) 
                { 
                    notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Success, response.Message, "Aceptar", true);
                    await OnStatusUpdate.InvokeAsync(true);
                }
                else { notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Error, "Cannot set up", "Aceptar", true); }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener las subSeries: {ex.Message}");
            }
        }

        public async Task PreparedModal()
        {
            StateHasChanged();
            subSerieDtoRequest = new SubSerieDtoRequest();
            UpdateForm = false;
            await GetSeries();
            Text = seriesList.Where(x => x.SeriesId == serieID).Select(x => x.Name).FirstOrDefault();
        }

      

        public void UpdateSelectedRecord(SubSeriesDtoResponse response)
        {
            _selectedRecord = response;

            activeState = _selectedRecord.ActiveState;
            UpdateForm = false;
            IsEditForm = true;
            Text = _selectedRecord.SeriesName;


            IsDisabledCode = true;

            subSerieDtoRequest.Code = _selectedRecord.Code;
            subSerieDtoRequest.Name = _selectedRecord.Name;
            subSerieDtoRequest.Description = _selectedRecord.Description;


        }


        private async Task GetSeries()
        {
            try
            {
                var response = await CallService.Get<List<SeriesDtoResponse>>("paramstrd/Series/ByFilter");
                seriesList = response.Data;
                meta = response.Meta;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener las subSeries: {ex.Message}");
            }
        }
        // Método para restablecer el formulario.
        private async Task ResetFormAsync()
        {
            subSerieDtoRequest = new SubSerieDtoRequest();
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

        #endregion Methods


    }
}