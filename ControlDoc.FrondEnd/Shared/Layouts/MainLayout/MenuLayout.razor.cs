using ControlDoc.Components.Components.DropDown;
using ControlDoc.Components.Components.Modals;
using ControlDoc.ComponentViews.ComponentViews.Menu;
using ControlDoc.Models.Enums;
using ControlDoc.FrondEnd.Shared.GlobalComponents.Spinner;
using ControlDoc.Models.Models.Administration.Response;
using ControlDoc.Models.Models.ComponentViews.Menu.Request;
using ControlDoc.Services.Interfaces;
using ControlDoc.Services.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Reflection.Metadata;
using Telerik.Blazor.Components.Menu;

namespace ControlDoc.FrondEnd.Shared.Layouts.MainLayout
{
    public partial class MenuLayout
    {
        #region Variables
        #region Injects
        [Inject]
        private EventAggregatorService EventAggregator { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }
        
        [Inject]
        private RenewTokenService RenewToken { get; set; }

        [Inject] 
        private ICallService CallService { get; set; }

        [Inject]
        private ISessionStorage SessionStorage { get; set; }

        [Inject]
        private IJSRuntime JSRuntime { get; set; }
        #endregion

        #region Parameter
        [CascadingParameter] 
        private Task<AuthenticationState>? authenticationState { get; set; }
        #endregion
        #endregion


        private bool isLoading = true;

        public SpinnerCargandoComponent spinerLoader;

        public void UpdateSpinner(bool newValue)
        {
            spinerLoader.UpdateLoadingStatus(newValue);
        }

        protected override async Task OnInitializedAsync()
        {
            // Carga tus datos aquí
            // Simula una carga de datos
            var authState = await authenticationState;
            if (!authState.User.Identity.IsAuthenticated)
            {
                NavigationManager.NavigateTo("");
            }
            else
            {
                var timeExpiration = await SessionStorage.GetValue<string>(ValuesKeys.TimeExpiration);
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(timeExpiration));
                var tiempoExpiracionString = dateTimeOffset.LocalDateTime.ToString();
                var timeExpirationMinute = DateTime.Parse(tiempoExpiracionString).Minute - DateTime.Now.Minute;
                RenewToken.Start(timeExpirationMinute);
            }

            await JSRuntime.InvokeVoidAsync("checkTheme"); // Verificar Theme almacenado en el localStorage               
            isLoading = false; // Actualiza la variable de estado
        }

        private bool showNewMenu = false;
        private bool closeMenu = false;

        private string subNavFiling = "d-none";
        private string subNavManagement = "d-none";
        private string subNavDocumentaryTasks = "d-none";
        private string subNavRecord = "d-none";
        private string subNavSearchers = "d-none";
        private SubMenu subMenuInstance;
        protected override async void OnInitialized()
        {
            EventAggregator.LanguageChangedEvent += HandleLanguageChanged;
           // string hola = DropDownLanguageComponent.GetText("Politicas");
            //await JSRuntime.InvokeVoidAsync("changeFooter", DropDownLanguageComponent.GetText("Politicas"), DropDownLanguageComponent.GetText("TerminosyCondiciones"));
            await GetMenus();
        }

        private async Task GetMenus()
        {
            try
            {
                var response = await CallService.Get<List<MenuModels>>("access/Access/ByFilter", null);
                MenusModels = response.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los perfiles de usuario: {ex.Message}");
            }
        }

        private async Task HandleLanguageChanged()
        {
            StateHasChanged();
        }

        private void ToggleNewMenu()
        {
            showNewMenu = !showNewMenu;
        }


        string ThreeLinesImage = "../img/menu/tresLineas.svg";
        string HomeImage = "../img/menu/inicio.svg";

        #region Acciones Radicacion
        string FilingImage = "../img/menu/radicacion.svg";
        string FilingReceived = "../img/menu/ventanillaRecibida.svg";
        string FilingInternal = "../img/menu/ventanillaInterna.svg";
        string Filingsent = "../img/menu/ventanillaEnviada.svg";
        string FilingUnofficial = "../img/menu/ventanillaNoOficial.svg";
        string FilingFaster = "../img/menu/ventanillaRapida.svg";

        #endregion


        #region Gestion
        string ManagementImage = "../img/menu/gestion.svg";
        string ManagementTray = "../img/menu/gestionsubItem.svg";
        string ManagementBoard = "../img/menu/tableroControl.svg";
        #endregion

        #region Tareas Documentales
        string DocumentaryTasksImage = "../img/menu/tareasDocumentales.svg";
        string DocumentaryTasksEditor = "../img/menu/editorTexto.svg";
        string DocumentaryTasksSpreadsheet = "../img/menu/hojaCalculo.svg";
        string DocumentaryTasksTray = "../img/menu/bandejaTareasSubItem.svg";
        #endregion

        #region Tareas Documentales
        string RecordImage = "../img/menu/expedientes.svg";
        string RecordConsult = "../img/menu/consultaExpediente.svg";
        string RecordAdministration = "../img/menu/administraacionExp.svg";
        string RecordRequest = "../img/menu/solicitudPrestamo.svg";
        #endregion

        #region Buscadores
        string SearchersImage = "../img/menu/buscadores.svg";
        string SearchersConsult = "../img/menu/consultaExpediente.svg";
        string SearchersSearch = "../img/menu/administraacionExp.svg";
        
        #endregion
        
        string BpmImage = "../img/menu/bpm.svg";
        string EnvironmentalImpactImage = "../img/menu/impactoAmbiental.svg";
        string LogoutImage = "../img/menu/cerrarSesion.svg";

