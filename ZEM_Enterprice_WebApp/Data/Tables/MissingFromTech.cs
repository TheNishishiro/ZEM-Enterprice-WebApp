using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZEM_Enterprice_WebApp.Data.Tables
{
    public class MissingFromTech
    {
        [Key]
        public string Kod { get; set; }
        public DateTime DataDodania { get; set; }
        public string User { get; set; }
    }
}
