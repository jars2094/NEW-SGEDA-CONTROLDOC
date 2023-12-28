using ControlDoc.Components.Components.Input;
using ControlDoc.Components.Components.Modals;
using ControlDoc.Models.Models.Administration.Request;
using ControlDoc.Models.Models.Administration.Response;
using ControlDoc.Models.Models.Generic;
using ControlDoc.Services.Interfaces;
using ControlDoc.Services.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telerik.Blazor.Components;
using Telerik.Blazor.Components.Common.DateInputs;
using Telerik.SvgIcons;
using static Telerik.Blazor.ThemeConstants;

namespace ControlDoc.ComponentViews.ComponentViews.Administration
{
    public partial class ModalProfileUsers : ComponentBase
    {

        #region Variables
        #region Injects
        [Inject] 
        private NavigationManager NavigationManager { get; set; }

        [Inject] 
        private ICallService CallService { get; set; }
        #endregion

        #region Parameters
        [Parameter] 
        public bool ModalStatus { get; set; } = false;

        [Parameter] 
        public string Id { get; set; }

        [Parameter] 
        public bool CrearEditar { get; set; }

        [Parameter]
        public EventCallback<bool> OnStatusChanged { get; set; }

        [Parameter] 
        public EventCallback<bool> OnChangeData { get; set; }
        #endregion

        private IJSRuntime Js { get; set; }

        //Request and Response
        private ProfileUsersDtoResponse _selectedRecord;
        private ProfileUsersDtoRequest profileDtoRequest = new ProfileUsersDtoRequest();
        private ProfileUsersDtoRequestUpdate profileUsersDtoRequestUpdate = new ProfileUsersDtoRequestUpdate();
        
        //Modal
        private ModalComponent? Modal { get; set; }
        
        //Inputs
        private InputModalComponent profileId { get; set; }
        private InputModalComponent profile1 { get; set; }
        private InputModalComponent profileCode { get; set; }
        private bool stateValue = true;
        private string description { get; set; }
        Decimal contadorcarac = 0;

        //Create or Edit
        private bool IsEditForm = false;
        private bool IsDisabledCode = false;
        private string IdProfile;

        #endregion Variables

        #region FormMethods

        private async Task HandleValidSubmit()
        {
            // Lógica de envío del formulario
            if (IsEditForm)
            {
                await Update();
                IsEditForm = false;
            }
            else
            {
                await Create();
            }
            await ResetFormAsync();
            StateHasChanged();
        }

        private async Task Create()
        {
            profileDtoRequest.Profile1 = profile1.InputValue;
            profileDtoRequest.ProfileCode = profileCode.InputValue;
            profileDtoRequest.Description = description ?? "";
            profileDtoRequest.ActiveState = stateValue;

            try
            {
                // TODO: Falta hacer el arreglo del post que dijo de Ivan
                var response = await CallService.Post<ProfileUsersDtoResponse, ProfileUsersDtoRequest>("permission/Profile/CreateProfile", profileDtoRequest);
                if (response.Succeeded)
                {
                    notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Success, "se creo el registro exitosamente", "aceptar", true);
                    await OnChangeData.InvokeAsync(true);
                }
                else
                {
                    notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Error, "No se creo el registro exitosamente", "aceptar", true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear un perfil {ex.Message}");
            }
        }

        private async Task Update()
        {
            profileUsersDtoRequestUpdate.Profile1 = profile1.InputValue;
            profileUsersDtoRequestUpdate.Description = description ?? "";
            profileUsersDtoRequestUpdate.ActiveState = stateValue;

            Dictionary<string, dynamic> headers = new(){{ "profileId", profileId.InputValue}};

            try
            {
                var response = await CallService.Put<ProfileUsersDtoResponse, ProfileUsersDtoRequestUpdate>("permission/Profile/UpdateProfile", profileUsersDtoRequestUpdate, headers);
                if (response.Succeeded)
                {

                    notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Success, "se actualizo exitosamente el registro", "Aceptar", true);
                    await OnChangeData.InvokeAsync(true);
                }
                else
                {
                    notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Error, "No se actualizo el registro, intente de nuevo", "Aceptar", true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar un perfil {ex.Message}");
            }
        }

        #endregion FormMethods

        #region Initialized

        protected override async Task OnInitializedAsync()
        {
            ModalStatus = false;
        }

        #endregion Initialized

        #region ModalMethods

        private ModalNotificationsComponent notificationModal;


        public async Task ResetFormAsync()
        {
            profileDtoRequest = new ProfileUsersDtoRequest();
            description = "";
            IdProfile = "";
            contadorcarac = 0;
            IsDisabledCode = false;
            StateHasChanged();
        }

        public void UpdateModalStatus(bool newValue)
        {
            ModalStatus = newValue;
            StateHasChanged();
        }

        private void HandleModalClosed(bool status)
        {
            ModalStatus = status;
            ResetFormAsync();
            StateHasChanged();
        }

        public void RecibirRegistro(ProfileUsersDtoResponse response)
        {
            _selectedRecord = response;

            IdProfile = _selectedRecord.ProfileId.ToString();

            profileDtoRequest.Profile1 = _selectedRecord.Profile1;
            profileDtoRequest.Description = _selectedRecord.Description;
            profileDtoRequest.ActiveState = _selectedRecord.ActiveState;
            profileDtoRequest.ProfileCode = _selectedRecord.ProfileCode;

            description = _selectedRecord.Description.ToString();
            stateValue =_selectedRecord.ActiveState;
            IsDisabledCode = true;
            IsEditForm = true;

        }
        
        #endregion ModalMethods

        #region NotificationModal

        private void HandleModalNotiClose(ModalClosedEventArgs args)
        {
            if (notificationModal.Type == ModalNotificationsComponent.ModalType.Success)
            {
                UpdateModalStatus(args.ModalStatus);
            }
        }

        #endregion NotificationModal

        #region ReloadPage
        private void RecargarPagina()
        {
            NavigationManager.NavigateTo(NavigationManager.Uri, true);
        }
        #endregion ReloadPage

        private void ContarCaracteres(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                contadorcarac = e.Value.ToString().Length;
            }
            else
            {
                contadorcarac = 0;
            }
        }
    }
}
