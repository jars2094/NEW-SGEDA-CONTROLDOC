using ControlDoc.Components.Components.Input;
using ControlDoc.ComponentViews.ComponentViews.Administration;
using ControlDoc.ComponentViews.ComponentViews.Documents;
using ControlDoc.Models.Models.Administration.Response;
using ControlDoc.Models.Models.Documents.Response;
using ControlDoc.Models.Models.Generic;
using ControlDoc.Models.Models.Pagination;
using ControlDoc.Services.Interfaces;
using ControlDoc.Services.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ControlDoc.FrondEnd.Areas.Documents
{
    public partial class ManagementTray
    {
        #region Inject

        // Inyección de dependencia del servicio IJSRuntime.
        [Inject] private IJSRuntime Js { get; set; }

        [Inject]
        private ICallService CallService { get; set; }
        #endregion

        #region Objects

        private DataCardsDtoResponse DataCards;
        private ModalAssociatedResourcesSearch modalAssociatedFilesSearch;
        private ModalSearchRegistration modalSearchRegistration;
        ModelData dona = new ModelData();

        private ModalManagementOfProcedures managementOfProcedures;
        #endregion

        #region Variables
        private int Total = 0;
        private Meta? meta;

        private bool activeState = false;
        public bool EnabledMes { get; set; } = false;
        public bool EnabledDia { get; set; } = false;
 
        private bool ClickPendiente = false;
        private string idcontrol { get; set; }
        private string Estado = "";
        private string NumRadica { get; set; }
        private int MounthValue { get; set; }
        private int YearValue { get; set; }
        private int DaysValue { get; set; }
        private string? ValuePrioridad { get; set; }

        private string textYear = "Seleccione un año...";
        private string textMes = "Seleccione un mes...";
        private string textDia = "Seleccione un dia...";

        private string EP;
        private string ET;
        private string GEX;
        #endregion

        #region Input
        private InputModalComponent IdcontrolInput { get; set; }
        private InputModalComponent NumRadicaInput { get; set; }
        #endregion

        #region List

        private List<SystemFieldsDtoResponse>? FormatBG { get; set; } = new ();
        private List<SystemFieldsDtoResponse>? FormatCL { get; set; } = new();
        private List<ManagementTrayDtoResponse>? DataGridEP { get; set; } = new();
        private List<ManagementTrayDtoResponse>? DataGridET { get; set; } = new();
        private List<ManagementTrayDtoResponse>? DataGridGEX { get; set; } = new();

        #region MesesList

        
        private List<FechaDtoResponse> Mounth = new() {
            new FechaDtoResponse{nombre = "Enero", valor= 1},
            new FechaDtoResponse{nombre = "Febrero", valor= 2},
            new FechaDtoResponse{nombre = "Marzo", valor= 3},
            new FechaDtoResponse{nombre = "Abril", valor= 4},
            new FechaDtoResponse{nombre = "Mayo", valor= 5},
            new FechaDtoResponse{nombre = "Junio", valor= 6},
            new FechaDtoResponse{nombre = "Julio", valor= 7},
            new FechaDtoResponse{nombre = "Agosto", valor= 8},
            new FechaDtoResponse{nombre = "Septiembre", valor= 9},
            new FechaDtoResponse{nombre = "Octubre", valor= 10},
            new FechaDtoResponse{nombre = "Noviembre", valor= 11},
            new FechaDtoResponse{nombre = "Diciembre", valor= 12}
        };
        #endregion

        #region AñosList
        private List<FechaDtoResponse> Year = new() {
            new FechaDtoResponse{nombre = "2030", valor= 2030},
            new FechaDtoResponse{nombre = "2029", valor= 2029},
            new FechaDtoResponse{nombre = "2028", valor= 2028},
            new FechaDtoResponse{nombre = "2027", valor= 2027},
            new FechaDtoResponse{nombre = "2026", valor= 2026},
            new FechaDtoResponse{nombre = "2025", valor= 2025},
            new FechaDtoResponse{nombre = "2024", valor= 2024},
            new FechaDtoResponse{nombre = "2023", valor= 2023}
        };
        #endregion

        #region DíasList
        private List<FechaDtoResponse> Days = new() {
            new FechaDtoResponse{nombre = "1", valor= 1},
            new FechaDtoResponse{nombre = "2", valor= 2},
            new FechaDtoResponse{nombre = "3", valor= 3},
            new FechaDtoResponse{nombre = "4", valor= 4},
            new FechaDtoResponse{nombre = "5", valor= 5},
            new FechaDtoResponse{nombre = "6", valor= 6},
            new FechaDtoResponse{nombre = "7", valor= 7},
            new FechaDtoResponse{nombre = "8", valor= 8},
            new FechaDtoResponse{nombre = "9", valor= 9},
            new FechaDtoResponse{nombre = "10", valor= 10},
            new FechaDtoResponse{nombre = "11", valor= 11},
            new FechaDtoResponse{nombre = "12", valor= 12},
            new FechaDtoResponse{nombre = "13", valor= 13},
            new FechaDtoResponse{nombre = "14", valor= 14},
            new FechaDtoResponse{nombre = "15", valor= 15},
            new FechaDtoResponse{nombre = "16", valor= 16},
            new FechaDtoResponse{nombre = "17", valor= 17},
            new FechaDtoResponse{nombre = "18", valor= 18},
            new FechaDtoResponse{nombre = "19", valor= 19},
            new FechaDtoResponse{nombre = "20", valor= 20},
            new FechaDtoResponse{nombre = "21", valor= 21},
            new FechaDtoResponse{nombre = "22", valor= 22},
            new FechaDtoResponse{nombre = "23", valor= 23},
            new FechaDtoResponse{nombre = "24", valor= 24},
            new FechaDtoResponse{nombre = "25", valor= 25},
            new FechaDtoResponse{nombre = "26", valor= 26},
            new FechaDtoResponse{nombre = "27", valor= 27},
            new FechaDtoResponse{nombre = "28", valor= 28},
            new FechaDtoResponse{nombre = "29", valor= 29},
            new FechaDtoResponse{nombre = "30", valor= 30},
            new FechaDtoResponse{nombre = "31", valor= 31}
        };
        #endregion

        #endregion List

        #region Initialization

        protected override async Task OnInitializedAsync()
        {
            PageLoadService.MostrarSpinnerReadLoad(Js);

            
            await GetDataCards();
            await GetPriority();
            await GetNotiComuni();
            await GetDataEP();

            PageLoadService.OcultarSpinnerReadLoad(Js);
        }

        #endregion Initialization

        #region DatePickerInicio
        private DateTime? SelectedDate { get; set; }
        private DateTime Max = new DateTime(2050, 12, 31);
        private DateTime Min = new DateTime(1950, 1, 1);
        private int DebounceDelay { get; set; } = 200;
        #endregion

        #region RefreshGrid

        private async Task HandleRefreshGridData(bool refresh)
        {
            //await GetCompanies();
        }

        #endregion

        #region OpenModal
        private async Task mostrarModal()
        {
            //modalAssociatedFilesSearch.UpdateModalStatus(true);
            modalSearchRegistration.UpdateModalStatus(true);
        }

        private async Task OpenModalManagementProcedures()
        {
            managementOfProcedures.UpdateModalStatus(true);
        }

        #endregion

        #region DataCards
        private async Task GetDataCards()
        {
            try
            {
                int id = 3045;
                
                Dictionary<string, dynamic> headers = new()
                {
                    {"AssingUserId", id}
                    
                };
                var response = await CallService.Get<DataCardsDtoResponse>("documentmanagement/Document/ByAssingUserId", headers);
                DataCards = response.Data;
                if (DataCards != null)
                {
                    EP = DataCards.withoutProcessing.ToString();
                    ET = DataCards.inProgress.ToString();
                    GEX = DataCards.successfulManagement.ToString();
                    Total = DataCards.withoutProcessing + DataCards.inProgress + DataCards.successfulManagement;

                    
                }
                else
                {
                    EP = "0";
                    ET = "0";
                    GEX = "0";
                    Total = 0;
                }     
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los tipos de documento: {ex.Message}");
            }
        }
        #endregion

        #region Data Grilla

        #region En proceso

        private async Task GetDataEP()
        {
            try
            {
                Estado = "En Proceso";
                int id = 3045;

                Dictionary<string, dynamic> headers = new()
                {
                    {"AssingUserId", id},
                    { "FlowStateFields","ES,SIT"}

                };
                var response = await CallService.Get<List<ManagementTrayDtoResponse>>("documentmanagement/Document/ByFilter", headers);
                DataGridEP = response.Data;
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los tipos de documento: {ex.Message}");
            }
        }
        #endregion

        #region GetDataET
        private async Task GetDataET()
        {
            try
            {
                Estado = "En Trámite";
                int id = 3045;

                Dictionary<string, dynamic> headers = new()
                {
                    {"AssingUserId", id},
                    { "FlowStateFields","ES,ETR"}

                };
                var response = await CallService.Get<List<ManagementTrayDtoResponse>>("documentmanagement/Document/ByFilter", headers);
                DataGridET = response.Data;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los tipos de documento: {ex.Message}");
            }
        }
        #endregion

        #region GetDataGEX
        private async Task GetDataGex()
        {
            try
            {
                Estado = "Gestión Exitosa";
                int id = 3045;

                Dictionary<string, dynamic> headers = new()
                {
                    {"AssingUserId", id},
                    { "FlowStateFields","ES,GEX"}

                };
                var response = await CallService.Get<List<ManagementTrayDtoResponse>>("documentmanagement/Document/ByFilter", headers);
                DataGridGEX= response.Data;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los tipos de documento: {ex.Message}");
            }
        }

        #endregion

        #endregion
       
        #region GraficaDona

        #region VariablesDona
        //variables necesarias 

        public class ModelData
        {
           

            public double SegmentValue { get; set; }
            
          
            public string color { get; set; }
        }
        #endregion VariablesDona

        public List<ModelData> Data = new List<ModelData>()
        {
           new ModelData()
        {
               
            SegmentValue = 10,
            color = "#AB2222"
        },
        new ModelData()
        {
           
            
            SegmentValue = 20,
            color = "#EAD519"

        },
        new ModelData()
        {
           
            SegmentValue = 40,
            color = "#82A738"

        }
    };



        #endregion

        #region Cascading drop
        private async Task cascadingMes()
        {
            EnabledMes = true;
        }
        private async Task cascadingDia()
        {
            EnabledDia = true;
        } 
        #endregion

        #region Dropdownlist Prioridad
        private async Task GetPriority()
        {
            try
            {
                Dictionary<string, dynamic> headers = new()
                {
                     { "ParamCode","RPRI" }
                };

                var response = await CallService.Get<List<SystemFieldsDtoResponse>>("params/SystemFields/ByFilter", headers);
                FormatBG = response.Data;

                meta = response.Meta;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener la prioridad: {ex.Message}");
            }
        }
        #endregion

        #region Dropdownlist Prioridad
        private async Task GetNotiComuni()
        {
            try
            {
                Dictionary<string, dynamic> headers = new()
                {
                     { "ParamCode","CL" }
                };

                var response = await CallService.Get<List<SystemFieldsDtoResponse>>("params/SystemFields/ByFilter", headers);
                FormatCL = response.Data;

                meta = response.Meta;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener la prioridad: {ex.Message}");
            }
        }
        #endregion
    }
}
