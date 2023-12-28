using ControlDoc.Components.Components.Modals;
using ControlDoc.ComponentViews.ComponentViews.Administration;
using ControlDoc.ComponentViews.ComponentViews.GenericViews;
using ControlDoc.Models.Models.Administration.Response;
using ControlDoc.Models.Models.Generic;
using ControlDoc.Models.Models.Pagination;
using ControlDoc.Services.Interfaces;
using ControlDoc.Services.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ControlDoc.FrondEnd.Areas.Administration
{
    public partial class ProductionOffices
    {
        #region Variables
        #region Injects
        [Inject]
        private ICallService CallService { get; set; }
        #endregion
        #endregion

        #region Attributes

        [Inject]
        private IJSRuntime Js { get; set; }

        private Meta meta { get; set; } = new() { pageSize = 10 };
        private int userIdToDelete { get; set; }
        private int idAdUnit { get; set; }
        private bool isEnable = true;
        private bool dataChargue = false;

        private ModalProductionOffices modalProductionOffice { get; set; } = new();

        private ModalNotificationsComponent modalNotification { get; set; } = new();
        private ModalNotificationsComponent notificationModalSucces { get; set; } = new();
        private GenericSearchModal genericSearchModal { get; set; } = new();

        private List<AdministrativeUnitDtoResponse> administrativeUnitsList { get; set; } = new();
        private List<ProductionOfficesDtoResponse> productionOfficesList { get; set; } = new();
        private Dictionary<string, dynamic> headers { get; set; } = new();

        #endregion Attributes

        #region Initialization

        protected override async Task OnInitializedAsync()
        {
            await FillAdministrativeUnitDdl();
            PageLoadService.OcultarSpinnerReadLoad(Js);
        }

        #endregion Initialization

        #region Event Handlers

        private void ShowModal()
        {
            PageLoadService.MostrarSpinnerReadLoad(Js);
            var auSelected = administrativeUnitsList.Find(x => x.AdministrativeUnitId == idAdUnit);
            modalProductionOffice.UpdateModalStatus(true);
            modalProductionOffice.UpdateAdministrativeUnitSelected(idAdUnit, auSelected?.Name);
            PageLoadService.OcultarSpinnerReadLoad(Js);
        }

        private async Task HandleStatusChanged(bool status)
        {
            PageLoadService.MostrarSpinnerReadLoad(Js);
            genericSearchModal.UpdateModalStatus(status);
            await SearchByAdministrativeUnit();
            PageLoadService.OcultarSpinnerReadLoad(Js);
        }

        private async Task HandleStatusChangedUpdated(bool status)
        {
            PageLoadService.MostrarSpinnerReadLoad(Js);
            modalProductionOffice.UpdateModalStatus(status);
            await SearchByAdministrativeUnit();
            PageLoadService.OcultarSpinnerReadLoad(Js);
        }

        private void HandleUserSelectedChanged(MyEventArgs<VUserDtoResponse> user)
        {
            PageLoadService.MostrarSpinnerReadLoad(Js);
            modalProductionOffice.updateBossSelection(user.Data);
            genericSearchModal.UpdateModalStatus(user.ModalStatus);
            PageLoadService.OcultarSpinnerReadLoad(Js);
        }

        private async Task OnDropDownValueChanged(int newValue)
        {
            PageLoadService.MostrarSpinnerReadLoad(Js);
            idAdUnit = newValue;
            isEnable = (idAdUnit <= 0);

            try
            {
                await SearchByAdministrativeUnit();
            }
            catch (Exception ex)
            {
                // Manejar excepciones, logearlas o tomar medidas apropiadas según tu aplicación
                Console.WriteLine($"Error en OnDropDownValueChanged: {ex.Message}");
            }
            PageLoadService.OcultarSpinnerReadLoad(Js);
        }

        private async Task SearchByAdministrativeUnit()
        {
            PageLoadService.MostrarSpinnerReadLoad(Js);
            headers = new()
                {
                    {"administrativeUnitId", idAdUnit }
                };

            var response = await CallService.Get<List<ProductionOfficesDtoResponse>>("paramstrd/ProductionOffice/ByFilter", headers) ?? new();

            // Verificar si response o response.Data son null antes de asignar

            if (response?.Data?.Count == 0 || response?.Data == null)
            {
                productionOfficesList = new();
                dataChargue = false;
                meta = new() { pageSize = 10 };
            }
            else
            {
                productionOfficesList = response?.Data ?? new();
                meta = response?.Meta;
                dataChargue = true;
            }
            PageLoadService.OcultarSpinnerReadLoad(Js);
        }

        #endregion Event Handlers

        #region Data Loading

        private async Task FillAdministrativeUnitDdl()
        {
            PageLoadService.MostrarSpinnerReadLoad(Js);
            var response = await CallService.Get<List<AdministrativeUnitDtoResponse>>($"paramstrd/AdministrativeUnit/ByAdministrativeUnits");

            administrativeUnitsList = response?.Data ?? new();
            PageLoadService.OcultarSpinnerReadLoad(Js);
        }

        #endregion Data Loading

        #region Modal Handling

        private async Task ShowModalEdit(ProductionOfficesDtoResponse value)
        {
            PageLoadService.MostrarSpinnerReadLoad(Js);
            modalProductionOffice.UpdateModalStatus(true);
            await modalProductionOffice.UpdateSelectedRecord(value);
            PageLoadService.OcultarSpinnerReadLoad(Js);
        }

        private void ShowModalDelete(ProductionOfficesDtoResponse value)
        {
            userIdToDelete = value.ProductionOfficeId;
            modalNotification.UpdateModal(ModalNotificationsComponent.ModalType.Delete, "Esta seguro de eliminar el mensaje", "Aceptar", true);
        }

        private async Task HandleModalNotiClose(ModalClosedEventArgs args)
        {
            if (args.IsAccepted)
            {
                await DeleteUser();
                await SearchByAdministrativeUnit();
            }
        }

        public async Task DeleteUser()
        {
            PageLoadService.MostrarSpinnerReadLoad(Js);
            Dictionary<string, dynamic> header = new()
            {
                { "ProOfficeId",userIdToDelete},
                {"updateUser", "Val" }
            };

            var response = await CallService.Put<int, int>("paramstrd/ProductionOffice/DeleteProductionOffice", 0, header);

            if (response.Data != null)
            {
                notificationModalSucces.UpdateModal(ModalNotificationsComponent.ModalType.Success, "Registro Borrado Correctamente", "Aceptar", true);
            }
            else
            {
                notificationModalSucces.UpdateModal(ModalNotificationsComponent.ModalType.Error, "Error al borrar el registro", "Aceptar", true);
            }
            PageLoadService.OcultarSpinnerReadLoad(Js);
        }

        private void HandlePaginationGrid(List<ProductionOfficesDtoResponse> newDataList)
        {
            PageLoadService.MostrarSpinnerReadLoad(Js);
            productionOfficesList = newDataList;
            PageLoadService.OcultarSpinnerReadLoad(Js);
        }

        #endregion Modal Handling
    }
}