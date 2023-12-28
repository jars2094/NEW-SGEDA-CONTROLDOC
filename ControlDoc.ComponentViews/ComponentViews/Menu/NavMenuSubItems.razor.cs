using ControlDoc.Models.Models.ComponentViews.Menu.Request;
using Microsoft.AspNetCore.Components;

namespace ControlDoc.ComponentViews.ComponentViews.Menu
{
    public partial class NavMenuSubItems : ComponentBase
    {
        [Parameter] public SidebarItems item { get; set; }
        [Parameter] public List<SidebarSubItems> sidebarItemsList { get; set; }

        private int activeSubMenuIndex = -1; // Inicialmente no hay ningún submenú activo

        /// <summary>
        /// Obtiene la ruta de la imagen del icono.
        /// </summary>
        /// <param name="option">Indica si el icono esta seleccionado</param>
        /// <param name="index">Index del icono</param>
        /// <returns>Ruta de la imagen del icono.</returns>
        private string GetIconPath(bool option, string index)
        {
            if (option && index == "9")
            {
                return $"../icons/icon_logout_selected.png";

            }
            else if (!option && index == "9")
            {
                return $"../icons/icon_logout.png";
            }
            else if (option)
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
            sidebarItemsList.ForEach(
                m => { m.ImageChange = false; }
            );


            sidebarItemsList[index].ImageChange = true;

        }

        private void SetActiveItem(int index)
        {
            // Verifica si no hay ningún submenú abierto actualmente
            if (activeSubMenuIndex == -1)
            {
                // Abre el submenú del nuevo elemento
                sidebarItemsList[index].Active = true;
                sidebarItemsList[index].ImageChange = true;
                activeSubMenuIndex = index; // Actualiza el índice del submenú activo
            }
        }
    }
}
