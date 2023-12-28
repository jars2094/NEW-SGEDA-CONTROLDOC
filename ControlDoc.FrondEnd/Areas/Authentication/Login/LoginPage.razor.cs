using ControlDoc.Components.Components.Captcha;
using ControlDoc.Components.Components.DropDown;
using ControlDoc.Components.Components.Input;
using ControlDoc.Components.Components.Modals;
using ControlDoc.ComponentViews.ComponentViews.Authentication.CodeRecovery;
using ControlDoc.Models.Models.Authentication.Login.Request;
using ControlDoc.Services.Interfaces;
using ControlDoc.Services.Models;
using ControlDoc.Services.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Toolbelt.Blazor.SpeechSynthesis;

namespace ControlDoc.FrondEnd.Areas.Authentication.Login
{
    public partial class LoginPage 
    {
        #region Variables
        #region Injects
        [Inject]
        private EventAggregatorService EventAggregator { get; set; }
        [Inject]
        private AuthenticationStateContainer authenticationStateContainer { get; set; }      
        [Inject]
        private IJSRuntime js { get; set; }
        [Inject]
        private SweetAlertService swal { get; set; }
        [Inject]
        private NavigationManager navigation { get; set; }
        #endregion
        #endregion

        private CodeRecovery codeRecovery;

        public string ComponenteRenderizar { get; set; } = "CodeRecovery";

        protected override async Task OnInitializedAsync()
        {
            // Esperar a que DropDownLanguageComponent esté completamente inicializado
            while (DropDownLanguageComponent.LanguageCache == null)
            {
                await Task.Delay(100);
            }

            EventAggregator.LanguageChangedEvent += HandleLanguageChanged;
            authenticationStateContainer.ComponentChange += StateHasChanged;
        }
        public void Dispose()
        {
            authenticationStateContainer.ComponentChange -= StateHasChanged;
        }
        public void ClickCallBack(string ComponenteRenderizarnew)
        {
            ComponenteRenderizar = ComponenteRenderizarnew;
            HandleLanguageChanged();
        }

        private async Task HandleLanguageChanged()
        {
            StateHasChanged();
        }
    }

}