        private void OnMouseOverWithSubItem(ref string image, ref string subnavItem)
        {
            image = image.Replace(".svg", "Hover.svg");
            subnavItem = "";
        }

        private void OnMouseOutWithSubItem(ref string image, ref string subnavItem)
        {
            image = image.Replace("Hover.svg", ".svg");
            subnavItem = "d-none";
        }

        private void OnMouseOver(ref string image)
        {
            image = image.Replace(".svg", "Hover.svg");
           
        }

        private void OnMouseOut(ref string image)
        {
            image = image.Replace("Hover.svg", ".svg");
            
        }
        private void OnNavitigionMenu(string Page)
        {
            NavigationManager.NavigateTo("/" + Page);
        }

        private string DocumentaryTaskPage = "DocumentsTask";

        private string FilingReceivedPage = "RadicationReceived";

        private string ManagementTrayPage = "ManagementTray";  


        //SUBMENU
        #region Atributos

        // Lista de Menus de la BD
        private List<MenuModels> MenusModels { get; set; } = new List<MenuModels>();

        // Diccionario que agrupa los diccionarios anteriores por nivel.
        private Dictionary<int, Dictionary<int, bool>> expandedItemsByLevel = new Dictionary<int, Dictionary<int, bool>>
        {
            { 1, new Dictionary<int, bool>() },
            { 2, new Dictionary<int, bool>() },
            { 3, new Dictionary<int, bool>() },
            { 4, new Dictionary<int, bool>() },
        };


        #endregion

        #region Metodos



        /// <summary>
        /// Verifica si un menu esta expandido en un nivel determinado (menu principal o subniveles de este)
        /// </summary>
        /// <param name="id">El id del menu o submenu</param>
        /// <param name="level">El nivel en el que se desea verificar la expansion</param>
        /// <returns>Si el menu o submenu está expandido en el nivel especificado sera True; de lo contrario, false.</returns>
        private bool IsExpanded(int id, int level)
        {
            if (expandedItemsByLevel.TryGetValue(level, out var expandedItems))
            {
                return expandedItems.ContainsKey(id) && expandedItems[id];
            }

            return false;
        }

        /// <summary>
        /// Verifica si esta expandido y cierra otros niveles
        /// </summary>
        /// <param name="id">El id del menu o submenu</param>
        /// <param name="level">El nivel en el que se desea verificar la expansion</param>
        private void ToggleSubMenu(int id, int level, string NameView)
        {
            if (!string.IsNullOrEmpty(NameView))
            {
                NavigationManager.NavigateTo("/" + NameView);
                showNewMenu = false;
            }
            // Antes de abrir un nuevo menu, cierra los menus previamente abiertos.
            CloseMenusInSameOrLowerLevel(id, level);
            // Cambia el estado de expansion del menú seleccionado.
            ToggleExpansion(id, level);
        }

        

        /// <summary>
        /// Cierra los menús y niveles anteriormente abiertos.
        /// </summary>
        /// <param name="id">El id del menu o submenu</param>
        /// <param name="level">El nivel en el que se desea verificar la expansion</param>
        public void CloseMenusInSameOrLowerLevel(int id, int level)
        {

            // Cerrar todos los menús en niveles iguales o inferiores.
            foreach (var kvp in expandedItemsByLevel)
            {
                var menuLevel = kvp.Key;
                var expandedItems = kvp.Value;

                if (menuLevel >= level)
                {
                    foreach (var key in expandedItems.Keys.ToList())
                    {
                        if (key != id && expandedItems[key])
                        {
                            expandedItems[key] = false;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Cambia el estado de expansion del menu en el nivel especificado.
        /// </summary>
        /// <param name="id">El id del menu o submenu</param>
        /// <param name="level">El nivel en el que se desea verificar la expansion</param>
        private void ToggleExpansion(int id, int level)
        {
            for (int i = 1; i <= level; i++)
            {
                if (!expandedItemsByLevel.ContainsKey(i))
                {
                    expandedItemsByLevel[i] = new Dictionary<int, bool>();
                }
            }
            if (expandedItemsByLevel.TryGetValue(level, out var expandedItems))
            {
                if (expandedItems.ContainsKey(id))
                {
                    expandedItems[id] = !expandedItems[id];
                }
                else
                {
                    expandedItems[id] = true;
                }
            }
        }

        /// <summary>
        /// Verifica si un menu tiene submenus/opciones de nivel 1 o si esta activo.
        /// </summary>
        /// <param name="menu">Objeto de tipo Menu</param>
        /// <returns>True si el MenuItems1s del menu no esta vacio</returns>
        private bool HasSubMenuItems(MenuModels menu)
        {
            return menu.MenuItems1s != null;
        }

        /// <summary>
        /// Verifica si un submenú de nivel 1 tiene una vista, si no es asi significa que tiene opciones.
        /// </summary>
        /// <param name="menuItem1">Objeto de tipo menuItem1</param>
        /// <returns>True si el menuItem1 tiene una vista null</returns>
        private bool HasSubMenuItems(MenuItems1 menuItem1)
        {
            return menuItem1.View == null;
        }

        /// <summary>
        /// Verifica si un submenu de nivel 2 tiene una vista, si no es asi significa que tiene opciones.
        /// </summary>
        /// <param name="menuItem2">Objeto de tipo menuItem2</param>
        /// <returns>True si el menuItem2 tiene una vista null</returns>
        private bool HasSubMenuItems(MenuItems2 menuItem2)
        {
            return menuItem2.View == null;
        }
        #endregion

        #region Dark-Mode
        async Task ToggleTheme()
        {
            await JSRuntime.InvokeVoidAsync("toggleTheme");
        }
        #endregion
    }
}
