using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDoc.Models.Models.Administration.Request
{
    public class ProductionOfficeUpdateDtoRequest
    {
        public int AdministrativeUnitId { get; set; }
        public int? BossId { get; set; }
        public string Name { get; set; } = null;
        public string Description { get; set; } = null;
        public bool? ActiveState { get; set; }
        public string UpdateUser { get; set; } = null;
    }
}