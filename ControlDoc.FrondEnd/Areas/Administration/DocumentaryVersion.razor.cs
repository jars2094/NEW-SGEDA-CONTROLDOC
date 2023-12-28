using ControlDoc.Components.Components.Modals;
using ControlDoc.ComponentViews.ComponentViews.Administration;
using ControlDoc.ComponentViews.ComponentViews.Documents;
using ControlDoc.Models.Models.Administration.Response;
using ControlDoc.Models.Models.Generic;
using ControlDoc.Services.Interfaces;
using ControlDoc.Services.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Telerik.Blazor.Components;

namespace ControlDoc.FrondEnd.Areas.Administration
{
    public partial class DocumentaryVersion
    {
        #region Variables
        #region Injects        
        [Inject]
        private IJSRuntime Js { get; set; }

        [Inject]
        private ICallService CallService { get; set; }
        #endregion
        #endregion

        private bool ClickPendiente = false;
        private PermissionDtoResponse recordToDelete { get; set; }


        #region GraficaDona

        #region VariablesDona
        //variables necesarias 
        public class ModelData
        {
            public string SegmentName { get; set; }
            public string Category { get; set; }
            public double SegmentValue { get; set; }
            public string color { get; set; }
            public bool ShouldShowInLegend { get; set; } = true;
        }
        #endregion VariablesDona

        public List<ModelData> Data = new List<ModelData>()
    {
           new ModelData()
        {
               SegmentName ="Pendientes",
            Category = "Pendientes",
            SegmentValue = 5,
            color = "#AB2222"
        },
        new ModelData()
        {
            SegmentName = "Anulados",
            Category = "Anulados",
            SegmentValue = 20,
            color = "#82A738"

        },
        new ModelData()
        {
            SegmentName = "Desanulados",
            Category = "Desanulados",
            SegmentValue = 40,
            color = "#41BAEA"

        },
        new ModelData()
        {
            SegmentName = "Rechazados",
            Category = "Rechazados",
            SegmentValue = 40,
            color = "#EAD519"

        }
    };



        #endregion


        private ModalDocumentaryVersion _ModalDocumentaryVersion;


        #region Initialization

        protected override async Task OnInitializedAsync()
        {

            await GetProfile();
            //TODO: Inicializa
            PageLoadService.OcultarSpinnerReadLoad(Js);
        }
        #endregion
        #region Metodos

        #region Cerrar spinerr
        //METODO PARA que se cierre el spiner
        private async Task GetProfile()
        {
            try
            {
                var response = await CallService.Get<List<PerfilesDtoResponse>>("permission/Profile/ByFilter");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener las Perfiles: {ex.Message}");
            }
        }
        #endregion



        private async Task MostrarModal()
        {

            _ModalDocumentaryVersion.UpdateModalStatusDocumentaryVersion(true);
            //TODO: Abre la modal
        }

        private async Task ShowModalEdit(PermissionDtoResponse args)
        {
            
            _ModalDocumentaryVersion.UpdateModalStatusDocumentaryVersion(true);
            

        }
        private void ShowModalDelete(PermissionDtoResponse record)
        {
            recordToDelete = record;
            


        }



        private async Task HandleModalNotiClose(ModalClosedEventArgs args)
        {
            if (args.IsAccepted)
            {
                
            }
            else
            {
                Console.WriteLine("Registro No eliminado");
            }


        }








        
        #region MetodosCard
        private async Task HandleClickCardPendiente(bool newvalue)
        {

            ClickPendiente = newvalue;

        }
        private async Task HandleClickCardAnulado(bool newvalue)
        {

            ClickPendiente = false;

        }
        private async Task HandleClickCardDesanulado(bool newvalue)
        {

            ClickPendiente = false;

        }
        private async Task HandleClickCardRechazado(bool newvalue)
        {

            ClickPendiente = false;

        }

        #endregion







        #endregion
        public void HandleRefreshGridData(bool refresh)
        {
            //TODO: Refresca la data de la grilla
        }

    }
}
