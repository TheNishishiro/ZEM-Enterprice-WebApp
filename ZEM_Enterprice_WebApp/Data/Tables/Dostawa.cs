using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZEM_Enterprice_WebApp.Data.Tables
{
    public class Dostawa
    {
        [Key]
        public string KodIloscData { get; set; }
        public string Kod { get; set; }
        public int Ilosc { get; set; }
        public DateTime Data { get; set; }
        public DateTime DataUtworzenia { get; set; }
        public string Uwagi { get; set; }
        public Technical Technical { get; set; }
        public ICollection<VtToDostawa> Skany { get; set; }
    }
}
