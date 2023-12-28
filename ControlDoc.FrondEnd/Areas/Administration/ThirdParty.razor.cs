using ControlDoc.Components.Components.Grids;
using ControlDoc.Components.Components.Input;
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
using System.Net;
using Telerik.Blazor.Components;

namespace ControlDoc.FrondEnd.Areas.Administration
{
    public partial class ThirdParty
    {
        #region Variables
        #region Injects

        [Inject]
        private IJSRuntime Js { get; set; }

        [Inject] 
        private ICallService CallService { get; set; }

        #endregion Injects

        private bool CrearEditar { get; set; } = true;
        private bool modalstatus;

        private List<ThirdPartyDtoResponse> ThirdPartyList;
        private ThirdPartyDtoResponse thirdPartyPut;

        private ModalThirdParty modalThirdParty;
        private ModalAddress modalAddress;
        private ModalNotificationsComponent notificationModal;
        private ModalNotificationsComponent notificationModalSucces;

        private Meta meta;

        //inputs
        private string names = "";
        private string email = "";
        private string identification = "";

        //Tabs
        private int currentTab {get; set;}
        List<MyTabModel> tabs = new ();
        #endregion

        #region Initialized

        protected override async Task OnInitializedAsync()
        {
            PageLoadService.MostrarSpinnerReadLoad(Js);
            await GetThirdPartyAll();
            var pnResponse = await CallService.Get<List<ThirdPartyDtoResponse>>("administration/ThirdParty/ByFilter", new Dictionary<string, dynamic> { { "personType", "PN" } });
            tabs.Add(new MyTabModel { Title = "Persona Natural", FilteredData = FilterData("PN"), Meta = pnResponse.Meta });
            var pjResponse = await CallService.Get<List<ThirdPartyDtoResponse>>("administration/ThirdParty/ByFilter", new Dictionary<string, dynamic> { { "personType", "PJ" } });
            tabs.Add(new MyTabModel { Title = "Persona Jurídica", FilteredData = FilterData("PJ"), Meta = pjResponse.Meta });
            PageLoadService.OcultarSpinnerReadLoad(Js);
        }

        #endregion Initialized

        #region EventHandlers

        private async Task ShowModal()
        {
            modalThirdParty.selectPersonType(currentTab);
            modalThirdParty.UpdateModalStatus(true);
        }

        private void HandleStatusChanged(bool status)
        {
            // Recibe el valor de estado (true o false) aquí y haz lo que necesites con él
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

        void TabChangedHandler(int newIndex)
        {
            currentTab = newIndex;
            modalThirdParty.selectPersonType(currentTab);
            StateHasChanged();
        }

        private void ShowModalEdit(ThirdPartyDtoResponse record)
        {
            modalThirdParty.selectPersonType(currentTab);
            modalThirdParty.UpdateModalStatus(true);
            modalThirdParty.RecibirRegistro(record);
        }

        private async Task HandleRecordToDelete(ThirdPartyDtoResponse args)
        {
            thirdPartyPut = args;
            notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Delete, "Esta seguro de eliminar la persona", "Aceptar", true);
            
        }

        private async Task HandleModalNotiClose(ModalClosedEventArgs args)
        {
            if (args.IsAccepted)
            {

                Dictionary<string, dynamic> ThirdPartyId = new() { { "thirdPartyIdToDelete", thirdPartyPut.ThirdPartyId } };
                var response = await CallService.Put<int, int>("administration/ThirdParty/DeleteThirdParty", 0, ThirdPartyId);

                notificationModalSucces.UpdateModal(ModalNotificationsComponent.ModalType.Success, "Se ha eliminado el registro existosamente", "Aceptar", true);

                await HandleRefreshGridData(true);

            }
            else
            {
                Console.WriteLine("Registro No eliminado");
            }

        }

        private void HandleUserSelectedChanged(MyEventArgs<List<(string, AddressDtoRequest)>> address)
        {
            modalAddress.UpdateModalStatus(address.ModalStatus);
            modalThirdParty.updateAddressSelection(address.Data);

        }

