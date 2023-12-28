using Microsoft.AspNetCore.Components;

namespace ControlDoc.Components.Components.DropDownListTelerik
{
    public partial class DropDownListTelerik<T> : ComponentBase
    {
        [Parameter] public string? LabelDDL { get; set; }
        [Parameter] public string DefaultText { get; set; } = "Seleccione una opción...";
        [Parameter] public string? Id { get; set; }
        [Parameter] public string? TextField { get; set; }
        [Parameter] public string? ValueField { get; set; }
        [Parameter] public string? ValueType { get; set; } = "int";
        [Parameter] public bool Filterable { get; set; } = false;
        [Parameter] public int SelectedValueint { get; set; } = 0;
        [Parameter] public string? SelectedValuestring { get; set; } = "";
        [Parameter] public bool Enabled { get; set; }
        [Parameter] public List<T> Data { get; set; }
        [Parameter] public EventCallback<int> OnValueEntered { get; set; }
        [Parameter] public EventCallback<string> OnValueEnteredString { get; set; }

        private void EnviarId()
        {
            OnValueEntered.InvokeAsync(SelectedValueint);
        }

        private void EnviarString()
        {
            OnValueEnteredString.InvokeAsync(SelectedValuestring);
        }
        public void resetDropDown()
        {
            StateHasChanged();
        }

        public void changeValue(string newValue)
        {
            ValueField = newValue;
            resetDropDown();
        }

        private async Task OnDropDownValueChanged(int newValue)
        {
            SelectedValueint = newValue;
            await OnValueEntered.InvokeAsync(SelectedValueint);
        }

    }
}
