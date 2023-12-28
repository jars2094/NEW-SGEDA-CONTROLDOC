using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDoc.Components.Components.Users
{
    public partial class UserCardComponent
    {
        [Parameter]
        public string FullName { get; set; } = "Nombre no encontrado";

        [Parameter]
        public string AdministrativeUnitName { get; set; } = "Unidad Administrativa no encontrado";
        [Parameter]
        public string ProductionOfficeName { get; set; } = "Oficina productora no encontrado";
        [Parameter]
        public string Positionname { get; set; } = "Cargo no encontrado";
    }
}
