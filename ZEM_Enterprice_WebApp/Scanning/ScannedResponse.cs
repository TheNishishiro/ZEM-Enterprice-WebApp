using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZEM_Enterprice_WebApp.Scanning
{
    /// <summary>
    /// Class used to store informations which are supposed to be returned after scan have been successful
    /// </summary>
    public class ScannedResponse
    {
        public string PrzewodCiety { get; set; }
        public string Rodzina { get; set; }
        public string Wiazka { get; set; }
        public string BIN { get; set; }
        public string KodWiazki { get; set; }
        public string LiteraRodziny { get; set; }
        public string IlePrzewodow { get; set; }
        public DateTime DataDostawy { get; set; }
        public DateTime DataDostawyOld { get; set; }
        public int numToComplete { get; set; }
        public int numScanned { get; set; }
        public int numScannedToComplete { get; set; }
        public bool print { get; set; }
        public bool isComplete { get; set; }
        public int sztukiDeklatowane { get; set; }
        public int sztukiSkanowane { get; set; }
        public int numerKompletu { get; set; }
        public FlagType Flag { get; set; }
        public HeaderTypes Header { get; set; }
        public List<string> Args { get; set; }
    }
}
