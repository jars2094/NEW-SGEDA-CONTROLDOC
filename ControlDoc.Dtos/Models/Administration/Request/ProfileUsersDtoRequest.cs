using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDoc.Models.Models.Administration.Request
{
    public class ProfileUsersDtoRequest
    {
        public string? Profile1 { get; set; }
        public string? ProfileCode { get; set; }
        public string? Description { get; set; }
        public bool ActiveState { get; set; }
    }

    public class ProfileUsersDtoRequestUpdate
    {
        public string? Profile1 { get; set; }
        public string? Description { get; set; }
        public bool ActiveState { get; set; }
    }
}