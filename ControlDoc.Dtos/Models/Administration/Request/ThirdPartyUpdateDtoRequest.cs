using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDoc.Models.Models.Administration.Request
{
    public class ThirdPartyUpdateDtoRequest
    {
        public string IdentificationType { get; set; }
        public string IdentificationNumber { get; set; }
        public string Names { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool ActiveState { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string WebPage { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string ChargeCode { get; set; }
        public string Initials { get; set; }
        public string NatureCode { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public AddressDtoRequest Address { get; set; }
    }
}
