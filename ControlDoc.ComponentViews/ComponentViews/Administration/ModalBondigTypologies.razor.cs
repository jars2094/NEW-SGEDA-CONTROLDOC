using ControlDoc.Components.Components.Modals;
using ControlDoc.Models.Models.Administration.Request;
using ControlDoc.Models.Models.Administration.Response;
using ControlDoc.Models.Models.Generic;
using ControlDoc.Models.Models.Pagination;
using ControlDoc.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace ControlDoc.ComponentViews.ComponentViews.Administration
{
    public partial class ModalBondigTypologies
    {
        #region Variables
        #region Injects        
        [Inject]
        private ICallService CallService { get; set; }
        #endregion
        #endregion

        private bool modalStatus = false;
        private string Text = "Ingrese el codigo";

        private DocumentaryTypologiesDtoRequest documentaryTypologies = new();

        private ModalNotificationsComponent notificationModal;
        private List<DocumentalVersionsDtoResponse> docVersionList;
        private List<AdministrativeUnitDtoResponse> adminUnitList;
        private List<ProductionOfficesDtoResponse> proOfficesList;
        private List<SeriesDtoResponse> seriesList;
        private List<SubSeriesDtoResponse> subSeriesList;
        private Meta meta;

        private bool isEnableAdminUnit = false;
        private bool isEnableProOffice = false;
        private bool isEnableSerie = false;
        private bool isEnableSubSerie = false;
        private bool isEnableButton = true;

        private int idDocumentaryTypologiesBag { get; set; }
        private int idDocVersion { get; set; }
        private int idAdminUnit { get; set; }
        private int idProOffice { get; set; }
        private int idSerie { get; set; }
        private int idSubSerie { get; set; }

        #region OnInitialized

        protected override async Task OnInitializedAsync()
        {
            await GetDocumentalVersions();
        }

        #endregion OnInitialized

        // Método para manejar el envío válido del formulario.
        private async Task HandleValidSubmit()
        {
            documentaryTypologies.DocumentaryTypologyBagId = idDocumentaryTypologiesBag;
            documentaryTypologies.SeriesId = idSerie;
            documentaryTypologies.SubSeriesId = idSubSerie;

            var response = await CallService.Post<DocumentaryTypologiesDtoResponse, DocumentaryTypologiesDtoRequest>("documentarytypologies/DocumentaryTypologies/CreateDocumentaryTypologies", documentaryTypologies);

            if (response.Succeeded)
            {
                notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Success, response.Message, "Aceptar", true);
            }
            else 
            {
                notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Error, "Vinculación Erronea", "Aceptar", true);
            }


            StateHasChanged();
        }

        private void HandleModalNotiClose(ModalClosedEventArgs args)
        {
            if (notificationModal.Type == ModalNotificationsComponent.ModalType.Success)
            {
                UpdateModalStatus(args.ModalStatus);
            }
        }

        public void UpdateRecord(DocumentaryTypologiesBagDtoResponse record)
        {
            idDocumentaryTypologiesBag = record.DocumentaryTypologyBagId;
            Text = record.DocumentaryTypologyBagId.ToString();
            StateHasChanged();
        }

        public void UpdateModalStatus(bool newValue)
        {
            modalStatus = newValue;
            StateHasChanged();
        }

        private void HandleModalClosed(bool status)
        {
            modalStatus = status;
        }

        #region TRDSearch

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
                Console.WriteLine($"Error al obtener las versiones documentales: {ex.Message}");
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

                var response = await CallService.Get<List<AdministrativeUnitDtoResponse>>("paramstrd/AdministrativeUnit/ByAdministrativeUnits", administrativeUnit);
                adminUnitList = response.Data;

                if (adminUnitList.Count != 0)
                {
                    EnableField(true, false, false, false, true);
                    meta = response.Meta;
                }
                else
                {
                    Console.Write(response.Message);
                    EnableField(false, false, false, false, true);
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

                var response = await CallService.Get<List<ProductionOfficesDtoResponse>>("paramstrd/ProductionOffice/ByProductionOffices", productionOffices);
                proOfficesList = response.Data;

                if (proOfficesList.Count != 0)
                {
                    EnableField(true, true, false, false, true);
                    meta = response.Meta;
                }
                else
                {
                    Console.Write(response.Message);
                    EnableField(true, false, false, false, true);
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

                var response = await CallService.Get<List<SeriesDtoResponse>>("paramstrd/Series/BySeries", series);
                seriesList = response.Data;

                if (seriesList.Count != 0)
                {
                    EnableField(true, true, true, false, true);
                    meta = response.Meta;
                }
                else
                {
                    Console.Write(response.Message);
                    EnableField(true, true, false, false, true);
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

                var response = await CallService.Get<List<SubSeriesDtoResponse>>("paramstrd/SubSeries/BySubSeries", subSeries);
                subSeriesList = response.Data;

                if (subSeriesList.Count != 0)
                {
                    EnableField(true, true, true, true, false);
                    meta = response.Meta;
                }
                else
                {
                    Console.Write(response.Message);
                    EnableField(true, true, true, false,false);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener Officinas Productoras: {ex.Message}");
            }
        }

        public async Task ResetFormAsync()
        {
 
            docVersionList = new();
            adminUnitList = new();
            proOfficesList = new();
            seriesList = new();
            subSeriesList = new();

            isEnableAdminUnit = false;
            isEnableProOffice = false;
            isEnableSerie = false;
            isEnableSubSerie = false;
            isEnableButton = true;


            idDocVersion = 0;
            idAdminUnit = 0;
            idProOffice = 0;
            idSerie = 0;
            idSubSerie = 0;


            await GetDocumentalVersions();
        }

        #endregion TRDSearch

        #region Validations

        public void EnableField(bool a, bool b, bool c, bool d, bool e)
        {
            isEnableAdminUnit = a;
            isEnableProOffice = b;
            isEnableSerie = c;
            isEnableSubSerie = d;
            isEnableButton = e;

            idAdminUnit = a ? idAdminUnit : 0;
            idProOffice = b ? idProOffice : 0;
            idSerie = c ? idSerie : 0;
            idSubSerie = d ? idSubSerie : 0;
        }

        #endregion Validations
    }
}
