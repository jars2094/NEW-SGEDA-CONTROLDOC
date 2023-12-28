using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDoc.Models.Models.Documents.Response
{
    public class VDocumentaryTaskDtoResponse
    {
        public bool? ViewState { get; set; }

        public bool? Indicted { get; set; }

        public int TaskId { get; set; }

        public int? ControlId { get; set; }

        public string? Class { get; set; }

        public string UserTaskName { get; set; } = null!;

        public string? UserForwardName { get; set; }

        public DateTime TaskDate { get; set; }

        public string? TaskDescription { get; set; }

        public string? Instruction { get; set; }

        public string? Process { get; set; }
    }
}
