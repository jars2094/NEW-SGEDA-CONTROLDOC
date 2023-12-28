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

namespace ControlDoc.FrondEnd.Areas.Administration
{
    public partial class SubSeries
    {
        #region Variables
        #region Injects
        [Inject]
        private IJSRuntime Js { get; set; }

        [Inject] 
        private ICallService CallService { get; set; }
        #endregion
        #endregion

        private int IdSerie { get; set; }

        private bool esHabilitado = true;
        private ModalSubSeries modalSubseries;

        private List<SubSeriesDtoResponse> subSeriesList;
        private List<SeriesDtoResponse> seriesList;
        private Meta meta;
        private Meta metasubSeries;
        private bool dataChargue = false;

        private SubSeriesDtoResponse recordToDelete;
        private ModalNotificationsComponent notificationModal;

        private ModalNotificationsComponent notificationModalSucces;

        //capturar id de la serie
        private DropDownListTelerik<SeriesDtoResponse> serieDW { get; set; }
        private Dictionary<string, dynamic> SubSerie { get; set; } = new Dictionary<string, dynamic>();

        private int serieSelectedValue;

        protected override async Task OnInitializedAsync()
        {
            
            await GetSeries();
            PageLoadService.OcultarSpinnerReadLoad(Js);
        }

        #region Methods

        private async Task mostrarModal()
        {
            await modalSubseries.PreparedModal();
            modalSubseries.UpdateModalStatus(true);
        }

        private void ShowModalEdit(SubSeriesDtoResponse record)
        {
            modalSubseries.UpdateModalStatus(true);
            modalSubseries.UpdateSelectedRecord(record);
        }

        private async Task ShowModalDelete(SubSeriesDtoResponse record)
        {
            recordToDelete = record;
            notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Delete, "¿Estas seguro de borrar el registro?", "Aceptar", true);
             
        }

        private async Task GetSeries()
        {
            
            try
            {
                var response = await CallService.Get<List<SeriesDtoResponse>>("paramstrd/Series/ByFilter");
                seriesList = response.Data;
                meta = response.Meta;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener las series: {ex.Message}");
            }
        }

        private async Task OnDropDownValueChanged(int newValue)
        {
            PageLoadService.MostrarSpinnerReadLoad(Js);
            IdSerie = newValue;
            SubSerie = new()
                {
                    {"seriesId", IdSerie }
                };

            esHabilitado = (IdSerie <= 0) ? true : false;
            var response = await CallService.Get<List<SubSeriesDtoResponse>>("paramstrd/SubSeries/ByFilter", SubSerie);
            subSeriesList = response.Data;
            metasubSeries = response.Meta;
            dataChargue = true;
            PageLoadService.OcultarSpinnerReadLoad(Js);

        }

        private async Task HandleChangedData(bool changed)
        {
            await OnDropDownValueChanged(IdSerie);
        }



        private async Task HandleModalNotiClose(ModalClosedEventArgs args)
        {
            PageLoadService.MostrarSpinnerReadLoad(Js);
            if (args.IsAccepted)
            {

                Dictionary<string, dynamic> headerId = new() { { "SubSerieId", recordToDelete.SubSeriesId } };

                var response = CallService.Put<int, int>("paramstrd/SubSeries/DeleteSubSerie", 0, headerId);
                await OnDropDownValueChanged(IdSerie);
                notificationModalSucces.UpdateModal(ModalNotificationsComponent.ModalType.Success, "Se ha borrado el registro existosamente", "Aceptar", true);

                 
                 


            }
            else
            {
                Console.WriteLine("Registro No eliminado");
            }
            PageLoadService.OcultarSpinnerReadLoad(Js);

        }

        private void HandlePaginationGrid(List<SubSeriesDtoResponse> newDataList)
        {
            subSeriesList = newDataList;
        }

        #endregion Methods
    }
}