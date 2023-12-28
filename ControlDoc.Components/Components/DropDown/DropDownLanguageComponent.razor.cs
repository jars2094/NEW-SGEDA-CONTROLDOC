//using ControlDoc.Control.Models.Enum;
//using ControlDoc.Control.Services.Features.Interfaces;
//using ControlDoc.Control.Services.Helpers;
using ControlDoc.Models.Enums;
using ControlDoc.Models.Models.Administration.Response;
using ControlDoc.Models.Models.Components.Language.Response;
using ControlDoc.Models.Models.Pagination;
using ControlDoc.Services.Interfaces;
using ControlDoc.Services.Services;
using Microsoft.AspNetCore.Components;
using System.Text.RegularExpressions;

namespace ControlDoc.Components.Components.DropDown
{
    public enum Language
    {
        ES,
        EN,
        FR,
        IT
    }

    public partial class DropDownLanguageComponent : ComponentBase
    {
        #region Variables
        #region Inject
        [Inject]
        private ILocalStorage LocalStorage { get; set; }

        [Inject]
        private EventAggregatorService EventAggregator { get; set; }

        [Inject] 
        private ICallService CallService { get; set; }

        #endregion Inject

        #region Parameters

        [Parameter] public string TextColor { get; set; } = "black";
        [Parameter] public string TextSize { get; set; } = "20px";
        [Parameter] public string ArrowIconColor { get; set; } = "black";

        #endregion Parameters
        #endregion

        #region Private Fields

        private string CurrentLanguage = "ES"; //Idioma predeterminado
        private string CodeLanguage;

        public static Dictionary<string, string>? LanguageCache;

        public List<PhraseDtoResponse> phraseDtoResponses { get; set; } = new List<PhraseDtoResponse>();
        private List<LanguageDtoResponse> languageDtoResponses { get; set; } = new List<LanguageDtoResponse>();
        private List<LanguageDtoResponse> languageDtoResponsesAux { get; set; } = new List<LanguageDtoResponse>();

        private string DefaultText ;

        #endregion Private Fields

        #region Methods

        public async Task<Dictionary<string, string>> LanguageSelected(string language)
        {

            bool validate = await LocalStorage.ContainsKey(ValuesKeys.Diccionario);

            if (validate)
            {
             
                await LocalStorage.RemoveItem(ValuesKeys.Diccionario);
            }

            //var headers = new Dictionary<string, dynamic>
            //{
            //    { "code", language.ToString() }
            //};

            var Peticion = await CallService.Get<List<PhraseDtoResponse>>($"translation/Language/TranslationByCode?code={language}");
            if (Peticion.Succeeded)
            {
                phraseDtoResponses = Peticion.Data;
                LanguageCache = phraseDtoResponses!
                            .ToDictionary(item => item.KeyPhrase.KeyName, item => item.TextPhrase);

                await LocalStorage.SetValue(ValuesKeys.Diccionario, LanguageCache);

                // Notificar a los componentes que el idioma ha cambiado
                await EventAggregator.PublishLanguageChanged();

                return LanguageCache;
            }
            else
            {
                LanguageCache = new();
            }

            return LanguageCache;
        }

        public static string GetText(string key) =>
        LanguageCache?.GetValueOrDefault(key) ?? "key no encontrada";

        #region Initialization

        protected override async Task OnInitializedAsync()
        {
            try
            {
                

                LanguageCache = await LocalStorage.GetValue<Dictionary<string, string>>(ValuesKeys.Diccionario);

                if (LanguageCache == null)
                {
                    LanguageCache = await LanguageSelected(CurrentLanguage);
                    await LocalStorage.SetValue(ValuesKeys.Diccionario, LanguageCache);

                }
                await GetLanguages();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en la inicialización de DropDownLanguageComponent: {ex.Message}");
            }
        }

        #endregion Initialization

        private async Task GetLanguages()
        {
            try
            {
                var response = await CallService.Get<List<LanguageDtoResponse>>("translation/Language/Get");
                languageDtoResponses = updateListText(response.Data).Result;
                DefaultText = languageDtoResponses.Where(s => s.CodeLanguage.Equals(CurrentLanguage)).Select(x => x.NameTraslated).First();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los lenguajes: {ex.Message}");
            }
        }

        private async Task<List<LanguageDtoResponse>> updateListText(List<LanguageDtoResponse> list)
        {
            foreach (var languageDto in list) { 
                languageDto.NameTraslated = GetText(languageDto.Name);
            }
            return list;
        }

        private void ChangeLanguage(string value)
        {
            LanguageSelected(value);
            languageDtoResponses = updateListText(languageDtoResponses).Result;
            CurrentLanguage = value;
            DefaultText = languageDtoResponses.Where(s => s.CodeLanguage.Equals(value)).Select(x => x.NameTraslated).First();
        }


        #endregion Methods
    }
}