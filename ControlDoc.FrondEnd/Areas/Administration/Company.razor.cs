using ControlDoc.Components.Components.Modals;
using ControlDoc.ComponentViews.ComponentViews.Administration;
using ControlDoc.Models.Models.Administration.Request;
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
    public partial class Company
    {
        #region Variables
        #region Inject
        [Inject] private IJSRuntime Js { get; set; }

        [Inject] private ICallService CallService { get; set; }
        #endregion

        private bool modalStatus = false;
        #endregion

        #region Objects
        private ModalCompanies modalCompanies;
        private ModalAddress modalAddress;

        private AddressDtoRequest Addressrequest;
        private ModalNotificationsComponent notificationModal;
        private CompaniesDtoResponse CompanyPut;
        private Meta meta;

        #endregion

        #region List
        private List<CompaniesDtoResponse> CompaniesList;
        #endregion

        #region Initialization

        protected override async Task OnInitializedAsync()
        {
            PageLoadService.MostrarSpinnerReadLoad(Js);
            // Ejecuta la primera petición inmediatamente y luego cada minuto
            await GetCompanies();
            PageLoadService.OcultarSpinnerReadLoad(Js);
        }

        #endregion Initialization

        #region ModalAddress
        // Método para mostrar el modal.
        private async Task mostrarModal()
        {
            modalCompanies.UpdateModalStatus(true);
        }

        private void HandleStatusChanged(bool status)
        {

            modalAddress.UpdateModalStatus(status);
        }

        private void HandleId(int id)
        {
            modalAddress.UpdateModalIdAsync(id);
        }

        private void HandleForm()
        {
            modalAddress.ResetForm();
        }

        private void HandleUserSelectedChanged(MyEventArgs<List<(string, AddressDtoRequest)>> address)
        {
            //modalAddress.resetForm();
            modalAddress.UpdateModalStatus(address.ModalStatus);
            modalCompanies.updateAddressSelection(address.Data);

        }


        #endregion

        #region General Methods GRID


        // Método para manejar la selección de registro.

        #region UpdateGrid
        private void HandleRecordSelected(MyEventArgs<CompaniesDtoResponse> args)
        {
            modalCompanies.UpdateModalStatus(args.ModalStatus);
            //modalCompanies.UpdateSelectedRecord(args.Data);
        }

        #endregion

        #region DeleteGrid
        private async Task HandleModalNotiClose(ModalClosedEventArgs args)
        {

            Dictionary<string, dynamic> CompanyId = new() { { "id", CompanyPut.CompanyId } };


            var response = CallService.Put<int, int>("companies/Company/DeleteCompany", 0, CompanyId);

            notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Success, "Se ha eliminado correctamente el registro.", "Aceptar", true);
            await HandleRefreshGridData(true);

        }
        #endregion

 

        #region RefreshGrid

        private async Task HandleRefreshGridData(bool refresh)
        {
            await GetCompanies();
        }

        #endregion


        #endregion General Methods

        #region ModalCompanies
        private void ShowModalEdit(CompaniesDtoResponse record)
        {

            modalCompanies.UpdateModalStatus(true);
            modalCompanies.RecibirRegistro(record);
        }

        private async Task ShowModalDelete(CompaniesDtoResponse record)
        {
            CompanyPut = record;
            notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Delete, "¿Estas seguro de borrar el registro?", "aceptar", true);


        }
        #endregion

        #region GetCompany Method

        // Método para obtener la lista de co´pañias.
        private async Task GetCompanies()
        {
            try
            {
                var response = await CallService.Get<List<CompaniesDtoResponse>>("companies/Company/ByFilter", null);
                CompaniesList = response.Data;
                meta = response.Meta;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener las compañias: {ex.Message}");
            }
        }

        #endregion GetCompany Method

        #region Methods Modal Notifications
        public void UpdateModalStatus(bool newValue)
        {
            modalStatus = newValue;
            StateHasChanged();
        }




        #endregion

        #region PaginationGrid
        private void HandlePaginationGrid(List<CompaniesDtoResponse> newDataList)
        {
            CompaniesList = newDataList;
        }
        #endregion
    }

}