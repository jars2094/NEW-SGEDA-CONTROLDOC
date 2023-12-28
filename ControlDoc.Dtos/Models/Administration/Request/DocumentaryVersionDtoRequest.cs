using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDoc.Models.Models.Administration.Request
{
    public class DocumentaryVersionDtoRequest
    {
        public int companyId { get; set; }
        public string versiontype { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string fileExt { get; set; }
        public string fileName { get; set; }
        public string archivo { get; set; }
        public List<AdministrativeActsDtoRequest> administrativeActs { get; set; }
    }
}
