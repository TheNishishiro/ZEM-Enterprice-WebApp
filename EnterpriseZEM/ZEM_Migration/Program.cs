using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EnterpriseZEM.db.tables;

namespace ZEM_Migration
{

    class Program
    {
        static void Main(string[] args)
        {
            using (var _db = new oldDbContext())
            {
                List<VTMagazyn> vtToDostawas = _db.VTMagazyn.ToList();
            }
        }
    }
}
