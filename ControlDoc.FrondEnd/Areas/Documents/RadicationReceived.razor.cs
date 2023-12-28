using ControlDoc.Components.Components.DropDownListTelerik;
using ControlDoc.Components.Components.Input;
using ControlDoc.Components.Components.Modals;
using ControlDoc.ComponentViews.ComponentViews.Administration;
using ControlDoc.ComponentViews.ComponentViews.Documents;
using ControlDoc.ComponentViews.ComponentViews.GenericViews;
using ControlDoc.Models.Models.Administration.Response;
using ControlDoc.Models.Models.Documents.Response;
using ControlDoc.Models.Models.Generic;
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
using Telerik.Blazor.Components;
using Telerik.SvgIcons;
using static Telerik.Blazor.ThemeConstants;

namespace ControlDoc.FrondEnd.Areas.Documents
{
    public partial class RadicationReceived
    {

        #region Variables
        #region Injects
        [Inject]
        private IJSRuntime Js { get; set; }

        [Inject]
        private ICallService CallService { get; set; }
        #endregion

        private string? ValueTipoDocumento { get; set; }
        private string? ValuePrioridad { get; set; } = "SE";
        private int ValuePais { get; set; }
        private int ValueDepartamento { get; set; }
        private int ValueMunicipio { get; set; }
        private DateTime ValueFechaDocumento { get; set; } = DateTime.Now;
        private string? ValueNotificacion { get; set; }

        private bool EnabledDepartamento { get; set; } = true;
        private bool EnabledMunicipio { get; set; } = true;
        
        private List<SystemFieldsDtoResponse>? lstTipoDoc { get; set; } = new List<SystemFieldsDtoResponse>();
        private List<SystemFieldsDtoResponse>? lstPrioridad { get; set; } = new List<SystemFieldsDtoResponse>();
        private List<CountryDtoResponse>? lstPais { get; set; } = new List<CountryDtoResponse>();
        private List<StateDtoResponse>? lstDepartamento { get; set; } = new List<StateDtoResponse>();
        private List<CityDtoResponse>? lstMunicipio { get; set; } =  new List<CityDtoResponse>();
        private List<SystemFieldsDtoResponse>? lstNotificacion { get; set; } = new List<SystemFieldsDtoResponse>();
        private FilingAttachmentsModel? Attachments { get; set; } = new FilingAttachmentsModel();

        private Meta? meta;

        private GenericDocTypologySearchModal docTypologySearchModal;
        private ModalCommunicationReceived communicationReceivedModal;
        private ModalNotificationsComponent notificationModal;
        private ModalVisualizacionMetadatos visualizacionMetadatos;
        private GenericSearchModal genericSearchModal { get; set; } = new();
        private List<PersonInRadication> listOfRadication { get; set; } = new();

        //Variable encargada de obtener el objeto TRD
        private VDocumentaryTypologyResponse TRDSelected = new();

        //Variables publicas de la vista
        List<FileInfoData> lstFileInfoData = new List<FileInfoData>();
        DateTime Min = new DateTime(1990, 1, 1, 8, 15, 0);
        DateTime Max = new DateTime(2025, 1, 1, 19, 30, 45);
        Decimal contadorcarac = 0;
        string Radicado = "";
        string IdDocumento = "";
        string Anio = "";

        //Variables para ocultar paneles
        private Dictionary<string,string> Panel1 = new Dictionary<string, string>();
        private Dictionary<string,string> Panel2 = new Dictionary<string, string>();
        private Dictionary<string,string> Panel4 = new Dictionary<string, string>();

        string panel_1 = "flex";
        string panel_2 = "none";
        string panel_3 = "none";
        string panel_4 = "none";
        string panel_5 = "none";
        #endregion

        #region Initialization
        protected override async Task OnInitializedAsync()
        {
            
            try
            {
                await GetDocumentTypeTCR();
                await GetPriority();
                await GetCountry();
                await GetNotification();
                GeneratePanels();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al realizar la initialización: {ex.Message}");
            }
            PageLoadService.OcultarSpinnerReadLoad(Js);
        }

        #endregion

        #region General Methods
        private async Task showModal()
        {
            docTypologySearchModal.UpdateModalStatus(true);
        }   
        private async Task GenerarRadicadoTemp() //Esto debe ser un submit
        {
            Radicado = "R45678-211544-2023";
            IdDocumento = "12457";
            Anio = DateTime.Now.Year.ToString();
        }
        private async Task showModalAttachments()
        {
             communicationReceivedModal.UpdateModalStatus(true);
        }

