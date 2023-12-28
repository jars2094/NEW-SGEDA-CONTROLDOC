using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDoc.Models.Models.Generic
{
    public class ModalClosedEventArgs
    {
        public bool IsAccepted { get; set; }
        public bool IsCancelled { get; set; }

        public bool ModalStatus { get; set; }
    }
}
