using ControlDoc.Components.Components.DropDown;
using ControlDoc.Services.Services;
using Microsoft.AspNetCore.Components;



namespace ControlDoc.FrondEnd.Shared.Layouts.LayoutAuthentication
{
    public partial class LoginLayout
    {
        #region Variables
        #region Inject

        [Inject]
        private EventAggregatorService EventAggregator { get; set; }

        #endregion
        #endregion

        Dictionary<string, string> source = new();
        private bool Changed = false;

        DateTime date;

        protected override async Task OnInitializedAsync()
        {
            date = new(2020, 10, 10);
            source = cargarValores();

            // Esperar a que DropDownLanguageComponent esté completamente inicializado
            while (DropDownLanguageComponent.LanguageCache == null)
            {
                await Task.Delay(100);
            }

            EventAggregator.LanguageChangedEvent += HandleLanguageChanged;
        }

        private async Task HandleLanguageChanged()
        {
            Changed = true;
            StateHasChanged();
        }

        



        private Dictionary<string, string> cargarValores()
        {
            var test = new Dictionary<string, string>();
            List<string> ciudades = new List<string>
        {
            "New York", "Los Angeles", "Chicago", "Houston", "Phoenix", "Philadelphia", "San Antonio", "San Diego", "Dallas", "San Jose"
        };

            int consecutivo = 1;

            foreach (string ciudad in ciudades)
            {
                test[consecutivo.ToString()] = ciudad + consecutivo;
                consecutivo++;
            }

            return test;
        }

        private void GetCity(string value)
        {
            var city = source
                .Where(item => item.Key == value)
                .FirstOrDefault();
        }

        private void GetDates(Dictionary<string, object> value)
        {
            var result = value;
        }

        private List<string> Countries = new List<string>()
        {
        "Albania",
        "Andorra",
        "Armenia",
        "Austria",
        "Azerbaijan",
        "Belarus",
        "Belgium",
        "Bosnia & Herzegovina",
        "Bulgaria",
        "Croatia",
        "Cyprus",
        "Czech Republic",
        "Denmark",
        "Estonia",
        "Finland",
        "France",
        "Georgia",
        "Germany",
        "Greece",
        "Hungary",
        "Iceland",
        "Ireland",
        "Italy",
        "Kosovo",
        "Latvia",
        "Liechtenstein",
        "Lithuania",
        "Luxembourg",
        "Macedonia",
        "Malta",
        "Moldova",
        "Monaco",
        "Montenegro",
        "Netherlands",
        "Norway",
        "Poland",
        "Portugal",
        "Romania",
        "Russia",
        "San Marino",
        "Serbia",
        "Slovakia",
        "Slovenia",
        "Spain",
        "Sweden",
        "Switzerland",
        "Turkey",
        "Ukraine",
        "United Kingdom",
        "Vatican City"
    };

        private string SelectedValue { get; set; } = "Austria";
    }
}
