using ControlDoc.Components.Components.Input;
using ControlDoc.Components.Components.Modals;
using ControlDoc.Models.Models.Administration.Request;
using ControlDoc.Models.Models.Administration.Response;
using ControlDoc.Models.Models.Generic;
using ControlDoc.Services.Interfaces;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDoc.ComponentViews.ComponentViews.Documents
{
    public partial class ModalOverrideTray : ComponentBase
    {
        #region Injects

        [Inject]
        private IJSRuntime Js { get; set; }

        #endregion

        #region Component Parameters

        
        [Parameter] public EventCallback<bool> OnChangeData { get; set; }

         public bool modalStatus = false;
        


        #endregion

        #region Variables


        private ModalComponent? modal { get; set; }

        private InputModalComponent inputId;
        private InputModalComponent inputCode;
        private InputModalComponent inputName;
        private InputModalComponent inputRegion;
        private InputModalComponent inputTerritory;
        private string txtAInformation;
        private ModalNotificationsComponent notificationModal;

        private bool IsDisabledCode = false;
        private List<FileInfoData> fileInfoDatas = new List<FileInfoData>();




        private bool IsEditForm = false;

        private AdministrativeUnitDtoResponse adminUnitDtoResponse = new();


        #endregion

        #region Methods



        // Método para manejar el envío válido del formulario.
        private async Task HandleValidSubmit()
        {
            // Lógica de envío del formulario

            notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Success, "Guardado satisfactoriamente", "Aceptar", true);
            await HandleFormCreate();
            

            StateHasChanged();

        }

        private async Task HandleFormCreate()
        {
           //TODO: Creacion de solicitud de anulacion


        }

        private async Task HandleFilesList(List<FileInfoData> newList)
        {
            fileInfoDatas = newList;
        }




        public void UpdateModalStatusTray(bool newValue)
        {
            modalStatus = newValue;
            StateHasChanged();
        }

        private void HandleModalClosed(bool status)
        {

            modalStatus = status;
            StateHasChanged();

        }

        private void HandleModalNotiClose(ModalClosedEventArgs args)
        {
            if (notificationModal.Type == ModalNotificationsComponent.ModalType.Success)
            {
                UpdateModalStatusTray(args.ModalStatus);
            }


        }

        private void Save()
        {
            notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Success, "Guardado satisfactoriamente", "Aceptar", true);
        }
        #endregion

    }
}
