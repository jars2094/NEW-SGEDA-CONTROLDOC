using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDoc.Models.Models.Generic
{
    public class DataCardsDtoResponse
    {
        public int withoutProcessing { get; set; }
        public int inProgress { get; set; }
        public int successfulManagement { get; set; }
    }
}
