using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRSReportGen.entity
{
   abstract class EntityBase
    {

        public DateTime DateAdded { get; set; }
        public DateTime LastUpdated { get; set; }
        public EntityStatus RecordStatus { get; set; }
    }
}
