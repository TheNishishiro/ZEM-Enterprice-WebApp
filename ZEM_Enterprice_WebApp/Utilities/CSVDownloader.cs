using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZEM_Enterprice_WebApp.Data;


namespace ZEM_Enterprice_WebApp.Utilities
{
    public class CSVDownloader
    {
        public static async Task<MemoryStream> OnPostDownloadCsvAsync<T>(ApplicationDbContext _db, IQueryable<T> query)
        {
            StringBuilder sb = new StringBuilder();
            MemoryStream ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.AutoFlush = true;
            var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(await query.ToListAsync());
            await ms.FlushAsync();
            await csv.FlushAsync();
            ms.Position = 0;

            return ms;
        }
    }
}
