using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace ControlDoc.Components.Components.Input
{
    public enum ValidationType
    {
        None,
        NotEmpty,
        Name,
        Code,
        Website,
        Email,
        NumbersOnly
    }

    public partial class InputModalComponent : ComponentBase
    {
        #region Component Parameters

        // Indica si el campo es obligatorio.
        [Parameter] public bool IsRequired { get; set; } = false;

        // Texto de etiqueta del campo.
        [Parameter] public string LabelText { get; set; } = "Text not provided";

        // Placeholder del campo.
        [Parameter] public string Placeholder { get; set; } = "No text provided";

        // Tipo de validación para el campo.
        [Parameter] public ValidationType FieldType { get; set; } = ValidationType.None;

        [Parameter] public bool IsVisible { get; set; } = true;
        [Parameter] public string InputType { get; set; } = "text";
        [Parameter] public bool IsDisabled { get; set; } = false;
        [Parameter] public EventCallback<string> InputValueChanged { get; set; }
        [Parameter] public EventCallback<string> MethodValueChanged { get; set; }



        private Dictionary<string, object> GetInputAttributes()
        {
            var attributes = new Dictionary<string, object>
        {

            { "placeholder", Placeholder },
            { "value", InputValue } // Remueve @bind="InputValue" y maneja los cambios manualmente si es necesario
        };

            if (IsRequired)
            {
                attributes.Add("required", "required");
            }

            if (IsDisabled)
            {
                attributes.Add("disabled", "disabled");
            }

            return attributes;
        }

        #endregion

        #region Private Variables


        // Valor de entrada del campo.
        
        private bool IsValid()
        {
            if (Validate(FieldType, InputValue))
            {
                return true;
            }

            // Aquí puedes mostrar un mensaje de error específico para cada tipo de validación si lo deseas.
            return false;
        }

        private string _inputValue;

        [Parameter]
        public string InputValue
        {
            get => _inputValue;
            set
            {
                if (_inputValue != value)
                {
                    _inputValue = value;
                    InputValueChanged.InvokeAsync(value); // Notifica al componente padre
                    MethodValueChanged.InvokeAsync(value); // Notifica al componente padre
                    
                }
            }
        }

        
        public bool IsInputValid
        {
            get
            {
                return IsValid();
            }
        }
        #endregion

        #region Validation Methods

        // Subregión para microvalidaciones

        // Verifica si el valor contiene solo números.
        private bool IsNumbersOnlyValid(string value)
        {
            return Regex.IsMatch(value, "^[0-9]+$");
        }

        // Verifica si el valor no está vacío.
        private bool IsNotEmpty(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        // Verifica si el valor contiene solo letras.
        private bool IsNameValid(string value)
        {
            return Regex.IsMatch(value, "^[a-zA-Z\\s]+$");
        }

        // Verifica si el valor es válido para un código.
        private bool IsCodeValid(string value)
        {
            return value.Length <= 5 && Regex.IsMatch(value, "^[a-zA-Z0-9]+$");
        }

        // Verifica si el valor es una URL válida.
        private bool IsWebsiteValid(string value)
        {
            return Uri.TryCreate(value, UriKind.Absolute, out _);
        }

        // Verifica si el valor es una dirección de correo electrónico válida.
        private bool IsEmailValid(string value)
        {
            try
            {
                var address = new System.Net.Mail.MailAddress(value);
                return address.Address == value;
            }
            catch
            {
                return false;
            }
        }

        // Subregión para la validación principal basada en el tipo de validación seleccionado.
        private bool Validate(ValidationType validationType, string value)
        {
            switch (validationType)
            {
                case ValidationType.None:
                    return true;
                case ValidationType.NotEmpty:
                    return IsNotEmpty(value);
                case ValidationType.Name:
                    return IsNameValid(value);
                case ValidationType.Code:
                    return IsCodeValid(value);
                case ValidationType.Website:
                    return IsWebsiteValid(value);
                case ValidationType.Email:
                    return IsEmailValid(value);
                case ValidationType.NumbersOnly:
                    return IsNumbersOnlyValid(value);
                default:
                    return true;
            }
        }

        #endregion

        #region Component Methods

        // Limpia el campo estableciendo su valor en vacío.
        public void ClearField()
        {
            InputValue = string.Empty;
        }
        public void UpdateIsRequired(bool newValue)
        {
            IsRequired = newValue;
            StateHasChanged(); 
        }
        public void UpdateInputValue(string newValue)
        {
            InputValue = newValue;
            StateHasChanged();
        }

        public void UpdateIsDisabled(bool newValue)
        {
            IsDisabled = newValue;
        }




        #endregion

    }

}
