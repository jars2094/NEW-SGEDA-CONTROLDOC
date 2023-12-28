using ControlDoc.ComponentViews.ComponentViews.Administration;
using ControlDoc.ComponentViews.ComponentViews.GenericViews;
using ControlDoc.Models.Models.Administration.Response;
using ControlDoc.Models.Models.Generic;
using ControlDoc.Models.Models.Pagination;
using ControlDoc.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace ControlDoc.ComponentViews.ComponentViews.Documents
{
    public class ObjectTramite
    {
        public VUserDtoResponse UserInfo { get; set; }
        public string? Action { get; set; }
        public string? Days { get; set; }
        public string? Hours { get; set; }
        public string? Asunto { get; set; }
    }

    public class ValoresComunes
    {
        public string? Action { get; set; }
        public string? Days { get; set; }
        public string? Hours { get; set; }
        public string? Asunto { get; set; }
    }
    public partial class ModalManagementOfProcedures
    {
        #region Variables
        #region Injects
        [Inject]
        private ICallService CallService { get; set; }
        #endregion
        #endregion

        private GenericSearchModal userSearchModal;
        string Radicado = "R45678-211544-2023";
        string IdDocumento = "12457";
        string Anio = DateTime.Now.Year.ToString();
        private string? ValueTipoAction { get; set; }
        Decimal countCarac = 0;

        string panel_1 = "flex";
        string panel_2 = "none";
        string panel_3 = "none";
        private bool modalStatus = false;

        private List<SystemFieldsDtoResponse>? lstTipoActions { get; set; } = new();
        private List<ObjectTramite>? userSenderTramite { get; set; } = new();
        private ValoresComunes valoresComunes = new();
        private List<VUserDtoResponse> userListSenders = new();
        private List<VUserDtoResponse> userListCopies = new();

        #region Initialization
        protected override async Task OnInitializedAsync()
        {
            try
            {
                await GetDocumentTypeTCR();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al realizar la initialización: {ex.Message}");
            }
        }

        #endregion

        public void UpdateModalStatus(bool newValue)
        {
            modalStatus = newValue;
            StateHasChanged();
        }
        private void HandleModalClosed(bool status)
        {

            modalStatus = status;

            StateHasChanged();

        }

        private void EnablePanel(string value, decimal panel)
        {
            if (value == null)
            {
                panel_2 = "none";
                panel_3 = "none";
                ValueTipoAction = value;
            }
            else
            {
                switch (panel)
                {
                    case 1:
                        ValueTipoAction = value;
                        panel_2 = panel_1;
                        break;
                    case 2:
                        panel_3 = panel_1;
                        break;

                }
            }
        }


        private async Task GetDocumentTypeTCR()
        {
            try
            {
                Dictionary<string, dynamic> headers = new()
                {
                     { "ParamCode","ACO" }
                };

                var response = await CallService.Get<List<SystemFieldsDtoResponse>>("params/SystemFields/ByFilter", headers);
                lstTipoActions = response.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los tipos de documento: {ex.Message}");
            }
        }
        private void CountCharacters(ChangeEventArgs e)
        {
            String value = e.Value.ToString() ?? String.Empty;

            if (!string.IsNullOrEmpty(value))
            {
                countCarac = value.Length;

            }
            else
            {
                countCarac = 0;

            }
        }

        private void HandleUserSelectedChanged(MyEventArgs<List<object>> usersData)
        {
            userSenderTramite.Clear();
            var usersSearchResultList = usersData.Data.ToList();
            var senders = usersSearchResultList[0];
            var copies = usersSearchResultList[1];

            if (senders != null)
            {
                try
                {
                    userListSenders = (List<VUserDtoResponse>)senders;
                }
                catch (InvalidCastException)
                {
                    
                    userListSenders = new List<VUserDtoResponse>();
                }
            }
            if (copies != null)
            {
                try
                {
                    userListCopies = (List<VUserDtoResponse>)copies;
                }
                catch (InvalidCastException)
                {
                    
                    userListCopies = new List<VUserDtoResponse>();
                }
            }

            foreach (var sender in userListSenders)
            {
                var tramite = new ObjectTramite
                {
                    UserInfo = sender,
                    Action = null,
                    Days = null,
                    Hours = null,
                    Asunto = null
                };

                userSenderTramite.Add(tramite);
            }

            if(userSenderTramite.Count > 0)
            {
                panel_3 = panel_1;
            }

            userSearchModal.UpdateModalStatus(usersData.ModalStatus);
        }
        private async Task showModalSearchUser()
        {
            userSearchModal.UpdateModalStatus(true);
        }
    }
}
