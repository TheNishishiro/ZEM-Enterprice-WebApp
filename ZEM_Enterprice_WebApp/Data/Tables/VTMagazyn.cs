using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZEM_Enterprice_WebApp.Data.Tables
{
    public class VTMagazyn
    {
        [Key]
        public Guid VTMagazynId { get; set; }
        public int NumerKompletu { get; set; }
        public int SztukiZeskanowane { get; set; }
        public int SztukiDeklarowane { get; set; }
        public int ZeskanowanychNaKomplet { get; set; }
        public int NaKomplet { get; set; }
        public string Wiazka { get; set; }
        public string KodCiety { get; set; }
        public string Pracownik { get; set; }
        public string DokDostawy { get; set; }
        public DateTime DataUtworzenia { get; set; }
        public DateTime DataDostawy { get; set; }
        public bool Komplet { get; set; }
        public bool Deklarowany { get; set; }
        public bool autocompleteEnabled { get; set; }
        public bool wymuszonaIlosc { get; set; }
        public DateTime? DataDopisu { get; set; }
        public int DopisanaIlosc { get; set; }
        public string DostawaDopis { get; set; }
        public string Uwagi { get; set; }
        public Technical Technical { get; set; }
        public ICollection<VtToDostawa> Dostawy { get; set; }
    }
}
