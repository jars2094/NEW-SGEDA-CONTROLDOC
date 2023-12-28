using static System.Net.Mime.MediaTypeNames;
using Toolbelt.Blazor.SpeechSynthesis;
using Microsoft.AspNetCore.Components;
using ControlDoc.Services.Services;
using Microsoft.JSInterop;
using ControlDoc.Components.Components.DropDown;
using ControlDoc.Services.Interfaces;
using ControlDoc.Models.Models.Components.Chatcha.Request;

namespace ControlDoc.Components.Components.Captcha
{
    public partial class CaptchaComponent : ComponentBase
    {
        #region Inject
        [Inject]
        private SpeechSynthesis SpeechSynthesis { get; set; }
        [Inject]
        private EventAggregatorService EventAggregator { get; set; }
        [Inject]
        private IJSRuntime js { get; set; }
        [Inject]
        private ICallService callService { get; set; }
        #endregion

        #region Parameters
        [Parameter]
        public EventCallback<string> OnCaptchaEntered { get; set; }
        #endregion

        #region Private Fields
        private string lastRequestTime = "Never";
        private Timer? timer;
        public string CaptchaValue => captcha;
        private string captcha = string.Empty;
        //CompareCaptchaDtoRequest compareCaptchaDtoRequest = new CompareCaptchaDtoRequest();
        #endregion

        #region Methods

        #region Language Handling
        // Método que se suscribe al método de publicación del servicio de traducción
        private async Task HandleLanguageChanged()
        {
            StateHasChanged();
        }
        private string setKeyName(string key)
        {
            return DropDownLanguageComponent.GetText(key);
        }
        #endregion

        #region Event Handling
        private async Task OnCaptchaInputChanged(ChangeEventArgs e)
        {
            string userInput = e.Value.ToString();
            await OnCaptchaEntered.InvokeAsync(userInput);
        }
        #endregion

        #region Initialization
        protected override void OnInitialized()
        {
            //Ejecuta la primera petición inmediatamente y luego cada minuto
            TimerCallback timerCallback = _ => PeticionCaptcha();
            timer = new Timer(timerCallback, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
            EventAggregator.LanguageChangedEvent += HandleLanguageChanged;
        }
        #endregion

        #region API Calls
        // Método para traer de la API el Captcha
        private async void PeticionCaptcha()
        {
            var captchaResponse = await callService.Get<string>("security/Captcha/GetCaptcha",null);
            captcha = captchaResponse.Data!;
            lastRequestTime = DateTime.Now.ToString();
            StateHasChanged();
        }
        #endregion

        #region Cleanup
        public void Dispose()
        {
            timer.Dispose();
        }
        #endregion

        #region Speech Synthesis
        private float Speed = 0.5f; // Puedes ajustar la velocidad aquí

        private async Task onClickSpeak()
        {
            var options = new SpeechSynthesisUtterance
            {
                Text = captcha,
                Rate = Speed // Ajusta la velocidad aquí (1.0 es la velocidad normal)
            };

            await this.SpeechSynthesis.SpeakAsync(options);
        }
        #endregion

        #endregion  // Fin de la región "Métodos"
    }
}