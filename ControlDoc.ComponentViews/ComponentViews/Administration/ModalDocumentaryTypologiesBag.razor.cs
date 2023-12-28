using ControlDoc.Components.Components.Input;
using ControlDoc.Components.Components.Modals;
using ControlDoc.Models.Models.Administration.Request;
using ControlDoc.Models.Models.Administration.Response;
using ControlDoc.Models.Models.Generic;
using ControlDoc.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ControlDoc.ComponentViews.ComponentViews.Administration
{
    public partial class ModalDocumentaryTypologiesBag
    {
        #region Variables
        #region Injects
        [Inject] 
        private IJSRuntime Js { get; set; }

        [Inject] 
        private ICallService CallService { get; set; }
        #endregion

        #region Parameters
        // El parámetro "IdModalIdentifier" se utiliza como un identificador único de la modal.
        [Parameter] 
        public string IdModalIdentifier { get; set; }

        [Parameter] 
        public EventCallback<bool> OnChangeData { get; set; }
        #endregion
        #endregion

        private bool modalStatus = false;

        private bool IsEditForm = false;

        private DocumentaryTypologiesBagDtoRequest documentaryTypologiesBag = new();
        
        private DocumentaryTypologiesBagDtoResponse _selectedRecord;

        private InputModalComponent inputName;

        private InputModalComponent inputDescription;

        private InputModalComponent inputNivelSeguridad;  //No se ha definido su funcion

        private ModalNotificationsComponent notificationModal;

        private void HandleModalClosed(bool status)
        {
            modalStatus = status;
            documentaryTypologiesBag = new DocumentaryTypologiesBagDtoRequest();
            StateHasChanged();
        }

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
            if(inputName.IsInputValid &&  inputDescription.IsInputValid) 
            {

                documentaryTypologiesBag.TypologyName = inputName.InputValue;
                documentaryTypologiesBag.TypologyDescription = inputDescription.InputValue;

                var response = await CallService.Post<DocumentaryTypologiesBagDtoResponse, DocumentaryTypologiesBagDtoRequest>("documentarytypologies/DocumentaryTypologiesBag/CreateDocumentaryTypologiesBag", documentaryTypologiesBag);
               
                if (response.Succeeded)
                {
                    notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Success, "se creo el registro exitosamente", "Aceptar", true);
                    await OnChangeData.InvokeAsync(true);
                }
                else 
                {

                    notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Error, response.Message, "Aceptar", true);
                }
            }
            else
            {                
                notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Error, "Parametros Incorrectos", "Aceptar", true);
            }

        }

        private async Task HandleFormUpdate()
        {

            StateHasChanged();
            IsEditForm = true;

            _selectedRecord.TypologyName = inputName.InputValue;
            _selectedRecord.TypologyDescription = inputDescription.InputValue;

            Dictionary<string, dynamic> headers = new() { { "documentaryTypologyBagId", _selectedRecord.DocumentaryTypologyBagId } };
            
            var response = await CallService.Put<DocumentaryTypologiesBagDtoResponse, DocumentaryTypologiesBagDtoResponse>("documentarytypologies/DocumentaryTypologiesBag/UpdateDocumentaryTypologiesBag", _selectedRecord, headers);

            if (response.Succeeded)
            {
                notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Success, response.Message, "Aceptar", true);
                await OnChangeData.InvokeAsync(true);
            }
        }

        // Método para restablecer el formulario.
        private async Task ResetFormAsync()
        {
            if (!IsEditForm)
            {
                documentaryTypologiesBag = new();
            }
        }

        public void UpdateModalStatus(bool newValue)
        {
            modalStatus = newValue;
        }

        private void HandleModalNotiClose(ModalClosedEventArgs args)
        {
            if (notificationModal.Type == ModalNotificationsComponent.ModalType.Success)
            {
                UpdateModalStatus(args.ModalStatus);
            }
        }

        public void UpdateSelectedRecord(DocumentaryTypologiesBagDtoResponse response) 
        {
            IsEditForm = true;
            _selectedRecord = response;
            documentaryTypologiesBag.TypologyName = _selectedRecord.TypologyName;
            documentaryTypologiesBag.TypologyDescription = _selectedRecord.TypologyDescription;
        }
    }
}