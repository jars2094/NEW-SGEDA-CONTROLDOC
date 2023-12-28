using ControlDoc.Components.Components.DropDownListTelerik;
using ControlDoc.Components.Components.Modals;
using ControlDoc.ComponentViews.ComponentViews.Administration;
using ControlDoc.Models.Models.Administration.Response;
using ControlDoc.Models.Models.Generic;
using ControlDoc.Models.Models.Pagination;
using ControlDoc.Services.Interfaces;
using ControlDoc.Services.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace ControlDoc.FrondEnd.Areas.Administration
{
    public partial class Series
    {
        #region Variables
        #region Injects
        [Inject]
        private IJSRuntime Js { get; set; }

        [Inject] 
        private ICallService CallService { get; set; }
        #endregion
        #endregion

        #region Initialization


        private int IdproOffice { get; set; }

        private bool esHabilitado = true;
        private ModalSeries modalSeries;
        private List<SeriesDtoResponse> seriesList;
        private Meta seriesMeta;
        private bool dataChargue;
        private List<ProductionOfficesDtoResponse> productionOfficesList;
        private Meta meta;

        private SeriesDtoResponse recordToDelete;
        private ModalNotificationsComponent notificationModal;
        private ModalNotificationsComponent notificationModalSucces;

        //capturar id de la serie
        private DropDownListTelerik<ProductionOfficesDtoResponse> proOfficeDW { get; set; }

        private Dictionary<string, dynamic> SeriesHeader { get; set; } = new Dictionary<string, dynamic>();

        private int proOfficeSelectedValue;

        protected override async Task OnInitializedAsync()
        {
            
            await GetProductionOffices();
            PageLoadService.OcultarSpinnerReadLoad(Js);
        }

        #endregion Initialization

        private async Task mostrarModal()
        {
            await modalSeries.PreparedModal();
            modalSeries.UpdateModalStatus(true);
        }



        private void ShowModalEdit(SeriesDtoResponse record)
        {

            modalSeries.UpdateModalStatus(true);
            modalSeries.UpdateSelectedRecord(record);
        }

      


        private async Task GetProductionOffices()
        {
            try
            {
                var response = await CallService.Get<List<ProductionOfficesDtoResponse>>("paramstrd/ProductionOffice/ByFilter");
                productionOfficesList = response.Data;
                meta = response.Meta;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener las oficinas productoras: {ex.Message}");
            }
        }

        private async Task OnDropDownValueChanged(int newValue)
        {
            PageLoadService.MostrarSpinnerReadLoad(Js);
            IdproOffice = newValue;
            Dictionary<string, dynamic> productionOffice = new()
                {
                    {"productionOfficeId", IdproOffice }
                };

            esHabilitado = (IdproOffice <= 0) ? true : false;
            var response = await CallService.Get<List<SeriesDtoResponse>>("paramstrd/Series/ByFilter", productionOffice);
            seriesList = response.Data;
            seriesMeta = response.Meta;
            dataChargue = true;
            PageLoadService.OcultarSpinnerReadLoad(Js);
        }

        private async Task HandleChangedData(bool changed)
        {
            await OnDropDownValueChanged(IdproOffice);
        }


        private async Task ShowModalDelete(SeriesDtoResponse record)
        {
            recordToDelete = record;
            notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Delete, "Esta seguro de eliminar el mensaje", "Aceptar", true);
        }


        private async Task HandleModalNotiClose(ModalClosedEventArgs args)
        {
            PageLoadService.MostrarSpinnerReadLoad(Js);
            if (args.IsAccepted)
            {

                Dictionary<string, dynamic> headerId = new() { { "SerieId", recordToDelete.SeriesId } };

                var response = await CallService.Put<int, int>("paramstrd/Series/DeleteSeries", 0, headerId);

                await OnDropDownValueChanged(IdproOffice);
                notificationModalSucces.UpdateModal(ModalNotificationsComponent.ModalType.Success, "Se ha borrado el registro existosamente", "Aceptar", true);


            }
            else
            {
                Console.WriteLine("Registro No eliminado");
            }
            PageLoadService.OcultarSpinnerReadLoad(Js);

        }

        private void HandlePaginationGrid(List<SeriesDtoResponse> newDataList)
        {
            seriesList = newDataList;
        }


    }
}