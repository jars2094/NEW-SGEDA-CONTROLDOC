using ControlDoc.Models.Models.Administration.Request;
using ControlDoc.Models.Models.Administration.Response;
using ControlDoc.Models.Models.Generic;
using ControlDoc.Models.Models.Pagination;
using ControlDoc.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDoc.ComponentViews.ComponentViews.Documents
{
    public partial class ModalCommunicationReceived : ComponentBase
    {
        #region Variables
        #region Injects
        [Inject]
        private ICallService CallService { get; set; }
        #endregion
        #endregion

        private bool modalStatus = false;
        private bool SelectDeliveryCertificate;
        private bool SelectReasonForReturn;
        Decimal contadorcarac = 0;
        [Parameter]
        public EventCallback<FilingAttachmentsModel> OnReturnFilingAttachmentsModel { get; set; }

        private List<SystemFieldsDtoResponse>? lstNotificacion { get; set; } = new List<SystemFieldsDtoResponse>();
        private string? ValueNotificacion { get; set; }

        private FilingAttachmentsModel FilingAttachmentsModel = new FilingAttachmentsModel();
        private Meta? meta;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await GetNotification();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al realizar la initialización: {ex.Message}");
            }
        }
        private async Task HandleValidSubmit()
        {
            await OnReturnFilingAttachmentsModel.InvokeAsync(FilingAttachmentsModel);
            FilingAttachmentsModel = new FilingAttachmentsModel();
            modalStatus = false;
            contadorcarac = 0;
            SelectDeliveryCertificate = false;
            SelectReasonForReturn = false;
            StateHasChanged();
        }

        private async Task GetNotification()
        {
            try
            {
                Dictionary<string, dynamic> headers = new()
                {
                     { "ParamCode","RNOTI" }
                };

                var response = await CallService.Get<List<SystemFieldsDtoResponse>>("params/SystemFields/ByFilter", headers);
                lstNotificacion = response.Data;

                meta = response.Meta;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener la notificación: {ex.Message}");
            }
        }

        private List<string> DropDownData { get; set; } = new List<string> {
        "Manager", "Developer", "QA", "Technical Writer", "Support Engineer"
        };

        private string DropDownValue { get; set; } = "Developer";        

        private void OnDropDownValueChanged(string newValue)
        {
            DropDownValue = newValue;            
        }

        private void ContarCaracteres(ChangeEventArgs e)
        {
            contadorcarac = !string.IsNullOrEmpty(e.Value.ToString()) ? e.Value.ToString().Length : 0;
        }

        private void HandleModalClosed(bool status)
        {
            modalStatus = status;
            
            StateHasChanged();
        }

        public void UpdateModalStatus(bool newValue)
        {
            modalStatus = newValue;
            StateHasChanged();
        }

        private async Task HandleFilesList(List<FileInfoData>  newList)
        {
            FilingAttachmentsModel.Files = newList;
        }
    }
}
