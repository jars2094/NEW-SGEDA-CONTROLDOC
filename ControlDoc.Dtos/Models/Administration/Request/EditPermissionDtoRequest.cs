using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDoc.Models.Models.Administration.Request
{
    public class EditPermissionDtoRequest
    {
        public bool accessF { get; set; }
        public bool createF { get; set; }
        public bool modifyF { get; set; }
        public bool consultF { get; set; }
        public bool deleteF { get; set; } = false;
        public bool printF { get; set; }
        public bool activeState { get; set; }
    }
}
