using ControlDoc.Components.Components.DropDown;
using ControlDoc.Components.Components.Input;
using ControlDoc.Components.Components.Modals;
using ControlDoc.Services.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ControlDoc.ComponentViews.ComponentViews.Authentication.Register
{
    public partial class Register : ComponentBase
    {
        #region Inject 
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

        private FormModel formModel = new FormModel();
        private InputComponent? nameInput;
        private InputComponent? passwordInput;
        private InputComponent? emailInput;
        private bool formSubmitted = false;
        private ModalComponent? modal { get; set; }

        protected override void OnInitialized()
        {
            EventAggregator.LanguageChangedEvent += HandleLanguageChanged;
        }
        private async Task HandleLanguageChanged()
        {
            StateHasChanged();
        }
        private class FormModel
        {
            public string Name { get; set; } = string.Empty;

            public string Password { get; set; } = string.Empty;
        }

        private string setKeyName(string key)
        {
            return DropDownLanguageComponent.GetText(key);
        }

        private async Task HandleValidSubmit()
        {
            formSubmitted = true;
            nameInput?.ValidateInput();
            passwordInput?.ValidateInput();
            emailInput?.ValidateInput();

            ResetForm();
        }

        private void ResetForm()
        {
            nameInput?.Reset();
            emailInput?.Reset();
            passwordInput?.Reset();
            formSubmitted = false;

            // Implementar el método HandleRegisterSubmit
        }

        private async Task HandleRegisterSubmit()
        {
            //ToDoHandleRegisterSubmit
        }
    }
}
