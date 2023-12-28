using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDoc.Models.Models.Administration.Response
{
    public class ProfileUsersDtoResponse
    {
        public int ProfileId { get; set; }
        public string ProfileCode { get; set; }
        public string Profile1 { get; set; }
        public string Description { get; set; }
        public bool ActiveState { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }

    }
}
