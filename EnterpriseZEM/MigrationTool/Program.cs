using CsvHelper;
using EnterpriseZEM.db.tables;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigrationTool
{
    class Program
    {
        public static void CreateCache()
        {
            using (var _db = new OldContext())
            {
                List<VTMagazyn> vTMagazyns = _db.VTMagazyn.OrderBy(c => c.DataUtworzenia).ToList();
                List<VtToDostawa> vtToDostawas = _db.VtToDostawa.ToList();
                List<Dostawa> dostawas = _db.Dostawa.ToList();

                using (var writer = new StreamWriter("./VTMAGAZYN.CSV"))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(vTMagazyns);
                }

                using (var writer = new StreamWriter("./VTTODOSTAWA.CSV"))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(vtToDostawas);
                }

                using (var writer = new StreamWriter("./DOSTAWA.CSV"))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(dostawas);
                }
            }
        }


        static void Main(string[] args)
        {
            CreateCache();
        }
    }
}
