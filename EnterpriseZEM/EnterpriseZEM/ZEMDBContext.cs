using EnterpriseZEM.db.tables;
using EnterpriseZEM_Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EnterpriseZEM_Common.Settings;

namespace EnterpriseZEM
{
    class ZEMDBContext : DbContext
    {
        public DbSet<Technical> Technical { get; set; }
        public DbSet<Dostawa> Dostawa { get; set; }
        public DbSet<VTMagazyn> VTMagazyn { get; set; }
        public DbSet<PendingChangesTechnical> PendingChangesTechnical { get; set; }
        public DbSet<MissingFromTech> MissingFromTech { get; set; }
        public DbSet<ScanCache> ScanCache { get; set; }
        public DbSet<ScannedKanban> ScannedKanbans { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Settings.Properties[FieldTypes.ConnectionString.ToString()]).EnableSensitiveDataLogging();
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<VTMagazyn>()
                .HasOne(d => d.Technical)
                .WithMany(c => c.VTMagazyns)
                .IsRequired();

            builder.Entity<Dostawa>()
                .HasOne(d => d.Technical)
                .WithMany(c => c.Dostawas)
                .IsRequired();

            builder.Entity<VtToDostawa>().HasKey(c => new { c.DostawaId, c.VTMagazynId });
            builder.Entity<VtToDostawa>()
                .HasOne(c => c.Dostawa)
                .WithMany(c => c.Skany).OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(c => c.DostawaId);
            builder.Entity<VtToDostawa>()
                .HasOne(c => c.VTMagazyn)
                .WithMany(c => c.Dostawy).OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(c => c.VTMagazynId);

            builder.Entity<Technical>().HasQueryFilter(c => c.Deleted == false);
            //builder.Entity<VTMagazyn>().HasQueryFilter(c => c.Deleted == false);

            base.OnModelCreating(builder);
        }
    }
}
