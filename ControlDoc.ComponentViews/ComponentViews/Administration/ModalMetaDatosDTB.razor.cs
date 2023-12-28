using ControlDoc.Components.Components.Input;
using ControlDoc.Components.Components.Modals;
using ControlDoc.Models.Models.Administration.Response;
using ControlDoc.Models.Models.Generic;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDoc.ComponentViews.ComponentViews.Administration
{
    public partial class ModalMetaDatosDTB
    {

        #region Variables
        private bool modalStatus = false;
        private bool IsEditForm = false;
        private bool activeState = false;
        #endregion

        #region Objects
        private DocumentaryTypologiesBagDtoResponse MetaDataFormResponse = new DocumentaryTypologiesBagDtoResponse();
        private ModalNotificationsComponent notificationModal;
        #endregion

        #region InputsReference
        private InputModalComponent MetaDatoInput;
        private InputModalComponent PositionInput;
        private InputModalComponent IdMetaTitituloInput;

        private string Metadato;
        private string Position;
        private string MetaTitulo;
        #endregion
        #region CloseModal

        private void HandleModalClosed(bool status)
        {

            modalStatus = status;
            MetaDataFormResponse = new DocumentaryTypologiesBagDtoResponse();

            StateHasChanged();
        }
        #endregion

        #region ModalNotifcation
        public void UpdateModalStatus(bool newValue)
        {
            modalStatus = newValue;
            StateHasChanged();
        }


        private void HandleModalNotiClose(ModalClosedEventArgs args)
        {
            if (notificationModal.Type == ModalNotificationsComponent.ModalType.Success)
            {
                UpdateModalStatus(args.ModalStatus);
            }


        }
        #endregion


        #region Identificar Editar/Crear
        private async Task HandleValidSubmit()
        {
            // Lógica de envío del formulario
            if (IsEditForm)
            {

                //await PutTemplateDoc();

            }
            else
            {

                //await PostTemplate();

            }

            StateHasChanged();

        }
        #endregion

        #region GridConsulta

        public class ModelData
        {
            public string SegmentName { get; set; }

        }
        public List<ModelData> Data = new List<ModelData>();


        #endregion

        private void ShowModalEdit(DocumentaryTypologiesBagDtoResponse record)
        {

            //modalFormatMasterList.UpdateModalStatus(true);
            //modalFormatMasterList.RecibirRegistro(record);
        }
    }
}
