using Microsoft.AspNetCore.Components;

namespace ControlDoc.Components.Components.Input
{
    public partial class CheckBoxComponent
    {
        #region Parameter

        [Parameter] public bool isChecked { get; set; }
        [Parameter] public string TextColor { get; set; } = "black";
        [Parameter] public string FontFamily { get; set; } = "Arial, sans-serif";
        [Parameter] public string FontStyle { get; set; } = "normal";
        [Parameter] public string LabelText { get; set; }
        [Parameter] public string CheckboxColor { get; set; } = "blue";
        [Parameter] public string CheckboxBorderColor { get; set; } = "black";
        [Parameter] public string CheckIconColor { get; set; } = "blue";
        [Parameter] public string CheckIcon { get; set; } = "&#10003;"; // Default check icon


        void ToggleCheckbox()
        {
            isChecked = !isChecked;
        }

        #endregion
        #region Methods
        //private void HandleCheckboxChange(ChangeEventArgs e)
        //{
        //    isChecked = (bool)e.Value;
        //    StateHasChanged(); // Forzar la actualización del componente
        //}
        #endregion
    }

}
