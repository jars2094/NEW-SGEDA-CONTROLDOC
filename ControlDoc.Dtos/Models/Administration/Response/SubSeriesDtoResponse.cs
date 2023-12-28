using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDoc.Models.Models.Administration.Response
{
    public class SubSeriesDtoResponse
    {
        public int SubSeriesId { get; set; }
        public int SeriesId { get; set; }
        public string SeriesName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; } = null;
        public string Description { get; set; } = null;
        public bool ActiveState { get; set; }
        public string UpdateUser { get; set; }
    }
}
