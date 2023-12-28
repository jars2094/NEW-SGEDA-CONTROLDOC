using ControlDoc.Components.Components.Input;
using ControlDoc.Models.Models.Administration.Response;
using ControlDoc.Models.Models.Pagination;
using ControlDoc.Services.Interfaces;
using ControlDoc.Services.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http.Headers;

namespace ControlDoc.FrondEnd.Areas.Administration
{
    public partial class Audit
    {
        #region Variables
        #region Injects
        [Inject]
        private IJSRuntime Js { get; set; }

        [Inject]
        private ICallService CallService { get; set; }
        #endregion
        #endregion

        private Dictionary<string, dynamic> headers { get; set; } = new();
        private DateTime? from { get; set; }
        private DateTime? to { get; set; }
        private InputModalComponent detailInput { get; set; } = new();

        private List<VWLogsAuditDtoBugResponse> vWLogsAuditDtoBugList { get; set; } = new();
        private Meta meta { get; set; } = new() { pageSize = 10 };
        private bool dataChargue = false;


        private string detail { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await GetAudit();
            StateHasChanged();
            PageLoadService.OcultarSpinnerReadLoad(Js);
        }

        private async Task OnClickSearch()
        {
            await GetAudit();
        }

        private void OnClickReset()
        {
            from = null;
            to = null;
            detail = "";
            detailInput = new();
            StateHasChanged();
        }

        private async Task GetAudit()
        {
            try
            {
                headers = new()
            {
                { "Detail", detail??"" },
                { "From", (from?.ToString("MM-dd-yyyy")??DateTime.MinValue.ToString("MM-dd-yyyy"))  },
                { "To", (to?.ToString("MM-dd-yyyy")??DateTime.MinValue.ToString("MM-dd-yyyy")) }
            };

                var response = await CallService.Get<List<VWLogsAuditDtoBugResponse>>($"audit/Log/ByFilter", headers) ?? null;

                if (response != null)
                {
                    vWLogsAuditDtoBugList = response.Data ?? new();

                    meta = response?.Meta;
                    dataChargue = true;
                }
            }
            catch
            {
                vWLogsAuditDtoBugList = new();
            }
        }

        private void HandlePaginationGrid(List<VWLogsAuditDtoBugResponse> newDataList)
        {
            vWLogsAuditDtoBugList = newDataList;
        }
    }
}