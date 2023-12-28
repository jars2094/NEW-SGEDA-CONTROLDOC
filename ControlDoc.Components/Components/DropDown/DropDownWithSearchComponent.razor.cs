using Microsoft.AspNetCore.Components;

namespace ControlDoc.Components.Components.DropDown
{
    public partial class DropDownWithSearchComponent : ComponentBase
    {
        [Parameter] public Dictionary<string, string> DataSource { get; set; } = new();
        [Parameter] public EventCallback<string> OnValueEntered { get; set; }
        [Parameter] public bool HideOnSelect { get; set; }
        [Parameter] public string Title { get; set; } = string.Empty;
        [Parameter] public string Placeholder { get; set; } = string.Empty;

        private Dictionary<string, string> dataCopy { get; set; } = new();
        private string? search;
        private string? title;
        private bool show;

        protected override void OnInitialized()
        {
            Placeholder = "Buscar...";
            title = Title;
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                DataSource = DataSource
                    .OrderBy(order => order.Value)
                    .ToDictionary(value => value.Key, value => value.Value);

                dataCopy = DataSource;
            }
        }

        private void HandleClick() => show = !show;

        private async Task SelectValue(string key)
        {
            title = DataSource.FirstOrDefault(item => item.Key.Equals(key)).Value;
            await OnValueEntered.InvokeAsync(key);

            if (HideOnSelect) { HandleClick(); }
        }

        private void HandleInputChange(ChangeEventArgs e)
        {
            dataCopy = DataSource;

            search = e?.Value?.ToString();
            if (!string.IsNullOrEmpty(search))
            {
                dataCopy = dataCopy
                    .Where(Predicate(search))
                    .OrderBy(ord => ord.Value)
                    .ToDictionary(value => value.Key, value => value.Value);
            }
        }

        private static Func<KeyValuePair<string, string>, bool> Predicate(string search)
        {
            return x => x.Value.Contains(search, StringComparison.OrdinalIgnoreCase);
        }
    }
}
