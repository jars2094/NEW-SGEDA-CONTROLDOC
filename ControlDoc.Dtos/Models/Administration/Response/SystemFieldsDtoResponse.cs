using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDoc.Models.Models.Administration.Response
{
    public class SystemFieldsDtoResponse
    {
        public int systemParamId { get; set; }
        public string code { get; set; }
        public string value { get; set; }
        public SystemParam systemParam { get; set; }

        public class SystemParam
        {
            public string paramCode { get; set; }
            public string paramName { get; set; }
        }
    }
}
