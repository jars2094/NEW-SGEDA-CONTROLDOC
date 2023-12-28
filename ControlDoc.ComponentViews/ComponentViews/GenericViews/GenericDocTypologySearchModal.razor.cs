using ControlDoc.Components.Components.Input;
using ControlDoc.Components.Components.Modals;
using ControlDoc.Models.Models.Administration.Response;
using ControlDoc.Models.Models.Documents.Response;
using ControlDoc.Models.Models.Generic;
using ControlDoc.Models.Models.Pagination;
using ControlDoc.Services.Interfaces;
using ControlDoc.Services.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ControlDoc.ComponentViews.ComponentViews.GenericViews
{
    public partial class GenericDocTypologySearchModal
    {
        #region Variables
        #region Injects
        [Inject]
        private ICallService CallService { get; set; }
        #endregion
        #endregion

        #region Parameter

        [Inject]
        private IJSRuntime Js { get; set; }

        [Parameter] public EventCallback<MyEventArgs<VDocumentaryTypologyResponse>> OnStatusChanged { get; set; }
        [Parameter] public string title { get; set; }

        #endregion Parameter

        #region Attributes

        private bool modalStatus = false;
        private List<VDocumentaryTypologyResponse> docTypologyList;
        private List<DocumentaryTypologyDtoResponse> docTypologiesList;
        private List<DocumentalVersionsDtoResponse> docVersionList;
        private List<AdministrativeUnitDtoResponse> adminUnitList;
        private List<ProductionOfficesDtoResponse> proOfficesList;
        private List<SeriesDtoResponse> seriesList;
        private List<SubSeriesDtoResponse> subSeriesList;
        private ModalNotificationsComponent notificationModal;
        private Meta meta;

        private bool isEnableAdminUnit = false;
        private bool isEnableProOffice = false;
        private bool isEnableSerie = false;
        private bool isEnableSubSerie = false;
        private bool isEnableDocTypology = false;
        private bool isVisibleTypologyNameInput = false;
        private bool isEnableButton = true;

        public InputModalComponent typologyNameInput { get; set; }
        public VDocumentaryTypologyResponse vDocumentary { get; set; }

        private int idDocVersion { get; set; }
        private int idAdminUnit { get; set; }
        private int idProOffice { get; set; }
        private int idSerie { get; set; }
        private int idSubSerie { get; set; }
        private int idDocTypologies { get; set; }

        #endregion Attributes

        #region OnInitialized

        protected override async Task OnInitializedAsync()
        {
            await GetDocumentalVersions();
        }

        #endregion OnInitialized

        #region Methods

        public async Task resetModal()
        {
            docTypologyList = new();
            docTypologiesList = new();
            docVersionList = new();
            adminUnitList = new();
            proOfficesList = new();
            seriesList = new();
            subSeriesList = new();

            isEnableAdminUnit = false;
            isEnableProOffice = false;
            isEnableSerie = false;
            isEnableSubSerie = false;
            isEnableDocTypology = false;
            isVisibleTypologyNameInput = false;
            isEnableButton = true;

            idDocVersion = 0;
            idAdminUnit = 0;
            idProOffice = 0;
            idSerie = 0;
            idSubSerie = 0;
            idDocTypologies = 0;

            typologyNameInput.InputValue = "";

            await GetDocumentalVersions();
        }

        public void UpdateModalStatus(bool newValue)
        {
            modalStatus = newValue;
            StateHasChanged();
        }

        private async Task HandleModalClosed(bool status)
        {
            modalStatus = status;
            await resetModal();
        }

        public async Task GetDocumentalVersions()
        {
            try
            {
                var response = await CallService.Get<List<DocumentalVersionsDtoResponse>>("paramstrd/DocumentalVersions/ByDocumentalVersions");
                docVersionList = response.Data;

                if (docVersionList.Count != 0)
                {
                    meta = response.Meta;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener Unidades Administrativas: {ex.Message}");
            }
        }

        public async Task GetAdministrativeUnits(int id)
        {
            try
            {
                idDocVersion = id;
                adminUnitList = new();
                idAdminUnit = 0;

                Dictionary<string, dynamic> administrativeUnit = new()
                {
                    {"documentalVersionsId", idDocVersion }
                };

                await GetDocTypologies();
                var response = await CallService.Get<List<AdministrativeUnitDtoResponse>>("paramstrd/AdministrativeUnit/ByAdministrativeUnits", administrativeUnit);
                adminUnitList = response.Data;

                if (adminUnitList.Count != 0)
                {
                    EnableField(true, false, false, false, false, false, true);
                    meta = response.Meta;
                }
                else
                {
                    EnableField(false, false, false, false, false, false, true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener Unidades Administrativas: {ex.Message}");
            }
        }

        public async Task GetProducOffice(int id)
        {
            try
            {
                idAdminUnit = id;
                proOfficesList = new();
                idProOffice = 0;

                Dictionary<string, dynamic> productionOffices = new()
                {
                    {"AdministrativeUnitId", idAdminUnit }
                };

                await GetDocTypologies();
                var response = await CallService.Get<List<ProductionOfficesDtoResponse>>("paramstrd/ProductionOffice/ByProductionOffices", productionOffices);
                proOfficesList = response.Data;

                if (proOfficesList.Count != 0)
                {
                    EnableField(true, true, false, false, false, false, true);
                    meta = response.Meta;
                }
                else
                {
                    Console.Write(response.Message);
                    EnableField(true, false, false, false, false, false, true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener Officinas Productoras: {ex.Message}");
            }
        }

        public async Task GetSeries(int id)
        {
            try
            {
                idProOffice = id;
                seriesList = new();
                idSerie = 0;

                Dictionary<string, dynamic> series = new()
                {
                    {"productionOfficeId", idProOffice }
                };

                await GetDocTypologies();
                var response = await CallService.Get<List<SeriesDtoResponse>>("paramstrd/Series/BySeries", series);
                seriesList = response.Data;

                if (seriesList.Count != 0)
                {
                    EnableField(true, true, true, false, false, true, false);
                    meta = response.Meta;
                }
                else
                {
                    EnableField(true, true, false, false, false, false, false);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener Officinas Productoras: {ex.Message}");
            }
        }

        public async Task GetSubSeries(int id)
        {
            try
            {
                idSerie = id;
                subSeriesList = new();
                idSubSerie = 0;

                Dictionary<string, dynamic> subSeries = new()
                {
                    {"seriesId", idSerie }
                };

                await GetDocTypologies();
                var response = await CallService.Get<List<SubSeriesDtoResponse>>("paramstrd/SubSeries/BySubSeries", subSeries);
                subSeriesList = response.Data;

                if (subSeriesList.Count != 0)
                {
                    EnableField(true, true, true, true, false, true, false);
                    meta = response.Meta;
                }
                else
                {
                    EnableField(true, true, true, false, false, true, false);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener Officinas Productoras: {ex.Message}");
            }
        }

        public async Task GetDocTypologiesBySubSerieId(int id)
        {
            try
            {
                idSubSerie = id;
                docTypologiesList = new();
                idDocTypologies = 0;

                Dictionary<string, dynamic> documentTypologies = new()
                {
                    {"documentalVersionId", idDocVersion},
                    {"administrativeUnitId", idAdminUnit },
                    {"productionOfficeId", idProOffice},
                    {"seriesId", idSerie},
                    {"subSeriesId", idSubSerie }
                };

                var response = await CallService.Get<List<DocumentaryTypologyDtoResponse>>("generalviews/VDocumentaryTypology/ByFilter", documentTypologies);
                docTypologiesList = response.Data;

                if (docTypologiesList?.Count != 0)
                {
                    EnableField(true, true, true, true, true, true, false);
                    meta = response.Meta;
                }
                else
                {
                    EnableField(true, true, true, true, false, true, false);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener Officinas Productoras: {ex.Message}");
            }
        }

        public async Task GetDocTypologies()
        {
            try
            {
                Dictionary<string, dynamic> documentTypologies = new()
                {
                    {"documentalVersionId", idDocVersion},
                    {"administrativeUnitId", idAdminUnit },
                    {"productionOfficeId", idProOffice},
                    {"seriesId", idSerie},
                };

                var response = await CallService.Get<List<DocumentaryTypologyDtoResponse>>("generalviews/VDocumentaryTypology/ByFilter", documentTypologies);
                docTypologiesList = response.Data;

                if (docTypologiesList.Count != 0)
                {
                    meta = response.Meta;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener Officinas Productoras: {ex.Message}");
            }
        }

        public async Task OnClickButton()
        {
            try
            {
                PageLoadService.MostrarSpinnerReadLoad(Js);

                Dictionary<string, dynamic> dataGrid = new()
                  {
                    {"documentalVersionId", idDocVersion},
                    {"administrativeUnitId", idAdminUnit },
                    {"productionOfficeId", idProOffice},
                    {"seriesId", idSerie},
                    {"subSeriesId", idSubSerie },
                    {"documentaryTypologyId", idDocTypologies},
                    {"typologyName", typologyNameInput.InputValue??""}
                  };

                var response = await CallService.Get<List<VDocumentaryTypologyResponse>>("generalviews/VDocumentaryTypology/ByFilter", dataGrid);
                docTypologyList = response.Data;

                if (docTypologyList.Count != 0)
                {
                    meta = response.Meta;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener Officinas Productoras: {ex.Message}");
            }
            PageLoadService.OcultarSpinnerReadLoad(Js);
        }

        private void SelectDocTypology(VDocumentaryTypologyResponse docTypology)
        {
            vDocumentary = docTypology;
            var gestor = vDocumentary.LmfullName;
            var tipologia = vDocumentary.TypologyName;

            if (vDocumentary != null)
            {
                notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Delete, "Gestor Lider: " + gestor + "\n Tipologia Documental: " + tipologia, "Aceptar", true);
            }
        }

        private async Task HandleModalNotiClose(ModalClosedEventArgs args)
        {
            if (args.IsAccepted)
            {
                Dictionary<string, dynamic> data = new()
                  {
                    {"documentaryTypologyId", vDocumentary.DocumentaryTypologyId}
                  };

                var response = await CallService.Get<List<VBehaviorTypologyResponse>>("generalviews/VBehaviorTypology/ByFilter", data);
                vDocumentary.VBehaviors = response.Data ?? new();
                var eventArgs = new MyEventArgs<VDocumentaryTypologyResponse>
                {
                    Data = vDocumentary,
                    ModalStatus = false
                };
                await OnStatusChanged.InvokeAsync(eventArgs);
            }
            else
            {
                docTypologyList.Where(x => x.DocumentaryTypologyId.Equals(vDocumentary.DocumentaryTypologyId)).FirstOrDefault().Selected = false;
            }
        }

        #endregion Methods

        #region Validations

        public void EnableField(bool a, bool b, bool c, bool d, bool e, bool f, bool g)
        {
            isEnableAdminUnit = a;
            isEnableProOffice = b;
            isEnableSerie = c;
            isEnableSubSerie = d;
            isEnableDocTypology = e;
            isVisibleTypologyNameInput = f;
            isEnableButton = g;

            idAdminUnit = a ? idAdminUnit : 0;
            idProOffice = b ? idProOffice : 0;
            idSerie = c ? idSerie : 0;
            idSubSerie = d ? idSubSerie : 0;
            idDocTypologies = e ? idDocTypologies : 0;
        }

        #endregion Validations
    }
}