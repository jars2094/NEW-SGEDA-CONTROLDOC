using ControlDoc.ComponentViews.ComponentViews.GenericViews;
using ControlDoc.Models.Models.Administration.Response;
using ControlDoc.Models.Models.Documents.Response;
using ControlDoc.Models.Models.Generic;
using ControlDoc.Models.Models.Pagination;
using ControlDoc.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDoc.ComponentViews.ComponentViews.Documents
{
    public partial class ModalSortDocuments
    {
        #region Variables
        #region Injects
        [Inject]
        private ICallService CallService { get; set; }
        #endregion
        #endregion

        [Parameter]
        public EventCallback<bool> OnStatusChangedTRD { get; set; }

        [Parameter]
        public EventCallback<bool> OnStatusChangedUser { get; set; }

        private bool modalStatus = false;
        private Meta meta;
        private List<SystemFieldsDtoResponse> systemFieldsCLList;
        private List<SystemFieldsDtoResponse> systemFieldsMRList;
        private List<VUserDtoResponse> vUserList;
        int systemParamCLId;
        int systemParamMRId;

        private GenericDocTypologySearchModal docTypologySearchModal;
        private GenericSearchModal genericSearchModal;
        private VDocumentaryTypologyResponse TRDSelected = new();

        protected override async Task OnInitializedAsync()
        {
            await GetClassCom();
            await GetReceivingM();
        }

        private async Task OpenNewModalTRD()
        {
            await OnStatusChangedTRD.InvokeAsync(true);
        }

        private async Task OpenNewModalUser()
        {
            await OnStatusChangedUser.InvokeAsync(true);
            StateHasChanged();
        }

        private async Task HandleModalClosed(bool status)
        {
            modalStatus = status;
            StateHasChanged();
        }

        public void UpdateModalStatus(bool newValue)
        {
            modalStatus = newValue;
            StateHasChanged();
        }

        public async Task GetClassCom()
        {
            try
            {
                Dictionary<string, dynamic> paramcode = new()
                {
                    {"ParamCode", "CL"}
                };

                var response = await CallService.Get<List<SystemFieldsDtoResponse>>("params/SystemFields/ByFilter", paramcode);
                systemFieldsCLList = response.Data ?? new();

                if (systemFieldsCLList.Count != 0)
                {
                    meta = response.Meta;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener Unidades Administrativas: {ex.Message}");
            }
        }

        public async Task GetReceivingM()
        {
            try
            {
                Dictionary<string, dynamic> paramcode = new()
                {
                    {"ParamCode", "MR"}
                };

                var response = await CallService.Get<List<SystemFieldsDtoResponse>>("params/SystemFields/ByFilter", paramcode);
                systemFieldsMRList = response.Data ?? new();

                if (systemFieldsMRList.Count != 0)
                {
                    meta = response.Meta;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener Unidades Administrativas: {ex.Message}");
            }
        }
    }
}
