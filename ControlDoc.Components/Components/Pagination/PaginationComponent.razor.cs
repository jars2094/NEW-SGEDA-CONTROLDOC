using ControlDoc.Models.Models.Administration.Response;
using ControlDoc.Models.Models.Generic;
using ControlDoc.Models.Models.Pagination;
using ControlDoc.Services.Interfaces;
using ControlDoc.Services.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Telerik.SvgIcons;
using static System.Net.WebRequestMethods;

namespace ControlDoc.Components.Components.Pagination
{
    public class DropdownOption
    {
        public int PageNumber { get; set; }
        public string PageName { get; set; }
    }
    public partial  class PaginationComponent<T> : ComponentBase where T : class
    {
        #region Variables
        #region Injects
        [Inject]
        private IJSRuntime Js { get; set; }

        [Inject]
        private ICallService CallService { get; set; }
        #endregion
        #endregion

        [Parameter] public Meta? ObjectMeta { get; set; }
        [Parameter] public List<T> DataObjectList { get; set; }
        [Parameter] public Dictionary<string, dynamic> Headers { get; set; } = new Dictionary<string, dynamic>();

        [Parameter] public EventCallback<List<T>> OnPaginationRefresh { get; set; }

        List<DropdownOption> dropdownOptions = new List<DropdownOption>();
        
        public int selectedPage;
        private int totalPages;

        private bool leftButtonEnabled = false;
        private bool rightButtonEnabled = true;

        private string GetLeftButtonImage()
        {
            return leftButtonEnabled ? "..\\img\\leftOn.svg" : "..\\img\\leftOff.svg";
        }

        private string GetRightButtonImage()
        {
            return rightButtonEnabled ? "..\\img\\rightOn.svg" : "..\\img\\rightOff.svg";
        }
        private async Task GoToPreviousPage()
        {
            if (selectedPage > 1)
            {
                await OnPageSelectedAsync(selectedPage - 1);
            }
        }

        private async Task GoToNextPage()
        {
            if (selectedPage < totalPages)
            {
                await OnPageSelectedAsync(selectedPage + 1);
            }
        }

        private void SetDataListPages()
        {
            for (int i = 1; i <= ObjectMeta.totalPages; i++)
            {
                dropdownOptions.Add(new DropdownOption { PageNumber = i, PageName = $"Página {i}" });
            }
            selectedPage = 1;
            totalPages = dropdownOptions.Count;
        }

        public static string ModifyUrl(string originalUrl, int pageNumber, int pageSize)
        {
            var uriBuilder = new UriBuilder(originalUrl);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);

            // Actualizar los parámetros pageNumber y pageSize
            query["pageNumber"] = pageNumber.ToString();
            query["pageSize"] = pageSize.ToString();

            uriBuilder.Query = query.ToString();
            string url = uriBuilder.ToString();
            Uri uri = new Uri(url);

            string baseUrl = uri.GetLeftPart(UriPartial.Authority);
            string remainingPart = url.Substring(baseUrl.Length);
            if (remainingPart.StartsWith("/"))
            {
                remainingPart = remainingPart.Substring(1);
            }


            return remainingPart;
        }

        private async Task OnPageSelectedAsync(int selectedPageNumber)
        {
            PageLoadService.MostrarSpinnerReadLoad(Js);
            selectedPage = selectedPageNumber;

            // Actualizar los botones
            leftButtonEnabled = selectedPage > 1;
            rightButtonEnabled = selectedPage < totalPages;

            string MicroServicesUrl = ModifyUrl(ObjectMeta.firstPageUrl, selectedPage, ObjectMeta.pageSize);
            var requestHeaders = Headers.Any() ? Headers : null;
            var response = await CallService.Get<List<T>>(MicroServicesUrl, requestHeaders);
            DataObjectList = response.Data;
            await OnPaginationRefresh.InvokeAsync(DataObjectList);
            PageLoadService.OcultarSpinnerReadLoad(Js);
        }

        protected override void OnInitialized()
        {
            SetDataListPages();
        }

    }
}
