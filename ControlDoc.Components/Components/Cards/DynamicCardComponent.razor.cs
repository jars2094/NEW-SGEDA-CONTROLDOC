﻿using Microsoft.AspNetCore.Components;

namespace ControlDoc.Components.Components.Cards
{
    public partial class DynamicCardComponent
    {
        [Parameter] public string BorderColor { get; set; } = "#AB2222"; // Color por defecto
        [Parameter] public string ShadowColor { get; set; } = "rgba(0, 0, 0, 0.25)"; // Sombra por defecto
        [Parameter] public string? ImageUrl { get; set; }
        [Parameter] public string LabelText { get; set; } = "Label Default"; // Texto por defecto
        [Parameter] public string CardNumber { get; set; } = "5"; // Número por defecto
        [Parameter] public string listaDeCosas { get; set; } = "5"; // Número por defecto
        [Parameter] public EventCallback<bool> OnClickCardAction { get; set; }

        private void OnClickCard()
        {
            OnClickCardAction.InvokeAsync(true);
        }
    }
}
