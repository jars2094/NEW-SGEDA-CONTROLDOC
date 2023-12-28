using ControlDoc.Components.Components.DropDown;
using ControlDoc.Components.Components.Input;
using ControlDoc.Components.Components.Modals;
using ControlDoc.Services.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ControlDoc.ComponentViews.ComponentViews.Authentication.ChangePassword
{
    public partial class ChangePassword
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
        private InputComponent? passwordConfirmInput;
        private InputComponent? passwordInput;

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
            public string ConfirmPassword { get; set; } = string.Empty;

            public string Password { get; set; } = string.Empty;
        }

        private string setKeyName(string key)
        {
            return DropDownLanguageComponent.GetText(key);
        }

        private async Task HandleValidSubmit()
        {
            formSubmitted = true;

            passwordInput?.ValidateInput();
            passwordConfirmInput?.ValidateInput();
            // Implementar el método HandleChangePasswordSubmit
            ResetForm();
        }

        private void ResetForm()
        {
            passwordInput?.Reset();
            passwordConfirmInput?.Reset();

            formSubmitted = false;


        }

        private async Task HandleChangePasswordSubmit()
        {
            //ToDoHandleChangePasswordSubmit
        }
    }
}
