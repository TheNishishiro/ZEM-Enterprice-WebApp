using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZEM_Enterprice_WebApp.Data.Tables
{
    public class ScanCache
    {
        [Key]
        public Guid ScanCacheId { get; set; }
        public bool LookedBack { get; set; }
    }
}
