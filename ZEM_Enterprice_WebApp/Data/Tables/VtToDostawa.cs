using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZEM_Enterprice_WebApp.Data.Tables
{
    public class VtToDostawa
    {
        public Guid DostawaId { get; set; }
        public Dostawa Dostawa { get; set; }
        public Guid VTMagazynId { get; set; }
        public VTMagazyn VTMagazyn { get; set; }
    }
}
