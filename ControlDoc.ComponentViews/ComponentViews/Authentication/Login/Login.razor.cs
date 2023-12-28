using ControlDoc.Components.Components.Captcha;
using ControlDoc.Components.Components.DropDown;
using ControlDoc.Components.Components.Input;
using ControlDoc.Components.Components.Modals;
using ControlDoc.ComponentViews.ComponentViews.Authentication.CodeRecovery;
using ControlDoc.Models.Models.Authentication.Login.Request;
using ControlDoc.Services.Interfaces;
using ControlDoc.Services.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ControlDoc.ComponentViews.ComponentViews.Authentication.Login
{
    public partial class Login : ComponentBase
    {
        #region Variables
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

        [Inject] 
        private ICallService CallService { get; set; }

        [Inject]
        public IAuthenticationJWT? AuthenticationJWT { get; set; }
        #endregion
        #endregion

        public string ComponenteRenderizar { get; set; } = "CodeRecovery";

        private FormModel formModel = new FormModel();
        private InputComponent? nameInput;
        private InputComponent? passwordInput;
        private CaptchaComponent? captcha;
        private bool formSubmitted = false;
        private ModalComponent? modal { get; set; }


        private string userEnteredCaptcha = string.Empty;
        LoginUserDtoRequest loginUserDtoRequest = new LoginUserDtoRequest();
        protected override async Task OnInitializedAsync()
        {
            await AuthenticationJWT.LogoutToken();
            // Esperar a que DropDownLanguageComponent esté completamente inicializado
            while (DropDownLanguageComponent.LanguageCache == null)
            {
                await Task.Delay(100);
            }

            EventAggregator.LanguageChangedEvent += HandleLanguageChanged;
            authenticationStateContainer.ComponentChange += StateHasChanged;
            //return Task.CompletedTask;
        }
        private void CambiarComponente()
        {
            authenticationStateContainer.SelectedComponentChanged("PasswordRecovery");
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

        private string setKeyName(string key)
        {
            return DropDownLanguageComponent.GetText(key);
        }

        private async Task HandleValidSubmit()
        {
            formSubmitted = true;
            nameInput?.ValidateInput();
            passwordInput?.ValidateInput();

            if (userEnteredCaptcha != captcha?.CaptchaValue)
            {
                await modal!.MostrarNotificacion("Captcha Incorrecto", "Vuelva a intentar el Captcha", ModalComponent.Icons.error, "Aceptar", "");
                return;
            }
            else
            {
                if (nameInput?.IsInvalid == true || passwordInput?.IsInvalid == true)
                {
                    await modal!.MostrarNotificacion("Usuario o contraseña invalidos!", "Digita correctamente sus credenciales", ModalComponent.Icons.warning, "Aceptar", "");
                    return;
                }
                else
                {
                    await HandleLoginSubmit();
                }
            }

            ResetForm();
        }

        private async Task HandleLoginSubmit()
        {
            loginUserDtoRequest.UserName = nameInput?.InputValue ?? string.Empty;
            loginUserDtoRequest.Password = passwordInput?.InputValue ?? string.Empty;
            loginUserDtoRequest.Ip = "1.1.1.1";
            loginUserDtoRequest.Uuid = "b2b196bc-54dd-452c-9b2c-b30b89fb19e3";
            loginUserDtoRequest.CompanyId = 17;

            try
            {
                var loginResponse = await CallService.Post<object, LoginUserDtoRequest>("security/Session/CreateLogin", loginUserDtoRequest);
                if (loginResponse.Succeeded)
                {
                    authenticationStateContainer.Parametros(loginUserDtoRequest.UserName, loginUserDtoRequest.Uuid, loginUserDtoRequest.Ip);
                    //await modal!.MostrarNotificacion("Exitoso!", "Se ha logueado Exitosamente!", ModalComponent.Icons.success, "Aceptar", "");

                    authenticationStateContainer.SelectedComponentChanged("CodeRecovery");
                }
                else
                {
                    await modal!.MostrarNotificacion("Error!", "Usuario o Contraseña Incorrectos!", ModalComponent.Icons.error, "Aceptar", "");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al intentar iniciar sesión: {ex.Message}");
            }
        }



        private void OnCaptchaEntered(string captcha)
        {
            userEnteredCaptcha = captcha;
        }

        private void ResetForm()
        {
            nameInput?.Reset();

            passwordInput?.Reset();
            formSubmitted = false;
        }

        private class FormModel
        {
            public string Name { get; set; } = string.Empty;

            public string Password { get; set; } = string.Empty;
        }
    }

}