        private async Task showModalMetadatos()
        {
            visualizacionMetadatos.UpdateModalStatus(true);
        }

        private async Task GetDocumentTypeTCR()
        {
            try
            {
                Dictionary<string, dynamic> headers = new()
                {
                     { "ParamCode","TCR" }
                };

                var response = await CallService.Get<List<SystemFieldsDtoResponse>>("params/SystemFields/ByFilter", headers);
                lstTipoDoc = response.Data;

                meta = response.Meta;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los tipos de documento: {ex.Message}");
            }
        }
        private async Task GetPriority()
        {
            try
            {
                Dictionary<string, dynamic> headers = new()
                {
                     { "ParamCode","RPRI" }
                };

                var response = await CallService.Get<List<SystemFieldsDtoResponse>>("params/SystemFields/ByFilter", headers);
                lstPrioridad = response.Data;

                meta = response.Meta;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener la prioridad: {ex.Message}");
            }
        }
        private async Task GetCountry()
        {
            try
            {
                var response = await CallService.Get<List<CountryDtoResponse>>("location/Country/ByFilter");
                lstPais = response.Data != null ? response.Data : new List<CountryDtoResponse>();

                if (lstPais.Count > 0)
                {
                    meta = response.Meta;
                    EnabledDepartamento = false;
                    EnabledMunicipio = false;
                }
                else
                {
                    EnabledDepartamento = false;
                    EnabledMunicipio = false;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el país: {ex.Message}");
            }
        }
        private async Task GetState()
        {
            try
            {
                if (ValuePais > 0)
                {
                    EnabledDepartamento = true;
                    EnabledMunicipio = false;

                    Dictionary<string, dynamic> headers = new()
                    {
                         { "countryId",ValuePais }
                    };

                    var response = await CallService.Get<List<StateDtoResponse>>("location/State/ByFilter",headers);
                    lstDepartamento = response.Data != null ? response.Data: new List<StateDtoResponse>();

                    if (lstDepartamento.Count > 0)
                    {
                        meta = response.Meta;
                        ValueMunicipio = 0;
                    }
                    else
                    {
                        EnabledDepartamento = false;
                        EnabledMunicipio = false;
                    }

                }
                else
                {
                    ValueDepartamento = 0;
                    ValueMunicipio = 0;
                    EnabledDepartamento = false;
                    EnabledMunicipio = false;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el departamento: {ex.Message}");
            }
        }
        private async Task GetCity()
        {
            try
            {
                if (ValueDepartamento > 0)
                {
                    EnabledMunicipio = true;

                    Dictionary<string, dynamic> headers = new()
                    {
                         { "stateId",ValueDepartamento }
                    };

                    var response = await CallService.Get<List<CityDtoResponse>>("location/City/ByFilter",headers);
                    lstMunicipio = response.Data != null ? response.Data : new List<CityDtoResponse>();

                    if (lstMunicipio.Count > 0)
                    {
                        meta = response.Meta;
                    }
                    else
                    {
                        EnabledMunicipio = false;
                    }
                }
                else
                {
                    ValueMunicipio = 0;
                    EnabledMunicipio = false;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el municipio: {ex.Message}");
            }
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

        #endregion

        #region Validation Methods
        private void ContarCaracteres(ChangeEventArgs e)
        {
            String value = e.Value.ToString() ?? String.Empty;

            if (!string.IsNullOrEmpty(value))
            {
                contadorcarac = value.Length;
                ActivarPanel(value, "ASUNTO", 2);
            }
            else
            {
                contadorcarac = 0;
                ActivarPanel(value, "ASUNTO", 2);
            }
        }
        private void GeneratePanels()
        {
            #region PANEL 1
            Panel1["TIPODOCUMENTO"] = ""; 
            Panel1["TRAMITEGESTOR"] = "";
            #endregion

            #region PANEL 1
            Panel2["PAIS"] = ""; 
            Panel2["DEPARTAMENTO"] = "";
            Panel2["MUNICIPIO"] = "";
            //Panel2["FECHADOCUMENTO"] = "";
            Panel2["FOLIOS"] = "";
            Panel2["NDOCUMENTOINTERNO"] = "";
            //Panel2["METADATOS"] = "";
            Panel2["ASUNTO"] = "";
            #endregion

            #region PANEL 4
            Panel4["DESTINATARIOS"] = "OK";
            #endregion
        }
        private void ActivarPanel(string value, string componente, decimal panel)
        {
            bool habPanel2 = false;
            bool habPanel3 = false;
            bool habPanel5 = false;

            switch (panel)
            {
                case 1:
                    if (Panel1.ContainsKey(componente))
                    {
                        Panel1[componente] = value;
                        ValueTipoDocumento = componente == "TIPODOCUMENTO" ? Panel1[componente] : ValueTipoDocumento != "" ? ValueTipoDocumento : "" ;                        
                    }

                    habPanel2 = Panel1.Values.All(x => !string.IsNullOrEmpty(x) && x != "0");
                    habPanel3 = Panel2.Values.All(x => !string.IsNullOrEmpty(x) && x != "0");
                    habPanel5 = Panel4.Values.All(x => !string.IsNullOrEmpty(x) && x != "0");


                    if (habPanel2) //Habilita el paso 2
                    {
                        panel_2 = "flex";

                        if (habPanel3) //Habilita el paso 3 solo si el paso 2 esta completo
                        {
                            panel_3 = panel_4 = "flex";

                            if (habPanel5) //Habilita el paso 4 solo si el paso 4 esta completo
                            {
                                panel_5 = "flex";
                            }
                            else
                            {
                                panel_5 = "none";
                            }
                        }
                        else
                        {
                            panel_3 = panel_4 = "none";
                        }
                    }
                    else
                    {
                        panel_2 = panel_3 = panel_4 = panel_5 = "none";
                    }
                    break;
                case 2:
                    if (Panel2.ContainsKey(componente))
                    {
                        Panel2[componente] = value;
                        ValuePais = componente == "PAIS" ? Convert.ToInt32(Panel2[componente]) : ValuePais > 0 ? ValuePais : 0;
                        ValueDepartamento = componente == "DEPARTAMENTO" ? Convert.ToInt32(Panel2[componente]) : ValueDepartamento > 0 ? ValueDepartamento : 0;
                        ValueMunicipio = componente == "MUNICIPIO" ? Convert.ToInt32(Panel2[componente]) : ValueMunicipio > 0 ? ValueMunicipio : 0;

                        if(ValuePais == 0)
                        {
                            Panel2["DEPARTAMENTO"] = "";
                            Panel2["MUNICIPIO"] = "";
                        }
                    }

                    habPanel3 = Panel2.Values.All(x => !string.IsNullOrEmpty(x) && x != "0");
                    habPanel5 = Panel4.Values.All(x => !string.IsNullOrEmpty(x) && x != "0");

                    if (habPanel3) //Habilita el paso 3
                    {
                        panel_3 = panel_4 = "flex";

                        if (habPanel5) //Habilita el paso 5 solo si el paso 4 esta completo
                        {
                            panel_5 = "flex";
                        }
                        else
                        {
                            panel_5 = "none";
                        }
                    }
                    else
                    {
                        panel_3 = panel_4 = panel_5 = "none";
                    }
                    break;
            }
        }
        #endregion

        #region Modals Methods
        private void HandleTRDSelectedChanged(MyEventArgs<VDocumentaryTypologyResponse> trd)
        {
            docTypologySearchModal.resetModal();
            docTypologySearchModal.UpdateModalStatus(trd.ModalStatus);
            TRDSelected = trd.Data;
        }

        private void HandleMetaDataSelected(bool a)
        {
            visualizacionMetadatos.UpdateModalStatus(a);
        }

        private async Task HandleFilingAttachments(FilingAttachmentsModel data)
        {
            Attachments = data;

            if (Attachments.Files.Count > 0)
            {
                foreach (var item in Attachments.Files)
                {
                    FileInfoData fileData = new FileInfoData()
                    {
                        Name = item.Name,
                        Extension = item.Extension,
                        Size = item.Size,
                        IconPath = item.IconPath,
                        Description = Attachments.Details.ToString()
                    };

                    lstFileInfoData.Add(fileData);
                }

                if (lstFileInfoData.Count > 0)
                {
                    notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Success, "Adjuntos agregados satisfactoriamente", "Aceptar", true);
                }
                else
                {
                    notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Error, "No se cargaron los adjuntos satisfactoriamente", "Aceptar", true);
                }


            }

            await Task.Delay(1000);

        }
        private void HandleRadicationChanged(MyEventArgs<List<PersonInRadication>> listOfRadicationUsers)
        {
            listOfRadication = new();
            listOfRadication = listOfRadicationUsers.Data;
            genericSearchModal.UpdateModalStatus(listOfRadicationUsers.ModalStatus);
        }
        private async Task showRecipient()
        {
            genericSearchModal.UpdateModalStatus(true);
        }




        #endregion

    }
}
