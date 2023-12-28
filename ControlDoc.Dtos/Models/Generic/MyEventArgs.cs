using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDoc.Models.Models.Generic
{
    public class MyEventArgs<T>
    {
        public T Data { get; set; }
        public bool ModalStatus { get; set; }
    }
}
