using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseZEM.db.tables
{
    public class MissingFromTech
    {
        [Key]
        public string Kod { get; set; }
        public DateTime DataDodania { get; set; }
        public string User { get; set; }
    }
}
