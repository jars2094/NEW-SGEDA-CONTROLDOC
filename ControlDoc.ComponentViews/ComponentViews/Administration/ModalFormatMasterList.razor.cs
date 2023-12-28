using ControlDoc.Components.Components.Input;
using ControlDoc.Components.Components.Modals;
using ControlDoc.Models.Models.Administration.Request;
using ControlDoc.Models.Models.Administration.Response;
using ControlDoc.Models.Models.Generic;
using ControlDoc.Models.Models.Pagination;
using ControlDoc.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Telerik.SvgIcons;

namespace ControlDoc.ComponentViews.ComponentViews.Administration
{
    public partial class ModalFormatMasterList
    {

        #region Parameter
        [Parameter] public bool ModalStatus { get; set; }

        [Parameter] public EventCallback<bool> OnChangeData { get; set; }
        #endregion

        #region Event
  
        #endregion

        #region Objects
        private ModalNotificationsComponent notificationModal;
        private DocTemplateDtoResponse DocTemplateFormResponse = new DocTemplateDtoResponse();
        private Meta meta;
        private DocTemplateDtoRequest DocTemplateformRequest = new DocTemplateDtoRequest();
        private DocTemplateDtoResponse _selectedRecord;
        #endregion

        #region List
        private List<SystemFieldsDtoResponse> FormatDoc;

        #endregion

        #region Variables
        #region Injects
        [Inject]
        private ICallService CallService { get; set; }
        #endregion
        private bool IsEditForm = false;
        private string textFD = "Seleccione un tipo de formato...";
        private string ValueFormatDoc { get; set; }
        private bool activeState = true;
        private string file;
        private bool IsDisabledCode = false;
        private bool visible = false;

        public string[] ext { get; set; } = { ".xlsx", ".docx", ".xls", ".doc" };
        #endregion

        #region DropDownListFormatDoc
        public async Task GetFormatDoc()
        {
            try
            {

                Dictionary<string, dynamic> headers = new()
                {
                    { "ParamCode","TFOR" }
                };
                var response = await CallService.Get<List<SystemFieldsDtoResponse>>("params/SystemFields/ByFilter", headers);
                FormatDoc = response.Data;

                meta = response.Meta;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los tipos de documento: {ex.Message}");
            }
        }
        #endregion

        #region InputsReference
        private InputModalComponent CodeInput;
        private InputModalComponent NameTemplateInput;
        private InputModalComponent ProcessInput;
        private InputModalComponent VersionInput;

        private string version;
        
        #endregion

        #region OnInitialized
        protected override async Task OnInitializedAsync()
        {
            ModalStatus = false;
            await GetFormatDoc();
        }

        #endregion

        #region CloseModal
        private bool modalStatus = false;
        private void HandleModalClosed(bool status)
        {

            modalStatus = status;
            DocTemplateFormResponse = new DocTemplateDtoResponse();
       
            StateHasChanged();
        }
        #endregion

        #region ClearFormat
        public async Task ResetFiltersAsync()
        {
            DocTemplateFormResponse.Process = "";
            DocTemplateFormResponse.TempName = "";
            ValueFormatDoc = "";


            StateHasChanged();
        }
        #endregion

        #region Identificar Editar/Crear
        private async Task HandleValidSubmit()
        {
            // Lógica de envío del formulario
            if (IsEditForm)
            {
               
                await PutTemplateDoc();
               
            }
            else
            {
          
                await PostTemplate();
                
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

        #region PostTemplatedocuments

        private async Task PostTemplate()
        {
           

            DocTemplateformRequest.TempCode = CodeInput.InputValue;
            DocTemplateformRequest.TempName = NameTemplateInput.InputValue;
            DocTemplateformRequest.TempType = "TFOR," + ValueFormatDoc;
            DocTemplateformRequest.Process = ProcessInput.InputValue;
            //DocTemplateformRequest.tempVersion = int.Parse(VersionInput.InputValue);
            DocTemplateformRequest.Archivo = file;
            DocTemplateformRequest.ActiveState = activeState;

            var response = await CallService.Post<DocTemplateDtoResponse, DocTemplateDtoRequest>("documents/TemplateDocuments/CreateTemplateDocument", DocTemplateformRequest);
            if (response.Succeeded)
            {
                notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Success, "se creo el registro exitosamente", "aceptar", true);
                await OnChangeData.InvokeAsync(true);
            }
            else
            {

                notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Error, "No se creo el registro exitosamente", "aceptar", true);
            }
        }
        #endregion

        #region PutTemplateDocuments
        public async Task PutTemplateDoc()
        {
            IsEditForm = true;
            StateHasChanged();
           

            Dictionary<string, dynamic> headers = new() { { "idTempDoc", _selectedRecord.TemplateId } };

            try
            {

                DocTemplateformRequest.TempName = NameTemplateInput?.InputValue;
                DocTemplateformRequest.TempCode = CodeInput.InputValue;
                DocTemplateformRequest.TempType = "TFOR," + ValueFormatDoc;
                DocTemplateformRequest.Process = ProcessInput.InputValue??"";
                DocTemplateformRequest.ActiveState = activeState;
                DocTemplateformRequest.Archivo = file??"";

                var response = await CallService.Put<DocTemplateDtoResponse, DocTemplateDtoRequest>("documents/TemplateDocuments/UpdateTemplateDocument", DocTemplateformRequest, headers);
                if (response.Succeeded)
                {
                    notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Success, "se actualizo exitosamente el documento", "aceptar", true);
                    await OnChangeData.InvokeAsync(true);
                }
                else
                {

                    notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Error, "No se actualizo exitosamente el documento", "aceptar", true);

                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }

        }

        #endregion

        #region RecibirDataEditarDropDown
        private string NameTipoFormato(string selectedValue)
        {
            var name = selectedValue.Split(',');
            var selectedDataItem = FormatDoc.FirstOrDefault(item => item.code == name[1]);
            textFD = selectedDataItem.value;


            return textFD;
        }

        #endregion

        #region RecibirCampos

        public void RecibirRegistro(DocTemplateDtoResponse response)
        {
            _selectedRecord = response;

            IsDisabledCode = true;
            IsEditForm = true;
            visible = true;

            string  newdata = _selectedRecord.TempType;
            var name = newdata.Split(',');

          

            DocTemplateFormResponse.TempCode = _selectedRecord.TempCode;
            DocTemplateFormResponse.TempName = _selectedRecord.TempName;
            version = _selectedRecord.TempVersion.ToString();
            DocTemplateFormResponse.TempType = NameTipoFormato(_selectedRecord.TempType);
            DocTemplateFormResponse.Process = _selectedRecord.Process;
            DocTemplateFormResponse.ActiveState = _selectedRecord.ActiveState;

            DocTemplateformRequest.TempCode = _selectedRecord.TempCode;
            DocTemplateformRequest.TempName = _selectedRecord.TempName;
            DocTemplateformRequest.TempType = _selectedRecord.TempType;
            DocTemplateformRequest.Process = _selectedRecord.Process;
            DocTemplateformRequest.ActiveState = _selectedRecord.ActiveState;

            ValueFormatDoc = name[1];

        }
        #endregion

        #region ObtenerBase64Archivos
        private async Task HandleFilesList(List<FileInfoData> newList)
        {
            if (newList.Count > 0)
            {
                file = newList[0].Base64Data;
            }    
        }
        #endregion
    }
}
