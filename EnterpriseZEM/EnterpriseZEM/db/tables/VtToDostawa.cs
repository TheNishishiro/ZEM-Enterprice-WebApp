using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseZEM.db.tables
{
    public class VtToDostawa
    {
        public Guid DostawaId { get; set; }
        public Dostawa Dostawa { get; set; }
        public Guid VTMagazynId { get; set; }
        public VTMagazyn VTMagazyn { get; set; }
    }
}
