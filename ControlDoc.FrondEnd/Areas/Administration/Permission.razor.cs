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
    public partial class Permission
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
        private List<PerfilesDtoResponse> PerfilesList;
        private List<PermissionDtoResponse> PermisosList = new List<PermissionDtoResponse>();
        [Parameter] public string IdModalIdentifier { get; set; }
        [Parameter] public EventCallback<bool> OnChangeData { get; set; }

        private int IdPerfil { get; set; }
        private PermissionDtoResponse recordToDelete { get; set; }

        private ModalNotificationsComponent notificationModal;
        private ModalNotificationsComponent notificationModalSucces;

        private bool Habilitar { get; set; } = true;
        private bool CrearEditar { get; set; } = true;
        private ModalPermisos ModalCrearOEditar;

        //private List<PermissionDtoResponse> FunctionList;

        private Meta meta;
        protected override async Task OnInitializedAsync()
        {
          
           
            await GetProfile();
            PageLoadService.OcultarSpinnerReadLoad(Js);
        }
        #endregion

        #region Metodos


        //private async Task MostrarModal()
        //{
        //    CrearEditar = true;
        //    ModalStatusPermisos = true;
        //    var PerfilId = IdPerfil;
        //    await ModalCrearOEditar.GetFuncionalidades(IdPerfil);
        //    ModalCrearOEditar.Limpiar();
        //    ShowModalsService.ShowModal(Js, ModalCrearOEditar.IdModalIdentifier);

        //}

        private async Task MostrarModal()
        {
            ModalCrearOEditar.PerfilID = IdPerfil;
            ModalCrearOEditar.Limpiar();
            await ModalCrearOEditar.GetFuncionalidades();
            ModalCrearOEditar.UpdateModalStatus(true);
        }
        //private void HandleRecordSelected(PermissionDtoResponse? selectedRecord)
        //{ if (selectedRecord == null) {
        //        CrearEditar = true;
        //        ModalStatusPermisos = true;
        //        var PerfilId = IdPerfil;
        //        ModalCrearOEditar.Limpiar();
        //        ShowModalsService.ShowModal(Js, ModalCrearOEditar.IdModalIdentifier);
        //    } else { 
        //    CrearEditar=false;
        //    ModalCrearOEditar.UpdateSelectedRecord(selectedRecord);
        //    }
        //} 

        private async Task ShowModalEdit(PermissionDtoResponse args)
        {
            CrearEditar = false;
            ModalCrearOEditar.UpdateModalStatus(true);
            ModalCrearOEditar.UpdateSelectedRecord(args);

        }


        //private async Task HandleRecordToDelete(PermissionDtoResponse record)
        //{

        //    var recordToDelete = record;
        //    var nameFuncionality = record.functionalityName;
        //    notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Delete, "¿Estas seguro de borrar la funcionalidad {nameFuncionality}?", "Aceptar", true);
        //    Dictionary<string, dynamic> headerId = new() 
        //    { { "PermissionsId", recordToDelete.permissionId } };

        //    var response = CallService.Put<int, int>("permission/Permission/DeletePermission", 0, headerId);

        //    if (response.Id != null) { notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Success, "Se ha eliminado correctamente el registro.", "Aceptar", true); }

        //    await GetPermisos();
        //}
        private void ShowModalDelete(PermissionDtoResponse record)
        {
            recordToDelete = record;
            notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Delete, "Esta seguro de eliminar el Permiso", "Aceptar", true);

           
        }



        private async Task HandleModalNotiClose(ModalClosedEventArgs args)
        {
            if (args.IsAccepted)
            {
                if (recordToDelete != null)
                {
                    Dictionary<string, dynamic> headerId = new()
                { { "PermissionsId", recordToDelete.permissionId } };
                    var response = CallService.Put<int, int>("permission/Permission/DeletePermission", 0, headerId);
                    notificationModalSucces.UpdateModal(ModalNotificationsComponent.ModalType.Success, "Permiso Borrado Correctamente", "Aceptar", true);
                }
                else { notificationModalSucces.UpdateModal(ModalNotificationsComponent.ModalType.Error, "Error al eliminar permiso", "Aceptar", true); }
                HandleRefreshGridDataAsync(true);

            }
            else
            {
                Console.WriteLine("Registro No eliminado");
            }


        }

        private async Task GetProfile()
        {
            try
            {
                var response = await CallService.Get<List<PerfilesDtoResponse>>("permission/Profile/ByFilter");
                PerfilesList = response.Data;
                meta = response.Meta;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener las Perfiles: {ex.Message}");
            }
        }
        //Endpoint para mostrar en la grilla
        public async Task GetPermisos()
        {
            
            try
            {
                if (IdPerfil > 0)
                {
                    Habilitar = false;
                    Dictionary<string, dynamic> PerProfileid = new()
                    {
                        {"PerProfileid", IdPerfil}
                    };
                    var response = await CallService.Get<List<PermissionDtoResponse>>($"permission/Permission/ByFilterProfileId", PerProfileid);
                    PermisosList = response.Data != null ? response.Data: new List<PermissionDtoResponse>();
                    if (PermisosList.Count > 0)
                    {
                        meta = response.Meta;

                    }

                }
                else
                {
                    Habilitar = true;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los permisos: {ex.Message}");
            }


        }




        #endregion

        public async Task HandleRefreshGridDataAsync(bool refresh)
        {
            await GetPermisos();
        }
    }
}
