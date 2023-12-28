using ControlDoc.Components.Components.DropDownListTelerik;
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
using Telerik.Blazor.Components;

namespace ControlDoc.FrondEnd.Areas.Administration
{
    public partial class AdministrativeUnit
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
        private ModalAdministrativeUnit modalAdministrativeUnit;

        private List<AdministrativeUnitDtoResponse> administrativeUnitList;

        private List<DocumentalVersionsDtoResponse> documentalVersionsList;

        private Meta meta;

        private Meta metaAdministrativeUnits;

        private AdministrativeUnitDtoResponse recordToDelete;

        private ModalNotificationsComponent notificationModal;

        private ModalNotificationsComponent notificationModalSucces;

        //capturar id de la version documental
        private DropDownListTelerik<DocumentalVersionsDtoResponse> docVersionDW { get; set; }
        
        private int IdDocumental { get; set; } = 0;
        private bool esHabilitado = true;
        private bool dataChargue = false;

        private Dictionary<string, dynamic> VersionDocumental { get; set; } = new Dictionary<string, dynamic>();

        private async Task OnDropDownValueChanged(int newValue)
        {
            PageLoadService.MostrarSpinnerReadLoad(Js);
            IdDocumental = newValue;

            VersionDocumental = new()
                {
                    {"documentalVersionId", IdDocumental }
                };

            esHabilitado = (IdDocumental <= 0) ? true : false;
            var response = await CallService.Get<List<AdministrativeUnitDtoResponse>>("paramstrd/AdministrativeUnit/ByFilter", VersionDocumental);
            administrativeUnitList = response.Data;
            metaAdministrativeUnits = response.Meta;
            dataChargue = true;
            PageLoadService.OcultarSpinnerReadLoad(Js);


        }

        protected override async Task OnInitializedAsync()
        {
            
            await GetDocumentalVersions();
            PageLoadService.OcultarSpinnerReadLoad(Js);
        }

        #endregion Initialization

        private async Task mostrarModal()
        {
            await modalAdministrativeUnit.PreparedModal();
            modalAdministrativeUnit.UpdateModalStatus(true);
        }

        private void HandleUserSelectedChanged(MyEventArgs<VUserDtoResponse> user)
        {
            
            pruebamodal2.UpdateModalStatus(user.ModalStatus);

            modalAdministrativeUnit.updateBossSelection(user.Data);

        }


        private async Task GetDocumentalVersions()
        {
            try
            {
                var response = await CallService.Get<List<DocumentalVersionsDtoResponse>>("paramstrd/DocumentalVersions/ByFilter");
                documentalVersionsList = response.Data;
                meta = response.Meta;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener las series: {ex.Message}");
            }
        }


        //invocar nueva modal

        private GenericSearchModal pruebamodal2;

        private void HandleStatusChanged(bool status)
        {
            // Recibe el valor de estado (true o false) aquí y haz lo que necesites con él
            pruebamodal2.UpdateModalStatus(status);
        }

        private async Task HandleChangedData(bool changed) 
        {

            await OnDropDownValueChanged(IdDocumental);


        }


        private void ShowModalEdit(AdministrativeUnitDtoResponse record)
        {

            modalAdministrativeUnit.UpdateModalStatus(true);
            modalAdministrativeUnit.UpdateSelectedRecord(record);
        }

        private async Task ShowModalDelete(AdministrativeUnitDtoResponse record)
        {
            recordToDelete = record;
            notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Delete, "Esta seguro de eliminar el mensaje", "Aceptar", true);
        }


        private async Task HandleModalNotiClose(ModalClosedEventArgs args)
        {
            if (args.IsAccepted)
            {
                PageLoadService.MostrarSpinnerReadLoad(Js);
                Dictionary<string, dynamic> headerId = new() { { "adminUnitId", recordToDelete.AdministrativeUnitId } };
                var response = await CallService.Put<int, int>("paramstrd/AdministrativeUnit/DeleteAdministrativeUnit", 0, headerId);

                notificationModalSucces.UpdateModal(ModalNotificationsComponent.ModalType.Success, "Se ha borrado el registro existosamente", "Aceptar", true);

                await OnDropDownValueChanged(IdDocumental);
                PageLoadService.OcultarSpinnerReadLoad(Js);




            }
            else
            {
                Console.WriteLine("Registro No eliminado");
            }

        }

        private void HandlePaginationGrid(List<AdministrativeUnitDtoResponse> newDataList)
        {
            administrativeUnitList = newDataList;
        }



    }
}