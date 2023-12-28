using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDoc.Models.Models.Generic
{
    public class FileInfoData
    {
        public string Name { get; set; }
        public string Extension { get; set; }
        public long Size { get; set; }
        public string IconPath { get; set; }
        public string Base64Data { get; set; }
        public string Hash { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
