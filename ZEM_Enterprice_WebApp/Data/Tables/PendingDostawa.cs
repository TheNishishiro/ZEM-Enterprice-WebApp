using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZEM_Enterprice_WebApp.Data.Tables
{
    public class PendingDostawa
    {
        [Key]
        public Guid PendingDostawaId { get; set; }
        public string Kod { get; set; }
        public int Ilosc { get; set; }
        public DateTime Data { get; set; }
        public string Uwagi { get; set; }
    }
}
