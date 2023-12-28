using ControlDoc.Components.Components.Modals;
using ControlDoc.ComponentViews.ComponentViews.Administration;
using ControlDoc.Models.Models.Administration.Response;
using ControlDoc.Models.Models.Generic;
using ControlDoc.Models.Models.Pagination;
using ControlDoc.Services.Interfaces;
using ControlDoc.Services.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Telerik.Blazor.Components;

namespace ControlDoc.FrondEnd.Areas.Administration
{
    public partial class DocumentaryTypologiesBag
    {
        #region Variables
        #region Injects
        [Inject]
        private IJSRuntime Js { get; set; }

        [Inject]
        private ICallService CallService { get; set; }
        #endregion
        #endregion

        #region Initializations
        private List<DocumentaryTypologiesBagDtoResponse> documentaryTypologiesBagList;
        private Meta meta;

        private ModalDocumentaryTypologiesBag modalDocumentaryTypologiesBag;
        private ModalBondigTypologies modalDocumentaryTypologiesBondig;
        private ModalMetaDatosDTB modalMetaDatosDTB;


        private DocumentaryTypologiesBagDtoResponse recordToDelete;


        private ModalNotificationsComponent notificationModal;
        private ModalNotificationsComponent notificationModalSucces;

        #endregion Initialization

        protected override async Task OnInitializedAsync()
        {
            PageLoadService.MostrarSpinnerReadLoad(Js);
            await GetDocumentaryTypologiesBag();
            PageLoadService.OcultarSpinnerReadLoad(Js);
        }

        private async Task mostrarModal()
        {

            modalDocumentaryTypologiesBag.UpdateModalStatus(true);
        }


        private async Task HandleChangedData(bool changed)
        {

            await GetDocumentaryTypologiesBag();

        }

        private async Task GetDocumentaryTypologiesBag()
        {
            try
            {
                var response = await CallService.Get<List<DocumentaryTypologiesBagDtoResponse>>("documentarytypologies/DocumentaryTypologiesBag/ByFilter");
                documentaryTypologiesBagList = response.Data;
                meta = response.Meta;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener las bolsas de tipologias documentales: {ex.Message}");
            }
        }

         

        private async Task ShowModalDelete(DocumentaryTypologiesBagDtoResponse record)
        {
            recordToDelete = record;
            notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Delete, "Esta seguro de eliminar el mensaje", "Aceptar", true);
        }

        private void ShowModalEdit(DocumentaryTypologiesBagDtoResponse record)
        {
            modalDocumentaryTypologiesBag.UpdateModalStatus(true);
            modalDocumentaryTypologiesBag.UpdateSelectedRecord(record);
        }



        private void ShowModalBonding(DocumentaryTypologiesBagDtoResponse record) 
        {
            modalDocumentaryTypologiesBondig.UpdateRecord(record);
            modalDocumentaryTypologiesBondig.UpdateModalStatus(true);

        }

        private void ShowModalMetaDatos(DocumentaryTypologiesBagDtoResponse record)
        {
            modalMetaDatosDTB.UpdateModalStatus(true);
           

        }


        private async Task HandleModalNotiClose(ModalClosedEventArgs args)
        {
            if (args.IsAccepted)
            {

                Dictionary<string, dynamic> headerId = new() { { "documentaryTypologyBagId", recordToDelete.DocumentaryTypologyBagId } };
                var response = await CallService.Put<int, int>("documentarytypologies/DocumentaryTypologiesBag/DeleteDocumentaryTypologiesBag", 0, headerId);

                notificationModalSucces.UpdateModal(ModalNotificationsComponent.ModalType.Success, "Se ha borrado el registro existosamente", "Aceptar", true);

                await GetDocumentaryTypologiesBag();


            }
            else
            {
                Console.WriteLine("Registro No eliminado");
            }

        }

        private void HandlePaginationGrid(List<DocumentaryTypologiesBagDtoResponse> newDataList)
        {
            documentaryTypologiesBagList = newDataList;
        }

    }



}