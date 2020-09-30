using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseZEM.db.tables
{
    public class ScannedKanban
    {
        [Key]
        public long ScannedKanbanId { get; set; }
        public string Kod { get; set; }
        public string Wiazka { get; set; }
        public DateTime DataDodania { get; set; }
        public string User { get; set; }
    }
}
