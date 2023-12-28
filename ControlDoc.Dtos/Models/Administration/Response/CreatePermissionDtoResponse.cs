using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDoc.Models.Models.Administration.Response
{
    public class CreatePermissionDtoResponse
    {
        public int permissionId { get; set; }
        public int functionalityId { get; set; }
        public int profileId { get; set; }
        public bool accessF { get; set; }
        public bool createF { get; set; }
        public bool modifyF { get; set; }
        public bool consultF { get; set; }
        public bool deleteF { get; set; }
        public bool printF { get; set; }
        public bool activeState { get; set; }



        
    }
}
