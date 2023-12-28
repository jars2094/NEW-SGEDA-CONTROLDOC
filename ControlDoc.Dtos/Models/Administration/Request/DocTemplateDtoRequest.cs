using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDoc.Models.Models.Administration.Request
{
    public class DocTemplateDtoRequest
    {
        public string TempCode { get; set; }
        public string TempName { get; set; }

        public string TempType { get; set; }
        public string? Process { get; set; }
        public bool ActiveState { get; set; }
        public string Archivo { get; set; }
    }
}
