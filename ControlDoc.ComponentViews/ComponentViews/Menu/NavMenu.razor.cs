using ControlDoc.Models.Models.ComponentViews.Menu.Request;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ControlDoc.ComponentViews.ComponentViews.Menu
{
    public partial class NavMenu : ComponentBase
    {
        #region Inyeccion y Parametros

        // Inyección de IJSRuntime para interactuar con JavaScript.
        [Inject] private IJSRuntime JSRuntime { get; set; }



        // Parametros para mostrar y controlar el menu desplegable.
        [Parameter] public bool ShowNewMenu { get; set; }
        [Parameter] public EventCallback<bool> ToggleNewMenu { get; set; }

        [Parameter] public bool CloseMenu { get; set; }

        private bool secure = true;
        private int activeSubMenuIndex = -1; // Inicialmente no hay ningún submenú activo
        private SidebarItems iconOpen = null;

        #endregion

        #region Metodos


        /// <summary>
        /// Cambia la visibilidad del menu desplegable.
        /// </summary>
        private void ToggleNewMenuVisibility()
        {

            // Verifica si hay algún elemento activo en el menú principal o si el logout está activo
            bool itemActive = false; // Inicializamos la variable como falsa
            SidebarItems itemOpen = null;

            foreach (var item in sidebarItemsList)
            {
                if (item.Active)
                {
                    itemActive = true;
                    itemOpen = item; // Si encontramos uno, actualizamos la variable a verdadera
                    break; // Si encontramos uno, podemos salir del bucle
                }
            }

            if (itemActive)
            {
                itemOpen.Active = false;
                activeSubMenuIndex = -1;
                ShowNewMenu = !ShowNewMenu;
                ToggleNewMenu.InvokeAsync(ShowNewMenu);
            }
            else
            {
                /*// Restablece el estado de todos los menús antes de abrirlos
                subMenu.CloseMenus();*/
                ShowNewMenu = !ShowNewMenu;
                ToggleNewMenu.InvokeAsync(ShowNewMenu);

            }

        }

        /// <summary>
        /// Esta es una lista de iconos que se mostraran en el menu, aqui no se 
        /// encuentra el icono del menu desplegable.
        /// </summary>
        public readonly List<SidebarItems> sidebarItemsList = new()
        {
            new SidebarItems()
            {
                ItemName="Inicio",
                IconPath="1",
                Active=false,
                ImageChange=false,

            },
            new SidebarItems()
            {
                ItemName="Radicación",
                IconPath="2",
                Active=false,
                ImageChange=false,
                SubItems=new List<SidebarSubItems>
                {
                    new SidebarSubItems() {
                    ItemName="Radicación Ventanilla Recibida",
                    IconPath="2.1",
                    Active=false,
                    ImageChange=false},

                    new SidebarSubItems() {
                    ItemName="Radicación Ventanilla Interna",
                    IconPath="2.2",
                    Active=false,
                    ImageChange=false},

                    new SidebarSubItems() {
                    ItemName="Radicación Ventanilla Enviada",
                    IconPath="2.3",
                    Active=false,
                    ImageChange=false},

                    new SidebarSubItems() {
                    ItemName="Radicación Ventanilla No Oficial",
                    IconPath="2.4",
                    Active=false,
                    ImageChange=false},

                    new SidebarSubItems() {
                    ItemName="Radicación Ventanilla Rápida",
                    IconPath="2.5",
                    Active=false,
                    ImageChange=false},
                }
            },
            new SidebarItems()
            {
                ItemName = "Gestión",
                IconPath="3",
                Active=false,
                ImageChange=false,
                SubItems = new List<SidebarSubItems>
                {
                    new SidebarSubItems
                    {
                        ItemName = "Gestión",
                        IconPath = "3.1",
                        Active = false,
                        ImageChange = false
                    },
                    new SidebarSubItems
                    {
                        ItemName = "Tablero de Control",
                        IconPath = "3.2",
                        Active = false,
                        ImageChange = false
                    }
                }
            },
            new SidebarItems()
            {
                ItemName = "Tareas Documentales",
                IconPath="4",
                Active=false,
                ImageChange=false,
                SubItems = new List<SidebarSubItems>
                {
                    new SidebarSubItems
                    {
                        ItemName = "Editor de Texto",
                        IconPath = "4.1",
                        Active = false,
                        ImageChange = false
                    },
                    new SidebarSubItems
                    {
                        ItemName = "Hoja de Cálculo",
                        IconPath = "4.2",
                        Active = false,
                        ImageChange = false
                    },
                    new SidebarSubItems
                    {
                        ItemName = "Bandeja de Tareas",
                        IconPath = "4.3",
                        Active = false,
                        ImageChange = false
                    }
                }
            },
            new SidebarItems()
            {
                ItemName = "Expedientes",
                IconPath="5",
                Active=false,
                ImageChange=false,
                SubItems = new List<SidebarSubItems>
                {
                    new SidebarSubItems
                    {
                        ItemName = "Consulta de Expedientes",
                        IconPath = "5.1",
                        Active = false,
                        ImageChange = false
                    },
                    new SidebarSubItems
                    {
                        ItemName = "Administración Expedientes",
                        IconPath = "5.2",
                        Active = false,
                        ImageChange = false
                    },
                    new SidebarSubItems
                    {
                        ItemName = "Solicitud de préstamo",
                        IconPath = "5.3",
                        Active = false,
                        ImageChange = false
                    }
                }
            },
            new SidebarItems()
            {
                ItemName = "Buscadores",
                IconPath="6",
                Active=false,
                ImageChange=false,
                SubItems = new List<SidebarSubItems>
                {
                    new SidebarSubItems
                    {
                        ItemName = "Consulta de Documentos",
                        IconPath = "6.1",
                        Active = false,
                        ImageChange = false
                    },
                    new SidebarSubItems
                    {
                        ItemName = "Búsqueda rápida",
                        IconPath = "6.2",
                        Active = false,
                        ImageChange = false
                    }
                }
            },
            new SidebarItems()
            {
                ItemName = "BPM",
                IconPath="7",
                Active=false,
                ImageChange=false
            },
            new SidebarItems()
            {
                ItemName = "Impacto Ambiental",
                IconPath="8",
                Active=false,
                ImageChange=false
            },

        };

        /// <summary>
        /// Icono de Log-out
        /// </summary>
        SidebarItems logout = new SidebarItems()
        {
            ItemName = "Cerrar sesión",
            IconPath = "9",
            Active = false,
            ImageChange = false
        };

        /// <summary>
        /// Obtiene la ruta de la imagen del icono.
        /// </summary>
        /// <param name="option">Indica si el icono esta seleccionado</param>
        /// <param name="index">Index del icono</param>
        /// <returns>Ruta de la imagen del icono.</returns>
        private string GetIconPath(bool option1, bool option2, string index)
        {

            if (index == "9" && (option1 || option2))
            {
                return $"../icons/icon_logout_selected.png";

            }
            else if (index == "9" && (!option1 || !option2))
            {
                return $"../icons/icon_logout.png";
            }
            else if ((option1 || option2))
            {
                return $"../icons/icon_menu_selected_{index}.png";
            }
            else
            {
                return $"../icons/icon_menu_{index}.png";

            }
        }



        /// <summary>
        /// Cambia el estado de los elementos del menu.
        /// </summary>
        /// <param name="index">Index del elemento a activar</param>
        private void Change(int index)
        {
            // Obtiene el elemento en la posición del índice
            var item = sidebarItemsList[index];

            // Cambia el valor de ImageChange al contrario
            item.ImageChange = !item.ImageChange;

        }

        /// <summary>
        /// Aqui se da el estado de activo al icono que se selecciono para que se muestren
        /// las opciones de este.
        /// por ahora al no tener nada en los iconos con IconPath 1, 7 y 8, no se activaran estos.
        /// </summary>
        /// <param name="index"></param>
        private void SetActiveItem(int index)
        {
            if (ShowNewMenu)
            {
                ShowNewMenu = !ShowNewMenu;
                ToggleNewMenu.InvokeAsync(ShowNewMenu);

                //como la lista empieza en 0, por ello en vez de 1,7 y 8 son 0,6 y 7, y Verifica si no hay ningún submenú abierto actualmente
                if (index != 0 && index != 6 && index != 7 && activeSubMenuIndex == -1)
                {
                    // Abre el submenú del nuevo elemento
                    sidebarItemsList[index].Active = true;
                    activeSubMenuIndex = index; // Actualiza el índice del submenú activo
                    iconOpen = sidebarItemsList[index];

                }
            }
            else
            {
                if (index != 0 && index != 6 && index != 7)
                {
                    // Verifica hay algun submenú abierto actualmente
                    if (activeSubMenuIndex != -1)
                    {
                        int indexOpen = sidebarItemsList.IndexOf(iconOpen);
                        sidebarItemsList[indexOpen].Active = false;
                        iconOpen = null;
                        // Abre el submenú del nuevo elemento
                        sidebarItemsList[index].Active = true;
                        activeSubMenuIndex = index; // Actualiza el índice del submenú activo
                        iconOpen = sidebarItemsList[index];
                    }
                    else
                    {
                        // Abre el submenú del nuevo elemento
                        sidebarItemsList[index].Active = true;
                        activeSubMenuIndex = index; // Actualiza el índice del submenú activo
                        iconOpen = sidebarItemsList[index];
                    }
                }
            }
        }


        private void Change_logout()
        {

            logout.ImageChange = !logout.ImageChange;


        }



        public void CloseSubMenu(int index)
        {
            if (index == activeSubMenuIndex)
            {
                sidebarItemsList[index].Active = false;
                activeSubMenuIndex = -1; // No hay submenú activo
                iconOpen = null;
            }
        }


        // Lógica para cerrar el menú cuando CloseMenu cambia a true
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (CloseMenu && secure && activeSubMenuIndex != -1)
            {
                CloseMenu = false;
                secure = false;
                sidebarItemsList[activeSubMenuIndex].Active = false;
                activeSubMenuIndex = -1; // No hay submenú activo
                iconOpen = null;

            }
            else if (!secure && activeSubMenuIndex != -1)
            {
                CloseMenu = !CloseMenu;
                secure = true;

            }
            else
            {
                // Solo cambia secure a true si está actualmente en false
                if (secure)
                {
                    CloseMenu = !CloseMenu;
                    secure = true;
                }
                else
                {
                    CloseMenu = false;
                }
            }

        }


        #endregion

    }
}
