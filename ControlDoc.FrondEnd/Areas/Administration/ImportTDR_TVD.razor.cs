using ControlDoc.ComponentViews.ComponentViews.Administration;
using ControlDoc.ComponentViews.ComponentViews.Documents;
using ControlDoc.Models.Models.Administration.Response;
using ControlDoc.Models.Models.Generic;
using ControlDoc.Services.Interfaces;
using ControlDoc.Services.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Telerik.SvgIcons;

namespace ControlDoc.FrondEnd.Areas.Administration
{
    public partial class ImportTDR_TVD
    {
        #region Variables
        #region Injects
        [Inject] 
        private IJSRuntime Js { get; set; }
        
        [Inject]
        private ICallService CallService { get; set; }
        #endregion
        #endregion

        private ModalImportTDR_TVD _modalImportTDR_TVD;
        public class ModelData
        {
            public string Descripcion { get; set; }
            public string Estado { get; set; }
            public string Detalle { get; set; }
            public string NombreArchivo { get; set; }
            public int NumeroRegistro { get; set; }
        }


        public List<ModelData> Data = new List<ModelData>()
    {
           new ModelData()
        {
                Descripcion ="1",
                Estado = "001",
                Detalle = "Jose",
                NombreArchivo = "Si",
                NumeroRegistro=1
        },
        new ModelData()
        {
              Descripcion ="2",
              Estado = "002",
              Detalle = "Juan",
              NombreArchivo = "Si",
              NumeroRegistro=2

        },
        new ModelData()
        {
              Descripcion ="3",
              Estado = "003",
              Detalle = "Johan",
              NombreArchivo = "Si",
              NumeroRegistro=3

        },
        new ModelData()
        {
              Descripcion ="4",
              Estado = "004",
              Detalle = "jj",
              NombreArchivo = "Si",
              NumeroRegistro=4

        }
    };

        protected override async Task OnInitializedAsync()
        {

            await GetProfile();
            //TODO: Inicializa
            PageLoadService.OcultarSpinnerReadLoad(Js);
        }

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


        private async Task MostrarModalImport()
        {

            _modalImportTDR_TVD.UpdateModalStatusImport(true);
            //TODO: Abre la modal
        }

        public void HandleRefreshGridDataImport(bool refresh)
        {
            //TODO: Refresca la data de la grilla
        }







        private async Task HandleModalNotiClose(ModalClosedEventArgs args)
        {
            if (args.IsAccepted)
            {
                //TODO: notificacion de cerrar modal

            }
            else
            {
                Console.WriteLine("Registro No eliminado");
            }


        }
       







        #endregion
        public void HandleRefreshGridData(bool refresh)
        {
            //TODO: Refresca la data de la grilla
        }

    }
}

