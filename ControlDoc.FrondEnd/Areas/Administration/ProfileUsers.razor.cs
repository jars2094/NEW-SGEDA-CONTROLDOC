using ControlDoc.Components.Components.Grids;
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

namespace ControlDoc.FrondEnd.Areas.Administration
{
    public partial class ProfileUsers
    {

        #region Variables
        #region Injects

        [Inject]
        private IJSRuntime Js { get; set; }

        [Inject] private ICallService CallService { get; set; }

        #endregion Injects

        private bool CrearEditar { get; set; } = true;
        private bool modalstatus;
        private List<ProfileUsersDtoResponse> ProfileUsersList;
        private ProfileUsersDtoResponse profilePut;
        private Meta meta;
        private ModalProfileUsers modalProfileUsers;
        private ModalNotificationsComponent notificationModal;
        private ModalNotificationsComponent notificationModalSucces;


        #endregion Variables

        #region Initialized

        protected override async Task OnInitializedAsync()
        {
            PageLoadService.MostrarSpinnerReadLoad(Js);
            await GetProfiles();
            PageLoadService.OcultarSpinnerReadLoad(Js);
        }

        #endregion Initialized

        #region EventHandlers

        private async Task ShowModal()
        {
            modalProfileUsers.UpdateModalStatus(true);
        }

        private void HandleStatusChanged(bool status)
        {
            modalProfileUsers.UpdateModalStatus(status);
        }


        

        private void ShowModalEdit(ProfileUsersDtoResponse record)
        {
            modalProfileUsers.UpdateModalStatus(true);
            modalProfileUsers.RecibirRegistro(record);
        }

        #endregion EventHandlers

        #region ServiceMethods

        private async Task GetProfiles()
        {
            try
            {
                var response = await CallService.Get<List<ProfileUsersDtoResponse>>("permission/Profile/ByFilter", null);
                ProfileUsersList = response.Data;
                meta = response.Meta;
                //StateHasChanged();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los perfiles de usuario: {ex.Message}");
            }
        }

        private void HandlePaginationGrid(List<ProfileUsersDtoResponse> newDataList)
        {
            ProfileUsersList = newDataList;
        }

        #endregion ServiceMethods

        #region Methods Modal Notifications
        public void UpdateModalStatus(bool newValue)
        {
            modalstatus = newValue;
            StateHasChanged();
        }

        private async Task HandleRecordToDelete(ProfileUsersDtoResponse args)
        {
            profilePut = args;
            notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Delete, "Esta seguro de eliminar el perfil", "Aceptar", true);
        }

        private async Task HandleModalNotiCloseAsync(ModalClosedEventArgs args)
        {
            if (args.IsAccepted)
            {

                Dictionary<string, dynamic> ProfileId = new() { { "profilesId", profilePut.ProfileId } };
                var response = await CallService.Put<int, int>("permission/Profile/DeleteProfile", 0, ProfileId);
                notificationModalSucces.UpdateModal(ModalNotificationsComponent.ModalType.Success, "Se ha eliminado el registro existosamente", "Aceptar", true);
                await HandleRefreshGridData(true);
            }
            else
            {
                Console.WriteLine("Registro No eliminado");
            }


        }

        private async Task HandleRefreshGridData(bool refresh)
        {
            await GetProfiles();
        }
        #endregion
    }
}
