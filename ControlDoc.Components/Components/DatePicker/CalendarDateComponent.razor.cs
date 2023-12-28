using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDoc.Components.Components.DatePicker
{
    public partial class CalendarDateComponent
    {
        DateTime datePickerValue { get; set; } = DateTime.Now;
        public DateTime Min = new DateTime(1990, 1, 1, 8, 15, 0);
        public DateTime Max = new DateTime(2025, 1, 1, 19, 30, 45);
    
    }
}
