using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDoc.Models.Models.Administration.Response
{
    public class ProductionOfficesDtoResponse
    {
        public int ProductionOfficeId { get; set; }

        public int AdministrativeUnitId { get; set; }

        public string AdministrativeUnitName { get; set; }

        public int? BossId { get; set; }

        public string BossName { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool? ActiveState { get; set; }
    }
}