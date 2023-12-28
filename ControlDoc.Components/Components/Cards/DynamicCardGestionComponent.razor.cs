using Microsoft.AspNetCore.Components;

namespace ControlDoc.Components.Components.Cards
{
    public partial class DynamicCardGestionComponent
    {
        [Parameter] public string BorderColor { get; set; } = "#AB2222"; // Color por defecto

        [Parameter] public EventCallback<bool> OnClickCardAction { get; set; }
        [Parameter] public string? ImageUrl { get; set; }
        [Parameter] public string LabelText { get; set; } = "Label Default"; // Texto por defecto
        [Parameter] public string CardNumber { get; set; } = ""; // Número por defecto
        [Parameter] public string CardNumberPercen { get; set; } = ""; // Número por defecto

        private void OnClickCard()
        {
            OnClickCardAction.InvokeAsync(true);
        }
    }
}
