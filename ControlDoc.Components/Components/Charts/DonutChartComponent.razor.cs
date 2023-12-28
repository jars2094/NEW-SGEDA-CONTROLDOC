using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Blazor;

namespace ControlDoc.Components.Components.Charts
{
    public partial class DonutChartComponent : ComponentBase
    {

        [Parameter] public string ChartTitleText { get; set; } = "Seleccione una opción...";
        [Parameter] public bool ChartTitleVisible { get; set; } = false;
        private ChartLegendPosition filterOperator { get; set; } = ChartLegendPosition.Top;

        [Parameter]
        public List<ChartLegendPosition>? filterOperators { get; set; } = new List<ChartLegendPosition>()
    {
        ChartLegendPosition.Top,
        ChartLegendPosition.Right,
        ChartLegendPosition.Bottom,
        ChartLegendPosition.Custom,
        ChartLegendPosition.Left
    };
        [Parameter] public bool ChartLegendVisible { get; set; } = false;
        [Parameter] public bool ChartSeriesTooltipVisible { get; set; } = false;
        [Parameter] public bool ChartSeriesLabelsVisible { get; set; } = false;
        [Parameter] public string? TextField { get; set; }
        [Parameter] public string? ValueField { get; set; }
        [Parameter] public string? ColorField { get; set; }

        [Parameter] public IEnumerable<object>? Data { get; set; }
        [Parameter] public EventCallback<int> OnValueEntered { get; set; }

        protected override void OnInitialized()
        {

        }
    }
}
