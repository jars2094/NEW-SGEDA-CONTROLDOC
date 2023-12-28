using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDoc.Models.Models.Documents.Response
{
    public class VBehaviorTypologyResponse
    {
        public int DocumentaryTypologyId { get; set; }

        public int DocumentaryTypologyBehaviorId { get; set; }

        public string BehaviorCode { get; set; } = null!;

        public string BehaviorName { get; set; } = null!;

        public string BehaviorValue { get; set; } = null!;
    }
}
