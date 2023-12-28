using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Text.RegularExpressions;

namespace ControlDoc.Components.Components.Input
{
    public partial class CodeInputComponent : ComponentBase
    {
        #region Fields
        
        private string inputValue = "";
        #endregion

        #region Injected Services
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }
        #endregion

        #region Properties
        [Parameter]
        public EventCallback<bool> OnValidation { get; set; }

        public bool IsInvalid { get; private set; }

        private string displayValue;

        public string DisplayValue
        {
            get => displayValue;
            set
            {
                if (value != null)
                {
                    // Elimina los guiones y guarda el valor real
                    InputValue = new string(value.Where(char.IsLetterOrDigit).ToArray());

                    // Añade guiones para la presentación visual
                    displayValue = Regex.Replace(InputValue, ".{1}", "$0-").TrimEnd('-');
                }
            }
        }

        public string InputValue
        {
            get => inputValue;
            set
            {
                if (inputValue != value)
                {
                    inputValue = value;
                }
            }
        }
        #endregion

        #region Methods
        

        public void HandleInput(ChangeEventArgs e)
        {
            DisplayValue = e.Value?.ToString().ToUpper();
            StateHasChanged();
        }


        public void Reset()
        {
            inputValue = "";
            IsInvalid = false;
            StateHasChanged();
        }
        #endregion

        #region Validation
        public void ValidateInput()
        {
            ValidateCode();
            OnValidation.InvokeAsync(IsInvalid);
        }

        private void ValidateCode()
        {
            IsInvalid = inputValue.Length != 6;
        }

        private void ResetValidation()
        {
            IsInvalid = false;
        }
        #endregion
    }

}
