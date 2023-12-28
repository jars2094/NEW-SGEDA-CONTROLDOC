using ControlDoc.Components.Components.Input;
using ControlDoc.Models.Models.Documents.Response;
using ControlDoc.Models.Models.Pagination;
using ControlDoc.Services.Interfaces;
using ControlDoc.Services.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDoc.ComponentViews.ComponentViews.Documents
{
    public partial class ModalMasterFormatList
    {
        #region Variables
        #region Injects
        [Inject]
        private IJSRuntime Js { get; set; }

        [Inject]
        private ICallService CallService { get; set; }
        #endregion

        #region Entorno
        private bool modalStatus = false;
        #endregion

        #region Models
        private Meta meta;

        private List<TemplateDocumentDtoResponse> templateDocumentList;
        #endregion

        #region Components
        public InputModalComponent tempCodeInput { get; set; }

        public InputModalComponent tempNameInput { get; set; }

        public InputModalComponent tempVersionInput { get; set; }

        public InputModalComponent processInput { get; set; }
        #endregion
        #endregion

        protected override async Task OnInitializedAsync()
        {
            PageLoadService.MostrarSpinnerReadLoad(Js);
            await GetTemplateDoc();
            PageLoadService.OcultarSpinnerReadLoad(Js);
        }

        public void UpdateModalStatus(bool newValue)
        {
            modalStatus = newValue;
            StateHasChanged();
        }

        private async Task HandleModalClosed(bool status)
        {
            modalStatus = status;
        }

        private async Task GetTemplateDoc()
        {
            try
            {
                var response = await CallService.Get<List<TemplateDocumentDtoResponse>>("documents/TemplateDocuments/ByFilter");
                templateDocumentList = response.Data??new();

                if(templateDocumentList.Count != 0) 
                {
                    meta = response.Meta;
                }
                else { Console.WriteLine("no se encontraron registros"); }

                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener tareas documentales: {ex.Message}");
            }
        }

        private async Task GetTemplateDocByFilter()
        {
            try
            {
                string code = string.IsNullOrEmpty(tempCodeInput.InputValue) ? "" : tempCodeInput.InputValue;
                string name = string.IsNullOrEmpty(tempNameInput.InputValue) ? "" : tempNameInput.InputValue;
                int version = string.IsNullOrEmpty(tempVersionInput.InputValue) ? 0 : int.Parse(tempVersionInput.InputValue);
                string process = string.IsNullOrEmpty(processInput.InputValue) ? "" : processInput.InputValue;

                Dictionary<string, dynamic> templatedoc = new()
                {
                    {"Code", code},
                    {"Version", version },
                    {"NameTemplate", name},
                    {"Process", process},
                };

                var response = await CallService.Get<List<TemplateDocumentDtoResponse>>("documents/TemplateDocuments/ByFilter", templatedoc);
                templateDocumentList = response.Data ?? new();

                if (templateDocumentList.Count != 0)
                {
                    meta = response.Meta;
                }
                else 
                { 
                    templateDocumentList = new();
                    Console.WriteLine("no se encontraron registros"); 
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener tareas documentales: {ex.Message}");
            }
        }
    }
}
