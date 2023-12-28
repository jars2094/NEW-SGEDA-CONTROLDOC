using ControlDoc.Components.Components.DropDown;
using ControlDoc.Components.Components.DropDownListTelerik;
using ControlDoc.Components.Components.Modals;
using ControlDoc.Models.Models.Administration.Request;
using ControlDoc.Models.Models.Administration.Response;
using ControlDoc.Models.Models.Authentication.Login.Request;
using ControlDoc.Models.Models.Generic;
using ControlDoc.Models.Models.Pagination;
using ControlDoc.Services.Interfaces;
using ControlDoc.Services.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace ControlDoc.ComponentViews.ComponentViews.Administration
{
    public partial class ModalPermisos : ComponentBase
    {
        #region Variables
        #region Injects
        [Inject] 
        private ICallService CallService { get; set; }
        #endregion

        #region Parametros
        [Parameter] public string IdModalIdentifier { get; set; }
        [Parameter] public bool ModalStatusPermisos { get; set; } = false;
        [Parameter] public int PerfilID { get; set; }
        
        [Parameter] public EventCallback<bool> OnStatusChanged { get; set; }

        [Parameter] public EventCallback<bool> OnChangeData { get; set; }
        #endregion

        #region Entorno
        private List<FuncionalityDtoResponse> FuncionalidadList;
        private List<PermissionDtoResponse> PermisosList;
        CreatePermissionDtoRequest CreatePermissionDtoRequest = new CreatePermissionDtoRequest();
        EditPermissionDtoRequest editPermissionDtoRequest = new EditPermissionDtoRequest();


        private bool habilitarFuncionalidad { get; set; } = true;
        private ModalComponent? Modal { get; set; }
        private PermissionDtoResponse _selectedRecord { get; set; }
        private Meta meta;
        //private DropDownListTelerik<FuncionalityDtoResponse> SelectDropdown;
        private TelerikDropDownList<FuncionalityDtoResponse,int> SelectDropdown;
        private ModalNotificationsComponent notificationModal;

        private string btnSaveOEdit { get; set; } = "Guardar";
        private string DefaultTextGrid { get; set; } = "Seleccione un Perfil...";
        //Campos de permission
        private int FunctionalityId { get; set; }
        private int PermissionId { get; set; }
        private bool SelectAccessF { get; set; }
        private bool SelectCreateF { get; set; }
        private bool SelectModifyF { get; set; }
        private bool SelectDeleteF { get; set; }
        private bool SelectConsultF { get; set; }
        private bool SelectPrintF { get; set; }
        private bool SelectActivoState { get; set; } 
        private bool dropdownList { get; set; }

        #endregion

        #endregion

        #region Metodos

        protected override async Task OnInitializedAsync()
        {
            ModalStatusPermisos = false;

        }

        #region Modal

        private async Task OpenNewModal()
        {
            await OnStatusChanged.InvokeAsync(true);
        }

        public void UpdateModalStatus(bool newValue)
        {
            DefaultTextGrid = "Seleccione una funcionalidad...";
            ModalStatusPermisos = newValue;

            StateHasChanged();
        }

        private void HandleModalClosed(bool status)
        {

            ModalStatusPermisos = status;
            StateHasChanged();
        }

        private void HandleModalNotiClose(ModalClosedEventArgs args)
        {
            if (notificationModal.Type == ModalNotificationsComponent.ModalType.Success)
            {
                UpdateModalStatus(args.ModalStatus);
            }
            if (notificationModal.Type == ModalNotificationsComponent.ModalType.Error)
            {
                UpdateModalStatus(args.ModalStatus);
            }

        }

        //Actualiza la modal con la informacion de la fila de la grilla
        public void UpdateSelectedRecord(PermissionDtoResponse? record)
        {
            _selectedRecord = record;
            Limpiar();
            PermissionId = _selectedRecord.permissionId;
            FunctionalityId = _selectedRecord.functionalityId;
            SelectAccessF = _selectedRecord.accessF;
            SelectCreateF = _selectedRecord.createF;
            SelectModifyF = _selectedRecord.modifyF;
            SelectConsultF = _selectedRecord.consultF;
            SelectDeleteF = _selectedRecord.deleteF;
            SelectPrintF = _selectedRecord.printF;
            SelectActivoState = _selectedRecord.activeState;
            habilitarFuncionalidad = false;
            btnSaveOEdit = "Editar";
            DefaultTextGrid = _selectedRecord.functionalityName;
            
            SelectDropdown.Refresh();
            //SelectDropdown.changeValue(FunctionalityId.ToString());

        }

        //Reinicia el formulario 
        public async Task resetForm()
        {
            CreatePermissionDtoRequest = new CreatePermissionDtoRequest();
            await OpenNewModal();

        }

        //Limpia las variables de la modal
        public void Limpiar()
        {
            SelectAccessF = false;
            SelectCreateF = false;
            SelectModifyF = false;
            SelectConsultF = false;
            SelectPrintF = false;
            SelectActivoState = false;
            StateHasChanged();
        }

       
        #endregion

        #region Get
        public async Task GetFuncionalidades()
        {
           

            try
            {
                if (PerfilID > 0)
                {
                    btnSaveOEdit = habilitarFuncionalidad ? "Guardar" : "Editar";
                    Dictionary<string, dynamic> PerProfileid = new()
                    {
                        {"PerProfileid", PerfilID}
                    };
                    var response = await CallService.Get<List<FuncionalityDtoResponse>>("permission/Functionality/ByFilterprofileId", PerProfileid);
                    FuncionalidadList = response.Data != null ? response.Data:new List<FuncionalityDtoResponse>();
                    
                    if( FuncionalidadList.Count > 0)
                    {
                        meta = response.Meta;
                        habilitarFuncionalidad = true;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener las funcionalidades: {ex.Message}");
            }
        }
        #endregion

        #region Post

        private async Task PostPermission()
        {
            CreatePermissionDtoRequest.profileId = PerfilID;
            CreatePermissionDtoRequest.functionalityId = FunctionalityId;
            CreatePermissionDtoRequest.userId = 0;
            CreatePermissionDtoRequest.accessF = SelectAccessF;
            CreatePermissionDtoRequest.accessF = SelectCreateF;
            CreatePermissionDtoRequest.modifyF = SelectModifyF;
            CreatePermissionDtoRequest.consultF = SelectConsultF;
            CreatePermissionDtoRequest.deleteF = SelectDeleteF;
            CreatePermissionDtoRequest.printF = SelectPrintF;

            var Response = await CallService.Post<CreatePermissionDtoResponse, CreatePermissionDtoRequest>("permission/Permission/CreatePermission", CreatePermissionDtoRequest);
            if (Response.Succeeded)
            {
                //await Modal!.MostrarNotificacion("", "Permiso Actualizado satisfactoriamente", ModalComponent.Icons.success, "Aceptar", "");
                resetForm();
                Limpiar();
                
                SelectDropdown.Refresh();
                notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Success, "Permiso creado satisfactoriamente", "aceptar", true);
                await OnChangeData.InvokeAsync(true);

            }
            else
            {
                notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Error, "No se pudo actualizar permiso", "aceptar", true);
                

            }
        }
        #endregion

        #region Put


        private async Task PutPermission()
        {
            editPermissionDtoRequest.accessF = SelectAccessF;
            editPermissionDtoRequest.createF = SelectCreateF;
            editPermissionDtoRequest.modifyF = SelectModifyF;
            editPermissionDtoRequest.consultF = SelectConsultF;
            editPermissionDtoRequest.deleteF = SelectDeleteF;
            editPermissionDtoRequest.printF = SelectPrintF;
            editPermissionDtoRequest.activeState = SelectActivoState;


            Dictionary<string, dynamic> permissionId = new()
           {
                        {"permissionId", PermissionId }
            };

            var Response = await CallService.Put<PermissionDtoResponse, EditPermissionDtoRequest>("permission/Permission/UpdatePermission", editPermissionDtoRequest, permissionId);
            if (Response.Succeeded)
            {
                //await Modal!.MostrarNotificacion("", "Permiso Actualizado satisfactoriamente", ModalComponent.Icons.success, "Aceptar", "");
                resetForm();
                Limpiar();
                notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Success, "Permiso actualizado satisfactoriamente", "aceptar", true);
                await OnChangeData.InvokeAsync(true);


            }
            else
            {
                notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Error, "No se pudo actualizar permiso", "aceptar", true);



            }
        }

        #endregion

        #region Save
        private async Task Guardar()
        {
            try
            {
                if (habilitarFuncionalidad == true)
                {
                    PostPermission();
                }
                else
                {
                    PutPermission();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar un permiso {ex.Message}");
            }
        }
        #endregion

        #endregion


    }
}


