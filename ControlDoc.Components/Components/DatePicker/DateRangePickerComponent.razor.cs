using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;


namespace ControlDoc.Components.Components.DatePicker
{
    public partial class DateRangePickerComponent : ComponentBase
    {
        #region Parameter
        [Parameter] public EventCallback<Dictionary<string, object>> OnDatesEntered { get; set; }
        [Parameter] public DateTime CalendarDate { get; set; }


        #endregion

        #region Injects

        [Inject] private IJSRuntime? JS { get; set; }
        #endregion

        private string date = string.Empty;

        protected override void OnInitialized()
        {
            date = $"{CalendarDate.Month:D2}/{CalendarDate.Day:D2}/{CalendarDate.Year} - " +
                   $"{CalendarDate.Month:D2}/{CalendarDate.Day:D2}/{CalendarDate.Year}";
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender) await FirstRender();
        }

        private async Task FirstRender()
        {
            await JS.InvokeVoidAsync("initializeDateRangePicker", DotNetObjectReference.Create(this));

            // Ocultar el cuadro por defecto si se encuentra en el DOM
            await JS.InvokeVoidAsync("hideDefaultBox");
        }

        [JSInvokable]
        public async Task HandleDateRangeSelection(string start, string end, string label)
        {
            var values = new Dictionary<string, object>()
            {
                {nameof(start), start },
                {nameof(end), end}
            };

            await OnDatesEntered.InvokeAsync(values);
        }



    }
}
