using Microsoft.AspNetCore.Components;
using System.Text.RegularExpressions;

namespace ControlDoc.Components.Components.Input
{
    public enum InputTypeEnum
    {
        text,
        password,
        email,
        Code
    }
    public partial class InputComponent : ComponentBase
    {
        #region Fields
        private string[] realValues = new string[6] { "", "", "", "", "", "" };
        private string[] displayValues = new string[6];
        private string inputValue = "";
        private string inputType = "password";
        private string iconClass = "fa fa-eye";
        #endregion

        #region Properties
        [Parameter]
        public bool ShowErrors { get; set; } = false;

        [Parameter]
        public InputTypeEnum InputType { get; set; } = InputTypeEnum.text;

        [Parameter]
        public string Placeholder { get; set; } = "Introduzca un valor";

        [Parameter]
        public string ErrorMessage { get; set; } = "Error con la entrada";

        [Parameter]
        public EventCallback<bool> OnValidation { get; set; }

        public bool IsInvalid { get; private set; }

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
        public string GetCompleteCodeValue()
        {
            return string.Join("", realValues);
        }

        private void HandleInput(ChangeEventArgs e, int index)
        {
            string? newValue = e.Value?.ToString();
            if (newValue != null && newValue.Length > 0)
            {
                realValues[index] = newValue.Last().ToString().ToUpper();
                displayValues[index] = "•";
            }
            else
            {
                realValues[index] = "";
                displayValues[index] = "";
            }

            inputValue = string.Concat(realValues.Where(v => !string.IsNullOrEmpty(v)));
            this.StateHasChanged();
        }

        private void TogglePasswordVisibility()
        {
            if (InputType == InputTypeEnum.password)
            {
                inputType = inputType == "password" ? "text" : "password";
                iconClass = inputType == "password" ? "fa fa-eye" : "fa fa-eye-slash";
            }
        }

        public void Reset()
        {
            inputValue = "";
            realValues = new string[6] { "", "", "", "", "", "" };
            displayValues = new string[6];
            inputType = "password";
            iconClass = "fas fa-eye-slash";
            IsInvalid = false;
            StateHasChanged();
        }
        #endregion

        #region Validation
        public void ValidateInput()
        {
            switch (InputType)
            {
                case InputTypeEnum.password:
                    ValidatePassword();
                    break;
                case InputTypeEnum.email:
                    ValidateEmail();
                    break;
                case InputTypeEnum.text:
                    ValidateText();
                    break;
                case InputTypeEnum.Code:
                    ValidateCode();
                    break;
                default:
                    throw new InvalidOperationException("Tipo de entrada no válido");
            }

            OnValidation.InvokeAsync(IsInvalid);
        }

        private void ValidatePassword()
        {
            IsInvalid = string.IsNullOrWhiteSpace(inputValue) || inputValue.Length < 6;
        }

        private void ValidateEmail()
        {
            var emailRegex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            IsInvalid = string.IsNullOrWhiteSpace(inputValue) || !emailRegex.IsMatch(inputValue);
        }

        private void ValidateText()
        {
            IsInvalid = string.IsNullOrWhiteSpace(inputValue);
        }

        private void ValidateCode()
        {
            IsInvalid = inputValue.Length != 5;
        }

        private void ResetValidation()
        {
            IsInvalid = false;
        }
        #endregion
    }
}