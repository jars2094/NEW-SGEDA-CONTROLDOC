using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDoc.Models.Models.Administration.Response
{
    public class StateDtoResponse
    {
        public int countryId { get; set; }
        public int stateId { get; set; }
        public string codeNum { get; set; }
        public string codeTxt { get; set; }
        public string name { get; set; }

    }
}
