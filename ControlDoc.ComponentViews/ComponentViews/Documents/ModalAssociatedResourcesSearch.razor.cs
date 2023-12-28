using ControlDoc.Components.Components.Input;
using ControlDoc.Components.Components.Modals;
using ControlDoc.Models.Models.Administration.Response;
using ControlDoc.Models.Models.Documents.Response;
using ControlDoc.Models.Models.Generic;
using ControlDoc.Models.Models.Pagination;
using ControlDoc.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDoc.ComponentViews.ComponentViews.Documents
{
    public partial class ModalAssociatedResourcesSearch
    {


        #region InputsReference
        private InputModalComponent IdExpedienteInput;

        #endregion

        #region List
        private List<VDocumentaryTypologyResponse> docTypologyList;
        private List<DocumentaryTypologyDtoResponse> docTypologiesList;
        private List<DocumentalVersionsDtoResponse> docVersionList;
        private List<AdministrativeUnitDtoResponse> adminUnitList;
        private List<ProductionOfficesDtoResponse> proOfficesList;
        private List<SeriesDtoResponse> seriesList;
        private List<SubSeriesDtoResponse> subSeriesList;
        #endregion

        #region Variables
        #region Injects
        [Inject]
        private ICallService CallService { get; set; }
        #endregion

        #region Entorno
        private int numDoc = 1113018;
        private bool modalStatus = false;
        private bool IsEditForm = false;
        private int idDocVersion { get; set; }
        private int idAdminUnit { get; set; }
        private int idProOffice { get; set; }
        private int idSerie { get; set; }
        private int idSubSerie { get; set; }

        private bool isEnableAdminUnit = false;
        private bool isEnableProOffice = false;
        private bool isEnableSerie = false;
        private bool isEnableSubSerie = false;
        private bool isEnableDocTypology = false;
        private bool isVisibleTypologyNameInput = false;
        private bool isEnableButton = true;
        #endregion
        #endregion

        #region Objects
        private ModalNotificationsComponent notificationModal;
        private ManagementTrayDtoResponse AssociatedFormResponse;
        private Meta meta;
        #endregion

        #region cerrarModal
        private void HandleModalClosed(bool status)
        {

            modalStatus = status;
            //DocTemplateFormResponse = new DocTemplateDtoResponse();

            StateHasChanged();
        }
        #endregion

        #region Identificar Editar/Crear
        private async Task HandleValidSubmit()
        {
            // Lógica de envío del formulario
            if (IsEditForm)
            {

                //await PutTemplateDoc();

            }
            else
            {

                //await PostTemplate();

            }

            StateHasChanged();

        }
        #endregion

        #region ModalNotifcation
        public void UpdateModalStatus(bool newValue)
        {
            modalStatus = newValue;
            StateHasChanged();
        }


        private void HandleModalNotiClose(ModalClosedEventArgs args)
        {
            if (notificationModal.Type == ModalNotificationsComponent.ModalType.Success)
            {
                UpdateModalStatus(args.ModalStatus);
            }


        }
        #endregion

        #region GridConsulta

        public class ModelData
        {
            public string SegmentName { get; set; }
            
        }
        public List<ModelData> Data = new List<ModelData>();
    

        #endregion



        #region DropDownTRD

        #region EnableField
        public void EnableField(bool a, bool b, bool c, bool d, bool e, bool f, bool g)
        {
            isEnableAdminUnit = a;
            isEnableProOffice = b;
            isEnableSerie = c;
            isEnableSubSerie = d;
            isEnableDocTypology = e;
            isEnableButton = g;

            idAdminUnit = a ? idAdminUnit : 0;
            idProOffice = b ? idProOffice : 0;
            idSerie = c ? idSerie : 0;
            idSubSerie = d ? idSubSerie : 0;
            
        }
        #endregion

        #region AdministrativeUnit
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
        #endregion

        #region ProductionOffice
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
        #endregion

        #region Series
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

        #endregion

        #region SubSeries
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
        #endregion

        #endregion
    }
}
