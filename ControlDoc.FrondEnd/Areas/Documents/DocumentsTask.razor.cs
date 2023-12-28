using ControlDoc.Components.Components.Input;
using ControlDoc.Models.Models.Administration.Response;
using ControlDoc.Models.Models.Documents.Response;
using ControlDoc.Models.Models.Generic;
using ControlDoc.Models.Models.Pagination;
using ControlDoc.Services.Interfaces;
using ControlDoc.Services.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Reflection.PortableExecutable;
using Telerik.Blazor.Components;
using Telerik.SvgIcons;

namespace ControlDoc.FrondEnd.Areas.Documents
{
    public partial class DocumentsTask
    {
        #region Variables
        #region Injects
        [Inject]
        private IJSRuntime Js { get; set; }

        [Inject]
        private ICallService CallService { get; set; }
        #endregion
        #endregion

        public DateTime? StartValue { get; set; } = DateTime.Now;
        public DateTime? EndValue { get; set; } = DateTime.Now.AddDays(10);
        public InputModalComponent docTaskInput { get; set; }
        public string descriptionInput { get; set; }
        public InputModalComponent userInput { get; set; }

        DateTime Min = new DateTime(1990, 1, 1, 8, 15, 0);
        DateTime Max = new DateTime(2025, 1, 1, 19, 30, 45);
        private Meta meta;
        private bool activeState = false;
        private string created = "";
        private string review = "";
        private string approve = "";
        private string signed = "";
        private string involved = "";

        private List<VDocumentaryTaskDtoResponse> documentaryTaskList;
        private DataCardsDocTaskDtoResponse dataCardsDocTask;

        protected override async Task OnInitializedAsync()
        {
            await GetDocumentsTask();
            await GetDataCards();
            PageLoadService.OcultarSpinnerReadLoad(Js);
        }

        private async Task GetDocumentsTask()
        {
            try
            {
                var response = await CallService.Get<List<VDocumentaryTaskDtoResponse>>("documentarytasks/DocumentaryTask/ByFilter");
                documentaryTaskList = response.Data??new();

                if(documentaryTaskList.Count != 0)
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

        private async Task GetDocumentsTaskFilter()
        {
            try
            {
                PageLoadService.MostrarSpinnerReadLoad(Js);
                int taskId =    string.IsNullOrEmpty(docTaskInput.InputValue) ? 0:  int.Parse(docTaskInput.InputValue);
                int userTaskId = string.IsNullOrEmpty(userInput.InputValue) ? 0: int.Parse(userInput.InputValue);
                string description = string.IsNullOrEmpty(descriptionInput) ? "": descriptionInput;

                Dictionary<string, dynamic> documentsTask = new()
                {
                    {"TaskId", taskId},
                    {"TaskDescription", description },
                    {"UserTaskId", userTaskId},
                    {"StartDate", (StartValue?.ToString("MM-dd-yyyy")??DateTime.MinValue.ToString("MM-dd-yyyy"))},
                    {"EndDate", (EndValue?.ToString("MM-dd-yyyy")??DateTime.MinValue.ToString("MM-dd-yyyy"))}
                };

                var response = await CallService.Get<List<VDocumentaryTaskDtoResponse>>("documentarytasks/DocumentaryTask/ByFilter", documentsTask);
                documentaryTaskList = response.Data??new();

                if(documentaryTaskList.Count != 0)
                {
                    meta = response.Meta;
                }
                else 
                {
                    documentaryTaskList = new();
                    Console.WriteLine("pasaron cosas"); 
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error al obtener tareas documentales: {ex.Message}");
            }
            PageLoadService.OcultarSpinnerReadLoad(Js);
        }

        private async Task GetDataCards()
        {
            try
            {
                int id = 3042;

                Dictionary<string, dynamic> user = new()
                {
                    {"AssingUserId", id}
                };

                var response = await CallService.Get<DataCardsDocTaskDtoResponse>("documentarytasks/DocumentaryTask/GetCountTask", user);
                dataCardsDocTask = response.Data;

                if (dataCardsDocTask != null)
                {
                    created = dataCardsDocTask.Created.ToString();
                    review = dataCardsDocTask.Review.ToString();
                    approve = dataCardsDocTask.Approve.ToString();
                    signed = dataCardsDocTask.Signed.ToString();
                    involved = dataCardsDocTask.Involved.ToString();
                }
                else
                {
                    created = "0";
                    review = "0";
                    approve = "0";
                    signed = "0";
                    involved = "0";
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error al obtener tareas documentales: {ex.Message}");
            }
        }

    }
}
