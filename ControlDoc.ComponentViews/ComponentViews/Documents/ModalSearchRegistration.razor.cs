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
using static ControlDoc.ComponentViews.ComponentViews.Documents.ModalAssociatedResourcesSearch;

namespace ControlDoc.ComponentViews.ComponentViews.Documents
{
    public partial class ModalSearchRegistration
    {
        private DocTemplateDtoResponse DocTemplateFormResponse = new DocTemplateDtoResponse();

        #region Variables
        private bool modalStatus = false;
        private bool IsEditForm = false;
        private int ID;

        private string Idcontrol { get; set; }
        private string Folio { get; set; }
        private string Solicitante { get; set; }
        private string Radicado { get; set; }
        private string Anexo { get; set; }
        private string Oficina { get; set; }

        #endregion

        #region InputsReference
        private InputModalComponent IdcontrolInput { get; set; }
        private InputModalComponent FoliolInput { get; set; }
        private InputModalComponent SolicitanteInput { get; set; }
        private InputModalComponent RadicadoInput { get; set; }
        private InputModalComponent AnexoInput { get; set; }
        private InputModalComponent OficinaInput { get; set; }
        #endregion

        #region List
        public List<ModelData> Data = new List<ModelData>();
        #endregion

        #region Objects
        private ModalNotificationsComponent notificationModal;
        #endregion

        #region Fecha
        private DateTime? SelectedDate { get; set; }
        private DateTime Max = new DateTime(2050, 12, 31);
        private DateTime Min = new DateTime(1950, 1, 1);
        private int DebounceDelay { get; set; } = 200;
        #endregion

        #region CloseModal
        private void HandleModalClosed(bool status)
        {

            modalStatus = status;
            //DocTemplateFormResponse = new DocTemplateDtoResponse();

            StateHasChanged();
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
    }
}
