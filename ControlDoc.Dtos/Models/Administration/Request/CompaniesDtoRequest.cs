using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ControlDoc.Models.Models.Administration.Request
{
    public class CompaniesDtoRequest
    {
        public string IdentificationType { get; set; }
        public string Identification { get; set; }
        public string BusinessName { get; set; }
        public CompanyData CompanyData { get; set; }
    }
    public class CompanyData
    {
        public string LegalAgentIdType { get; set; }
        public string LegalAgentId { get; set; }
        public string LegalAgentFullName { get; set; }
        public string PhoneNumber { get; set; }
        public string CellPhoneNumber { get; set; }
        public string Email { get; set; }
        public string WebAddress { get; set; }
        public string? Domain { get; set; }
        public AddressDtoRequest Address { get; set; }

    }
}
