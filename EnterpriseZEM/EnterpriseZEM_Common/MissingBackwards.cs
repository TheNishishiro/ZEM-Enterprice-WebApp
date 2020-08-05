using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnterpriseZEM_Common
{
    [Serializable]
    public class MissingBackwards
    {
        public int ID { get; set; }
        public string Wiazka { get; set; }
        public DateTime Data { get; set; }
        public string KodCiety { get; set; }
        public int Zeskanowane { get; set; }
        public int Missing { get; set; }
        public int Ilosc { get; set; }
        public int NrKompletu { get; set; }
        public bool Dopis { get; set; }
        public bool Update { get; set; }
    }
}
