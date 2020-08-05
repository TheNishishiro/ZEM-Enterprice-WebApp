using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZEM_Enterprice_WebApp.Data.Tables
{
    public class PendingChangesTechnical
    {
        [Key]
        public Guid PendingChangesTechnicalId { get; set; }
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
        public string DataModyfikacji { get; set; }
    }
}
