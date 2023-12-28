using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDoc.Models.Models.Administration.Response
{
    public class ThirdUserDtoResponse
    {
        public int ThirdPartyId { get; set; }
        public int ThirdUserId { get; set; }

        public string IdentificationType { get; set; }
        public string IdentificationTypeName { get; set; }
        public string IdentificationNumber { get; set; }

        public string Names { get; set; }

        public string Email { get; set; }
        public bool ActiveState { get; set; }
        public bool Selected { get; set; }

        public bool Copy { get; set; }
    }
}