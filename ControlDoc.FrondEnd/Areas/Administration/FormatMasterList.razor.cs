using ControlDoc.Components.Components.Input;
using ControlDoc.Components.Components.Modals;
using ControlDoc.ComponentViews.ComponentViews.Administration;
using ControlDoc.Models.Models.Administration.Response;
using ControlDoc.Models.Models.Generic;
using ControlDoc.Models.Models.Pagination;
using ControlDoc.Services.Interfaces;
using ControlDoc.Services.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Xml.Linq;
using Telerik.Blazor.Components;
using Telerik.SvgIcons;

namespace ControlDoc.FrondEnd.Areas.Administration
{
    public partial class FormatMasterList
    {
        #region Variables

        #region Inject
        [Inject] private IJSRuntime Js { get; set; }

        [Inject] private ICallService CallService { get; set; }

        #endregion

        #endregion

        #region List

        private List<DocTemplateDtoResponse> DocTemplatelist;
        private List<DocTemplateDtoResponse> DocTemplatelistCode;
        private List<SystemFieldsDtoResponse> FormatDoc;

        private DocTemplateDtoResponse DocTemplate;

        #endregion List

        #region Variables

        private string ValueFormatDoc { get; set; }
        private string code { get; set; }
        private string version { get; set; }
        private string name { get; set; }
        private string process { get; set; }

        private string textFD = "Seleccione un tipo de formato...";

        #endregion Variables

        #region Objects

        private ModalFormatMasterList modalFormatMasterList;
        private Meta meta;
       

        public bool SecondGrid { get; private set; }

        private DocTemplateDtoResponse TemplateForm = new DocTemplateDtoResponse();

        #endregion Objects

        #region Variables Inputs

        private InputModalComponent CodeInput { get; set; }
        private InputModalComponent VersionInput { get; set; }
        private InputModalComponent NametemplateInput { get; set; }
        private InputModalComponent ProcessInput { get; set; }
        public object JsRuntime { get; private set; }

        #endregion Variables Inputs

        #region ModalListadoMaestro

        private void ShowModalEdit(DocTemplateDtoResponse record)
        {
            modalFormatMasterList.UpdateModalStatus(true);
            modalFormatMasterList.RecibirRegistro(record);
        }

        private async Task ShowModalDelete(DocTemplateDtoResponse record)
        {
            TemplateForm = record;
            notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Delete, "¿Esta seguro que desea eliminar la compañía?", "Aceptar", true);
        }

        #endregion ModalListadoMaestro

        #region Clear
        public async Task ResetFiltersAsync()
        {
            code = "";
            version = "";
            name = "";
            process = "";
            ValueFormatDoc = "";
            await GetMasterList();

            StateHasChanged();
        }

        #endregion

        #region ShowFile
        private async Task ShowFileAsync(DocTemplateDtoResponse record)
        {
            var id = record.FileId;
            var tipoDescarga = true;
            try
            {
                Dictionary<string, dynamic> headers = new()
                {
                    {"FileId", id}
                };

                var response = await CallService.Get<FileDtoResponse>("file/File/ByIdBase", headers);
                var data  = response.Data;

                if (data != null)
                {
                    meta = response.Meta;
                    await Js.InvokeVoidAsync("abrirNuevoTabUrl",data.FileName , data.FileExt, data.Archivo, tipoDescarga);
                }
                else { Console.Write(response.Message); }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el listado maestro de formatos: {ex.Message}");
            }
           
        }
        #endregion

        #region Initialization

        protected override async Task OnInitializedAsync()
        {
            PageLoadService.MostrarSpinnerReadLoad(Js);
            await GetMasterList();
            await GetFormatDoc();

            PageLoadService.OcultarSpinnerReadLoad(Js);
        }

        #endregion Initialization

        #region OpenModal

        private async Task mostrarModal()
        {
            modalFormatMasterList.UpdateModalStatus(true);
        }

        #endregion OpenModal

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

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los tipos de documento: {ex.Message}");
            }
        }

        #endregion DropDownListFormatDoc

        #region Filtro de búsqueda

        public async Task GetByFilter()
        {
            try
            {
                Dictionary<string, dynamic> headers = new()
                {
                    {"Code", CodeInput.InputValue??""},
                    {"Version", int.Parse(VersionInput.InputValue ?? "0") },
                    {"NameTemplate", NametemplateInput.InputValue??""},
                    {"Type", "TFOR,"+ValueFormatDoc??""},
                    {"Process", ProcessInput.InputValue??"" },
                };

                var response = await CallService.Get<List<DocTemplateDtoResponse>>("documents/TemplateDocuments/ByFilter", headers);
                DocTemplatelist = response.Data;

                if (DocTemplatelist.Count != 0)
                {
                    meta = response.Meta;
                }
                else { Console.Write(response.Message); }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el listado maestro de formatos: {ex.Message}");
            }
        }

        #endregion Filtro de búsqueda

        #region GetlistadoMaestro

        public async Task GetMasterList()
        {
            try
            {
                var response = await CallService.Get<List<DocTemplateDtoResponse>>("documents/TemplateDocuments/ByFilters", null);
                DocTemplatelist = response.Data;

                if (DocTemplatelist.Count != 0)
                {
                    meta = response.Meta;
                }
                else { Console.Write(response.Message); }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el listado maestro de formatos: {ex.Message}");
            }
        }

        #endregion GetlistadoMaestro

        #region DeleteGrid

        private async Task HandleModalNotiClose(ModalClosedEventArgs args)
        {
            if (args.IsAccepted)
            {
                Dictionary<string, dynamic> TempId = new() { { "idTemplate", TemplateForm.TemplateId } };

                var response = CallService.Put<int, int>("documents/TemplateDocuments/DeleteTemplate", 0, TempId);

                notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Success, "Se ha eliminado correctamente el registro.", "Aceptar", true);
                await HandleRefreshGridData(true);
            }
            else
            {
                Console.WriteLine("Registro No eliminado");
            }
        }

        #endregion DeleteGrid

        #region PaginationGrid
        private void HandlePaginationGrid(List<DocTemplateDtoResponse> newDataList)
        {
            DocTemplatelist = newDataList;
        } 
        #endregion

        #region Actualizar Grilla

        private async Task HandleRefreshGridData(bool refresh)
        {
            await GetMasterList();
        }

        #endregion Actualizar Grilla

        #region Methods Modal Notifications

        private bool modalStatus = false;

        private ModalNotificationsComponent notificationModal;

        public void UpdateModalStatus(bool newValue)
        {
            modalStatus = newValue;
            StateHasChanged();
        }

        #endregion Methods Modal Notifications

        #region PruebaSecond
        async Task OnRowExpandHandler(GridRowExpandEventArgs args)
        {
        

            try
            {
                DocTemplateDtoResponse record = args.Item as DocTemplateDtoResponse;
                Dictionary<string, dynamic> headers = new()
                {
                    {"Code", record.TempCode}
                };

                var response = await CallService.Get<List<DocTemplateDtoResponse>>("documents/TemplateDocuments/ByFilterCode", headers);
                DocTemplatelistCode = response.Data;

                if (DocTemplatelistCode.Count != 0)
                {
                    meta = response.Meta;
                    SecondGrid = true;
                }
                else { Console.Write(response.Message); }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el listado maestro de formatos: {ex.Message}");
            }
        }





        #endregion
    }
}