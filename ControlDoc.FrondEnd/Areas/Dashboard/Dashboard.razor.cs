using ControlDoc.Components.Components.Captcha;
using ControlDoc.Components.Components.DropDown;
using ControlDoc.Components.Components.DropDownListTelerik;
using ControlDoc.Models.Models.Administration.Response;
using ControlDoc.Models.Models.Pagination;
using ControlDoc.Services.Interfaces;
using ControlDoc.Services.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Drawing;

namespace ControlDoc.FrondEnd.Areas.Dashboard
{
    public partial class Dashboard
    {
        #region Variables
        #region Inject

        [Inject]
        private EventAggregatorService EventAggregator { get; set; }
        [Inject]
        private IJSRuntime Js { get; set; }


        [Inject] private ICallService CallService { get; set; }
        #endregion
        #endregion

        #region MethodLanguage
        private bool Changed = false;
        private bool isLoading = true;
        private DropDownListTelerik<CountryDtoResponse>? dropDownList;
        private DropDownListTelerik<CountryDtoResponse>? dropDownLists;

        protected override void OnInitialized()
        {
            EventAggregator.LanguageChangedEvent += HandleLanguageChanged;

        }
        //protected override async Task OnInitializedAsync()
        //{
        //    PageLoadService.MostrarSpinnerReadLoad(Js);
        //    var response = await CallService.Get<List<CountryDtoResponse>>("location/Country/ByFilter");
        //    CountryList = response.Data;
        //    PageLoadService.MostrarSpinnerReadLoad(Js);

        //    meta = response.Meta;
        //}

        protected override async Task OnInitializedAsync()
        {

            await GetDocumentalVersions();
            PageLoadService.OcultarSpinnerReadLoad(Js);
        }

        private async Task GetDocumentalVersions()
        {
            try
            {
                var response = await CallService.Get<List<DocumentalVersionsDtoResponse>>("paramstrd/DocumentalVersions/ByFilter");
                
                meta = response.Meta;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener las series: {ex.Message}");
            }
        }


        private async Task HandleLanguageChanged()
        {

            isLoading = false;
            Changed = true;
            StateHasChanged();
        }


        #endregion




        #region DateRangePicker
        public DateTime? StartValue { get; set; } = DateTime.Now;
        public DateTime? EndValue { get; set; } = DateTime.Now.AddDays(10);
        public DateTime Min = new DateTime(1990, 1, 1, 8, 15, 0);
        public DateTime Max = new DateTime(3000, 1, 1, 19, 30, 45);
        #endregion



        #region GraficaDona

        #region VariablesDona
        //variables necesarias 
        public class ModelData
        {
            public string Category { get; set; }
            public double Value { get; set; }
            public string color { get; set; }
        }
        #endregion VariablesDona

        public List<ModelData> Data = new List<ModelData>()
    {
           new ModelData()
        {
            Category = "EnProceso",
            Value = 5,
            color = "#AB2222"
        },
        new ModelData()
        {
            Category = "EnTransito",
            Value = 20,
            color = "#EAD519"

        },
        new ModelData()
        {
            Category = "GestiónExitosa",
            Value = 40,
            color = "#82A738"

        }
    };

        #endregion


        private bool click = false;

        #region Fecha
        private async Task ChangeDate()
        {
            click = true;
        }
        private async Task ChangeDate2()
        {
            click = false;
        }
        #endregion


        private Meta meta;

        private string? MySelectedIdCountry { get; set; }
        private int MySelectedIdCountrys { get; set; }
        private List<CountryDtoResponse> CountryList;
        
        
        private async Task GetCountry()
        {
            var assdasdasasd = dropDownList.SelectedValuestring;
            MySelectedIdCountry = dropDownList.SelectedValuestring;
            MySelectedIdCountrys = dropDownLists.SelectedValueint;
            try
            {
                var response = await CallService.Get<List<CountryDtoResponse>>("location/Country/ByFilter");
                CountryList = response.Data;

                meta = response.Meta;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los países: {ex.Message}");
            }
        }


    }
}
