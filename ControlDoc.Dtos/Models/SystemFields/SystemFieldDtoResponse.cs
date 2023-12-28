using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDoc.Models.Models.SystemFields
{
    public class SystemFieldDtoResponse
    {
        public int SystemParamId { get; set; }

        public string? Code { get; set; }

        public string? Value { get; set; }

        public SystemParamDtoResponse? SystemParam { get; set; }
    }
}