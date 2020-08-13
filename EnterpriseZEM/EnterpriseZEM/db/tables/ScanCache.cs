using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseZEM.db.tables
{
    public class ScanCache
    {
        [Key]
        public Guid ScanCacheId { get; set; }
        public bool LookedBack { get; set; }
    }
}
