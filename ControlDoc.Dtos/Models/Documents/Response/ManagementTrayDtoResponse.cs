using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDoc.Models.Models.Documents.Response
{
    public class ManagementTrayDtoResponse
    {
        public int controlId { get; set; }
        public string nameDocumentaryTypologiesBag { get; set; }
        public string filingCode { get; set; }
        public string docDescription { get; set; }
        public DateTime docDate { get; set; }
        public DateTime dueDate { get; set; }
        public List<object> documentReceivers { get; set; }
        public List<object> documentSignatories { get; set; }
    }
}
