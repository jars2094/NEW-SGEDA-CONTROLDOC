using ControlDoc.Components.Components.DropDown;
using ControlDoc.Components.Components.Input;
using ControlDoc.Components.Components.Modals;
using ControlDoc.Models.Models.Authentication.Login.Request;
using ControlDoc.Services.Interfaces;
using ControlDoc.Services.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ControlDoc.ComponentViews.ComponentViews.Authentication.PasswordRecovery
{
    public partial class PasswordRecovery
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
        private InputComponent? emailInput;
        private bool formSubmitted = false;
        private ModalComponent? modal { get; set; }
        [Parameter]
        public EventCallback<string> BotonClick { get; set; }
        private void CambiarComponente()
        {
            authenticationStateContainer.SelectedComponentChanged("Login");
        }
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

            emailInput?.ValidateInput();

            if (emailInput?.IsInvalid == true)
            {
                await modal!.MostrarNotificacion("Usuario o contraseña invalidos!", "Digita correctamente sus credenciales", ModalComponent.Icons.warning, "Aceptar", "");
                Console.WriteLine("Envío Incorrecto"); // Aca irían los mensajes de alerta
                return;
            }
            else
            {
                await HandlePasswordRecoverySubmit();
            } 
            ResetForm();
        }

        private void ResetForm()
        {
            emailInput?.Reset();
            formSubmitted = false;
        }

        private async Task HandlePasswordRecoverySubmit()
        {
            var UUID = Guid.NewGuid();

            var headers = new Dictionary<string, dynamic>
                {
                    { "email", emailInput.InputValue },
                    { "UUID", UUID }
                };

            try
            {
                //var loginResponse = await CallService.Post<object, LoginUserDtoRequest>("security/Session/CreateLogin", loginUserDtoRequest, headers);
                //if (loginResponse.Succeeded)
                //{
                //    authenticationStateContainer.Parametros(emailInput.InputValue, UUID, loginUserDtoRequest.Ip);
                //    //await modal!.MostrarNotificacion("Exitoso!", "Se ha logueado Exitosamente!", ModalComponent.Icons.success, "Aceptar", "");
                //    Console.WriteLine("Inicio de sesión exitoso");

                //    Console.WriteLine(DropDownLanguageComponent.GetText("LabelTextUsuario"));

                //    authenticationStateContainer.SelectedComponentChanged("CodeRecovery");


                //}
                //else
                //{
                //    await modal!.MostrarNotificacion("Error!", "Usuario o Contraseña Incorrectos!", ModalComponent.Icons.error, "Aceptar", "");
                //    Console.WriteLine("Error en el inicio de sesión");
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al intentar iniciar sesión: {ex.Message}");
            }
            //ToDoHandlePasswordRecoverySubmit
        }
    }
}