        private async Task HandleRefreshGridData(bool refresh)
        {
            await GetThirdParty(currentTab == 0 ? "PN" : "PJ");

            // Llama al método FilterData después de la actualización de GetThirdParty
            tabs[currentTab].FilteredData = FilterData(currentTab == 0 ? "PN" : "PJ");

            var pnResponse = await CallService.Get<List<ThirdPartyDtoResponse>>("administration/ThirdParty/ByFilter", new Dictionary<string, dynamic> { { "personType", "PN" } });
            tabs[0].Meta = pnResponse.Meta;
            var pjResponse = await CallService.Get<List<ThirdPartyDtoResponse>>("administration/ThirdParty/ByFilter", new Dictionary<string, dynamic> { { "personType", "PJ" } });
            tabs[1].Meta = pjResponse.Meta;
        }

        #endregion EventHandlers

        #region ServiceMethods

        private async Task GetThirdParty(string personType)
        {
            try
            {
                Dictionary<string, dynamic> header = new() { { "personType", personType } };
                var response = await CallService.Get<List<ThirdPartyDtoResponse>>("administration/ThirdParty/ByFilter", header);
                ThirdPartyList = response.Data;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener personas naturales y juridicas: {ex.Message}");
            }
        }

        private async Task GetThirdPartyAll()
        {
            try
            {
                var response = await CallService.Get<List<ThirdPartyDtoResponse>>("administration/ThirdParty/ByFilter", null);
                ThirdPartyList = response.Data;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener personas naturales y juridicas: {ex.Message}");
            }
        }

        #endregion ServiceMethods

        #region Filters

        private async Task ApplyFiltersAsync()
        {
            
            if (string.IsNullOrEmpty(names) && string.IsNullOrEmpty(email) && string.IsNullOrEmpty(identification))
            {
                notificationModalSucces.UpdateModal(ModalNotificationsComponent.ModalType.Error, "Ingrese por lo menos un criterio de busqueda", "Aceptar", true);
                var response = await CallService.Get<List<ThirdPartyDtoResponse>>("administration/ThirdParty/ByFilter", null);
                ThirdPartyList = response.Data;
                meta = response.Meta;

                // Actualiza los datos filtrados en cada pestaña
                tabs[0].FilteredData = FilterData("PN");
                tabs[1].FilteredData = FilterData("PJ");
            }
            else
            {
                Dictionary<string, dynamic> headers = new() { { "names", names }, { "email", email }, { "identificationNumber", identification } };

                var response = await CallService.Get<List<ThirdPartyDtoResponse>>("administration/ThirdParty/ByFilter", headers);
                
                if (response.Data != null)
                {
                    ThirdPartyList = response.Data;
                    meta = response.Meta;
                    // Actualiza los datos filtrados en cada pestaña
                    tabs[0].FilteredData = FilterData("PN");
                    tabs[1].FilteredData = FilterData("PJ");
                }
                else
                {
                    names = "";
                    email = "";
                    identification = "";
                    notificationModalSucces.UpdateModal(ModalNotificationsComponent.ModalType.Error, "Ingrese por lo menos un criterio de busqueda", "Aceptar", true);
                    await HandleRefreshGridData(true);
                }
                
            }
            StateHasChanged();
        }

        public async Task ResetFiltersAsync()
        {
            names = "";
            email = "";
            identification = "";
            await GetThirdPartyAll();
            tabs[0].FilteredData = FilterData("PN");
            tabs[1].FilteredData = FilterData("PJ");

            StateHasChanged();
        }

        #endregion Filters

        #region Tab

        public class MyTabModel
        {
            public string Title { get; set; }
            public List<ThirdPartyDtoResponse> FilteredData { get; set; }
            public Meta Meta { get; set; }
        }

        // Función para filtrar los datos según el tipo de persona
        List<ThirdPartyDtoResponse> FilterData(string personType)
        {
            return ThirdPartyList
                .Where(item => item.PersonType == personType)
                .ToList();
        }

        private void HandlePaginationGridAsync(List<ThirdPartyDtoResponse> newDataList)
        {
            ThirdPartyList = newDataList;
            tabs[currentTab].FilteredData = FilterData(currentTab == 0 ? "PN" : "PJ");
        }

        #endregion Tab

        #region Methods Modal Notifications

        public void UpdateModalStatus(bool newValue)
        {
            modalstatus = newValue;
            StateHasChanged();
        }


        #endregion

    }
}
