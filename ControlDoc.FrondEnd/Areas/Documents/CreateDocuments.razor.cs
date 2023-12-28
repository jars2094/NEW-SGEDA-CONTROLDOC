using ControlDoc.ComponentViews.ComponentViews.Administration;
using ControlDoc.ComponentViews.ComponentViews.Documents;
using ControlDoc.ComponentViews.ComponentViews.GenericViews;
using ControlDoc.Models.Models.Administration.Response;
using ControlDoc.Models.Models.Documents.Response;
using ControlDoc.Models.Models.Generic;
using ControlDoc.Models.Models.Pagination;
using ControlDoc.Services.Interfaces;
using ControlDoc.Services.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ControlDoc.FrondEnd.Areas.Documents
{
    public partial class CreateDocuments
    {
        #region Variables
        #region Injects
        [Inject]
        private IJSRuntime Js { get; set; }

        [Inject]
        private ICallService CallService { get; set; }
        #endregion
        #endregion

        private string panel_1 = "visible";
        private string panel_2 = "visible";
        private string panel_3 = "hidden";
        private string panel_4 = "hidden";

        private Meta meta;//esto no va
        private List<SubSeriesDtoResponse> subSeriesList;//esto tampoco

        private ModalMasterFormatList masterFormatList;
        private ModalSortDocuments sortDocuments;
        private GenericDocTypologySearchModal genericDocTypologySearchModal;
        private GenericSearchModal genericSearchModal=new();

        protected override async Task OnInitializedAsync()
        {
            await GetSubSeries();
            PageLoadService.OcultarSpinnerReadLoad(Js);
        }

        //el siguiente metodo debe removerse, solo se esta utilizando para ocultar el spiner al cargar la vista
        public async Task GetSubSeries()
        {
            try
            {
                var response = await CallService.Get<List<SubSeriesDtoResponse>>("paramstrd/SubSeries/BySubSeries");
                subSeriesList = response.Data?? new();

                if (subSeriesList.Count != 0)
                {
                    meta = response.Meta;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener Officinas Productoras: {ex.Message}");
            }
        }

        private async Task showModalMasterFormat()
        {
            masterFormatList.UpdateModalStatus(true);
        }

        private async Task showModalSortDocs()
        {
            sortDocuments.UpdateModalStatus(true);
        }

        private void HandleStatusChangedTRD(bool status)
        {
            genericDocTypologySearchModal.UpdateModalStatus(status);
        }

        private void HandleStatusChangedUser(bool status)
        {
            genericSearchModal.UpdateModalStatus(status);
        }

    }
}
