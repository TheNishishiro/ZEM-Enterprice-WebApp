using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZEM_Enterprice_WebApp.Data.Tables
{
    public class Technical
    {
        [Key]
        public string CietyWiazka { get; set; }
        public string Rodzina { get; set; }
        public string Wiazka { get; set; }
        public string LiterRodziny { get; set; }
        public string KodWiazki { get; set; }
        public string IlePrzewodow { get; set; }
        public string PrzewodCiety { get; set; }
        public string BIN { get; set; }
        public string IndeksScala { get; set; }
        public bool KanBan { get; set; }
        public string Uwagi { get; set; }
        public string DataUtworzenia { get; set; }
        public DateTime? DeleteDate { get; set; }
        public bool Deleted { get; set; }
        public ICollection<VTMagazyn> VTMagazyns { get; set;  }
        public ICollection<Dostawa> Dostawas { get; set; }
    }
}
