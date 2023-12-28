using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDoc.Models.Models.Generic
{
    public class DataCardsDocTaskDtoResponse
    {
        public int Created { get; set; }
        public int Review { get; set; }
        public int Approve { get; set; }
        public int Signed { get; set; }
        public int Involved { get; set; }
    }
}
