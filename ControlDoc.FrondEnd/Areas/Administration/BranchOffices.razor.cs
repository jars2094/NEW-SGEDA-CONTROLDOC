using ControlDoc.Components.Components.Grids;
using ControlDoc.Components.Components.Modals;
using ControlDoc.ComponentViews.ComponentViews.Administration;
using ControlDoc.FrondEnd.Shared.Layouts.MainLayout;
using ControlDoc.Models.Models.Administration.Response;
using ControlDoc.Models.Models.Generic;
using ControlDoc.Models.Models.Pagination;
using ControlDoc.Services.Interfaces;
using ControlDoc.Services.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ControlDoc.FrondEnd.Areas.Administration
{
    public partial class BranchOffices 
    {
        #region Variables
        #region Injects
        [Inject]
        private ICallService CallService { get; set; }
        #endregion
        #endregion

        #region Initialization

        // Inyección de dependencia del servicio IJSRuntime.
        [Inject]
        private IJSRuntime Js { get; set; }

        



        #region Variables

        private List<BranchOfficesDtoResponse> branchOfficesList;
        private Meta meta;

        
        
        private ModalBranchOffices modalbranchOffice;

        private BranchOfficesDtoResponse recordToDelete;

        private ModalNotificationsComponent notificationModal;
        private ModalNotificationsComponent notificationModalSucces;

        private bool IsRecordDeleted = false;

        #endregion

        protected override async Task OnInitializedAsync()
        {
            
            await GetBranchsOffices();

            PageLoadService.OcultarSpinnerReadLoad(Js);

        }

        #endregion

        #region General Methods
        // Método para mostrar el modal.
        private async Task mostrarModal()
        {
            modalbranchOffice.UpdateModalStatus(true);
        }
        

        

        

       
        #region GetBranchsOffices Method

        // Método para obtener la lista de sucursales.
        private async Task GetBranchsOffices()
        {
            try
            {
                // Realiza una llamada al servicio para obtener las sucursales.
                var response = await CallService.Get<List<BranchOfficesDtoResponse>>("params/BranchOffice/ByFilter");
                branchOfficesList = response.Data;
                meta = response.Meta;
            }
            catch (Exception ex)
            {
                // Manejo de errores en caso de fallo al obtener las sucursales.
                Console.WriteLine($"Error al obtener las sucursales: {ex.Message}");
            }
        }

        #endregion

        #endregion

        //nuevos metodos mostrar modal:

        private void ShowModalEdit(BranchOfficesDtoResponse record)
        {
            
            modalbranchOffice.UpdateModalStatus(true);
            modalbranchOffice.RecibirRegustro(record);
        }

        private void ShowModalDelete(BranchOfficesDtoResponse record)
        {
            recordToDelete = record;
            notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Delete, "Esta seguro de eliminar el mensaje", "Aceptar", true);

            //if (IsRecordDeleted)
            //{
            //    notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Success, "Regsitro Eliminado Correctamente", "Aceptar", true);
            //}
        }
        private async Task HandleModalNotiClose(ModalClosedEventArgs args)
        {
            if (args.IsAccepted)
            {
                Dictionary<string, dynamic> headerId = new() { { "branchOfficeDeleteId", recordToDelete.BranchOfficeId } };
                var response = await CallService.Put<int, int>("params/BranchOffice/DeleteBranchOffice", 0, headerId);
                notificationModalSucces.UpdateModal(ModalNotificationsComponent.ModalType.Success, "Registro Borrado Correctamente", "Aceptar", true);

                await GetBranchsOffices();
            }
            else
            {
                Console.WriteLine("Registro No eliminado");
            }


        }
        

       

        private async Task HandleRefreshGridDataAsync(bool refresh)
        {
            await GetBranchsOffices();
        }
        private void HandlePaginationGrid(List<BranchOfficesDtoResponse> newDataList)
        {
            branchOfficesList = newDataList;
        }


    }
}

