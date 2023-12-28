using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDoc.Models.Models.Administration.Request
{
    public class BranchOfficeDtoRequest
    {
        public int AddressId { get; set; }
        public string Code { get; set; }
        public string NameOffice { get; set; }
        public string Region { get; set; }
        public string Territory { get; set; }
    }
}
