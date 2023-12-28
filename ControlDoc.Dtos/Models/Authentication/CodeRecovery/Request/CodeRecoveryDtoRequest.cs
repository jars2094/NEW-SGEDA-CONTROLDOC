using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDoc.Models.Models.Authentication.CodeRecovery.Request
{
    public class CodeRecoveryDtoRequest
    {
        public string? code { get; set; }
        public string? uuid { get; set; }
        public string? ip { get; set; }
    }
}
