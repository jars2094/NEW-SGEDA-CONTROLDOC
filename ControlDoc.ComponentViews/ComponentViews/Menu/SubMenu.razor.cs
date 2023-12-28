using ControlDoc.Models.Enums;
using ControlDoc.Models.Models.Components.Language.Response;
using ControlDoc.Models.Models.ComponentViews.Menu.Request;
using ControlDoc.Services.Interfaces;
using ControlDoc.Services.Services;
using Microsoft.AspNetCore.Components;
using System.Reflection.Emit;
using System.Text.Json;

namespace ControlDoc.ComponentViews.ComponentViews.Menu
{
    public partial class SubMenu
    {
        #region Variables
        #region Injects
        [Inject] 
        private ICallService CallService { get; set; }
        #endregion
        #endregion

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
        /// Se Obtienen los menús desde API y los almacena en la lista Menus.
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            var respuesta = await CallService.Get<List<MenuModels>>("access/Access/menus", null);

            if (respuesta.Succeeded)
            {
                MenusModels = respuesta.Data;

            }
            else
            {
                Console.WriteLine("No hay menus disponibles");
            }
        }


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
        private void ToggleSubMenu(int id, int level)
        {
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
    }
}
