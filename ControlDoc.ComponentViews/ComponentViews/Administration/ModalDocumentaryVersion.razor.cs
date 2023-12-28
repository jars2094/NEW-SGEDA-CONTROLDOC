using ControlDoc.Components.Components.Input;
using ControlDoc.Components.Components.Modals;
using ControlDoc.Models.Models.Administration.Request;
using ControlDoc.Models.Models.Administration.Response;
using ControlDoc.Models.Models.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDoc.ComponentViews.ComponentViews.Administration
{
    public partial class ModalDocumentaryVersion: ComponentBase
    {
        #region Injects

        [Inject]
        private IJSRuntime Js { get; set; }

        #endregion

        #region Component Parameters


        [Parameter] public EventCallback<bool> OnChangeData { get; set; }

        public bool modalStatus = false;
        private DateTime? from { get; set; }
        private DateTime? to { get; set; }

        private bool SelectActivoState {  get; set; }



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

        //Cambiar el dto cuando este el back
        private DocumentaryVersionDtoRequest _selectedRecord { get; set; }

        private bool IsDisabledCode = false;
        private List<FileInfoData> fileInfoDatas = new List<FileInfoData>();




        private bool IsEditForm = false;

        private DocumentaryVersionDtoRequest _documentaryVersionDtoRequest = new();


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

        private async Task HandleFilesListActas(List<FileInfoData> newList)
        {
            fileInfoDatas = newList;
        }
        private async Task HandleFilesOrganigrama(List<FileInfoData> newList)
        {
            fileInfoDatas = newList;
        }




        public void UpdateModalStatusDocumentaryVersion(bool newValue)
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
                UpdateModalStatusDocumentaryVersion(args.ModalStatus);
            }


        }
       

        //Actualiza la modal con la informacion de la fila de la grilla
        public void UpdateSelectedRecord(DocumentaryVersionDtoRequest? record)
        {
            _selectedRecord = record;
            //Limpiar();
            //PermissionId = _selectedRecord.permissionId;
            //FunctionalityId = _selectedRecord.functionalityId;
            //SelectAccessF = _selectedRecord.accessF;
            //SelectCreateF = _selectedRecord.createF;
            //SelectModifyF = _selectedRecord.modifyF;
            //SelectConsultF = _selectedRecord.consultF;
            //SelectDeleteF = _selectedRecord.deleteF;
            //SelectPrintF = _selectedRecord.printF;
            //SelectActivoState = _selectedRecord.activeState;
            //habilitarFuncionalidad = false;
            //btnSaveOEdit = "Editar";
            //DefaultTextGrid = _selectedRecord.functionalityName;

            //SelectDropdown.Refresh();
            //SelectDropdown.changeValue(FunctionalityId.ToString());

        }

        private void Save()
        {
            notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Success, "Guardado satisfactoriamente", "Aceptar", true);
        }
        #endregion





    }
}
